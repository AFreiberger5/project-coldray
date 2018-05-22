/**************************************************
*  Credits: Created by Alexander Freiberger		  *
*                                                 *
*  Additional Edits by:                           *
*                                                 *
*                                                 *
*                                                 *
***************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class KevinAI : AIBase
{


    private EAIState m_currentState;

    private Helper helper;
    private NavMeshAgent m_agent;
    private Vector3[] positions;
    private bool m_isSearchingCoffe;
    private bool m_foundCoffe;
    private int currentCoffePoint;

    #region AnimatiorVariables
    private Animator m_animator;
    private int IDLiving;
    private int IDMove;
    private int IDFear;
    private int IDDeath;
    private int IDRunning;
    #endregion

    [SyncVar, SerializeField]
    private float m_HP = -1;

    private Dictionary<EDamageType, float> DefenseValues = new Dictionary<EDamageType, float>();

    [ServerCallback]
    private void Awake()
    {
        IDLiving = Animator.StringToHash("IsLiving");
        IDMove = Animator.StringToHash("IsWalking");
        IDFear = Animator.StringToHash("IsFearing");
        IDDeath = Animator.StringToHash("IsDying");
        IDRunning = Animator.StringToHash("IsRunning");

        //Add Defense Values for all Types of DamageSources
        #region AddDefenseValues

        DefenseValues.Add(EDamageType.MELEE, 0);
        DefenseValues.Add(EDamageType.RANGED, 0);
        DefenseValues.Add(EDamageType.MAGICAL, 0);
        DefenseValues.Add(EDamageType.PHYSICAL, 0);
        DefenseValues.Add(EDamageType.TRUE, 0);
        #endregion
    }

    [ServerCallback]
    void Start()
    {
        positions = new Vector3[5];
        currentCoffePoint = 1;
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.destination = transform.position;
        m_agent.autoBraking = false;
    }

    [ServerCallback]
    void Update()
    {
        if (!isLocalPlayer)
            NPCDecision();

        if (m_isSearchingCoffe && m_agent.remainingDistance < 0.1f && !m_agent.pathPending)
        {
            SearchCoffe();
        }
        if (m_foundCoffe)
        {
            //Head in ground
            m_agent.enabled = false;
        }

    }

    private void SearchCoffe()
    {
        //When Kevin gets hit, he panicks and tries to quickly find all his Cofffe(TM) to hide it

        if (currentCoffePoint >= positions.Length)
        {
            m_isSearchingCoffe = false;
            m_foundCoffe = true;
            return;
        }
        m_agent.destination = positions[currentCoffePoint];
        while (m_agent.pathPending)
        {
            //ToDo: Check if this while causes issues or not
            //wait for the path to be completed
        }
        if (m_agent.path == null)
        {
            NavMeshHit NHit;
            NavMesh.SamplePosition(positions[currentCoffePoint],out NHit,5,NavMesh.AllAreas); //Get neat point if random point was invalid
            m_agent.destination = NHit.position;
        }

        currentCoffePoint++;
    }
    protected override void KillNPC()
    {
        //DeathLogic not executed directly here, incase Death Animations get added
        OnNPCDeath();

    }

    protected override void NPCDecision()
    {

    }

    protected override void OnNPCSpawn()
    {
        m_currentState = EAIState.ALIVE | EAIState.IDLE;

        if (m_HP == -1)
            m_HP = 100;
    }


    protected override void OnNPCDeath()
    {
        m_currentState = EAIState.DEAD;
        helper.SpawnLoot(EDropTable.KEVIN, transform.position);
        NetworkServer.Destroy(gameObject);
    }

    [Server]
    private void RunAway()
    {
        // create a path by finding 5 waypoints, taken from current position/direction
        // must get away from local position,
        m_isSearchingCoffe = true;
        for (int i = 0; i < positions.Length; i++)
        {
            if (i == 0)
                positions[i] = new Vector3(Random.Range(0f, 5f), 0, Random.Range(0f, 5f));
            else
                positions[i] = new Vector3(Random.Range(1f, 5f) + positions[i - 1].x, 0, Random.Range(1f, 5f) + positions[i - 1].z);

        }

        for (int j = 1; j < positions.Length; j++)
        {
            positions[j] = transform.TransformPoint(positions[j]);
        }
        m_agent.enabled = true;
        //ToDo: check if point is off Navmesh -> move away from edges
        NavMeshHit NMHit = new NavMeshHit();
        for (int i = 0; i < 3; i++)
        {
            NavMeshHit NHit;
            if (NavMesh.SamplePosition(transform.TransformPoint(positions[0]), out NHit, 2, NavMesh.AllAreas))
            {
                NMHit = NHit;
                i = 3;
            }
        }
        if (NMHit.position == null)
            m_agent.destination = Vector3.zero;
        else
            m_agent.destination = NMHit.position;
    }

    /// <summary>
    /// Called from Player when interacting with the NPC for Combat and Quests
    /// </summary>
    public override void OnInteraction()
    {
        throw new System.NotImplementedException();
    }

    [Server]
    /// <summary>
    /// Called from Player when interacting with the NPC for Combat and Quests
    /// </summary>
    /// <param name="_value">Damage/Value received</param>
    /// <param name="_damageType">Type of Attack (Use NONE if not an attack)</param>
    public override void OnInteraction(float _value, EDamageType _damageType)
    {
        m_isSearchingCoffe = true;

        if (!_damageType.HasFlag(EDamageType.NONE))
        {
            m_HP -= base.DamageCalculation(_value, _damageType, DefenseValues);
            if (m_HP <= 0)
            {
                OnNPCDeath();
            }
        }
    }

    /// <summary>
    /// Called from Player when interacting with the NPC for Combat and Quests
    /// </summary>
    /// <param name="_obj">(Quest-)object from Player</param>
    public override void OnInteraction(object _obj)
    {
        throw new System.NotImplementedException();
    }
}
