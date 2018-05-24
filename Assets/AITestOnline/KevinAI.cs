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


    [SyncVar]
    private EAIState m_currentState;
    [SyncVar]
    private EAIState m_previousState;
    private Helper helper;
    private NavMeshAgent m_agent;
    private Vector3[] positions;
    private bool SearchingCoffe;
    private bool SecuredCoffe;
    private int currentCoffePoint;

    #region AnimatiorVariables
    private NetworkAnimator m_animator;
    private int IDLiving;
    private int IDMove;
    private int IDFear;
    private int IDDeath;
    private int IDRunning;
    #endregion

    [SyncVar, SerializeField]
    private float m_HP = -1;

    private System.Timers.Timer Time;

    private Dictionary<EDamageType, float> DefenseValues = new Dictionary<EDamageType, float>();

    [ServerCallback]
    private void Awake()
    {
        m_animator = GetComponentInChildren<NetworkAnimator>();
        IDLiving = Animator.StringToHash("IsLiving");
        IDMove = Animator.StringToHash("IsWalking");
        IDFear = Animator.StringToHash("IsFearing"); //Kevin values his Coffe, so he panicks whenever he thinks someone might steal it
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
        m_currentState = EAIState.IDLE | EAIState.ALIVE;
        positions = new Vector3[5];
        currentCoffePoint = 1;
        m_agent = GetComponentInChildren<NavMeshAgent>();
        m_agent.destination = transform.position;
        m_agent.autoBraking = false;
    }

    [ServerCallback]
    void Update()
    {
        //Testing only
        if (Input.GetKeyDown(KeyCode.V))
            RunAway();

        

        NPCDecision();

        if (SearchingCoffe && m_agent.remainingDistance < 0.1f && !m_agent.pathPending)
        {
            SearchCoffe();
        }
        if (SecuredCoffe)
        {


            if (Time == null || Time.Enabled == false)
            {
                m_currentState = m_currentState & ~EAIState.MOVING;
                Time = new System.Timers.Timer
                {
                    //After a while Coffe is brewed and Kevin is not scared anymore
                    Interval = 5000
                };
                Time.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
                {
                    SecuredCoffe = !SecuredCoffe;
                    Time.Stop();
                    m_currentState = EAIState.ALIVE | EAIState.IDLE;
                };
                Time.Start();
            }
        }
        if (m_currentState != m_previousState)
            ChangeAnimations();

    }

    private void ChangeAnimations()
    {
        //Changes animation based on whats needed in the animator, haven't found a good way to make it clean for all AI's
        if (m_currentState.HasFlag(EAIState.ALIVE))
            m_animator.animator.SetBool(IDLiving, true);
        if (m_currentState.HasFlag(EAIState.IDLE))
        {
            m_animator.animator.SetBool(IDMove, false);
            m_animator.animator.SetBool(IDFear, false);
            m_animator.animator.SetBool(IDRunning, false);
        }
        if (m_currentState.HasFlag(EAIState.MOVING))
            m_animator.animator.SetBool(IDRunning, true);
        if (!m_currentState.HasFlag(EAIState.MOVING) && m_previousState.HasFlag(EAIState.MOVING))
            m_animator.animator.SetBool(IDRunning, false);
        if (m_currentState.HasFlag(EAIState.ATTACKING))
            m_animator.animator.SetBool(IDFear, true);
        if (!m_currentState.HasFlag(EAIState.ATTACKING) && m_previousState.HasFlag(EAIState.ATTACKING))
            m_animator.animator.SetBool(IDFear, false);
        if (m_currentState.HasFlag(EAIState.DYING))
            m_animator.animator.SetBool(IDLiving, false);
        if (m_currentState.HasFlag(EAIState.DEAD))
            m_animator.animator.SetBool(IDDeath, true);



        m_previousState = m_currentState;
    }

    private void SearchCoffe()
    {
        //When Kevin gets hit, he panicks and tries to quickly find all his Cofffe(TM) to hide it

        if (currentCoffePoint >= positions.Length)
        {
            SearchingCoffe = false;
            SecuredCoffe = true;
            m_agent.isStopped = true;
            return;
        }
        m_agent.SetDestination(positions[currentCoffePoint]);

        transform.forward = m_agent.destination - transform.position;


        currentCoffePoint++;
    }
    // IEnumerator PathCorrection()
    // {
    //       //Used for path correction if random points are outside of world or in obstacles
    //     if (m_agent.pathPending)
    //         yield return new WaitForEndOfFrame();
    //
    //     if (m_agent.pathStatus == NavMeshPathStatus.PathInvalid)
    //     {
    //         NavMeshHit NHit;
    //         NavMesh.SamplePosition(positions[currentCoffePoint], out NHit, 5, NavMesh.AllAreas); //Get neat point if random point was invalid
    //         m_agent.destination = NHit.position;
    //     }
    //
    // }
    protected override void KillNPC()
    {
        //DeathLogic not executed directly here, incase something like Death Animations get added
        OnNPCDeath();

    }

    protected override void NPCDecision()
    {
        //When other bioms get added, Kevins walk around differently 

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

    private void RunAway()
    {                                       //Attacking = Feared, it's Kevins Battleplan
        m_currentState = EAIState.ALIVE | EAIState.ATTACKING | EAIState.MOVING;
        currentCoffePoint = 1;
        m_agent.isStopped = false;
        // create a path by finding 5 waypoints, taken from current position/direction
        // must get away from local position,
        SearchingCoffe = true;
        //System Random costs more, but unity random gave bad values for gameplay
        System.Random RandomCoffeSpots = new System.Random();
        for (int i = 0; i < positions.Length; i++)
        {
            if (i == 0)
                positions[i] = new Vector3((float)(RandomCoffeSpots.NextDouble() % 6), 0, (float)(RandomCoffeSpots.NextDouble() % 6));
            else
                positions[i] = new Vector3((float)(RandomCoffeSpots.NextDouble() % 6) + positions[i - 1].x,
                                                    0, (float)(RandomCoffeSpots.NextDouble() % 6) + positions[i - 1].z);

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
            //To add some randomness and guarantee first point is ON a navmesh
            NavMeshHit NHit;
            if (NavMesh.SamplePosition(transform.TransformPoint(positions[0]), out NHit, 2, NavMesh.AllAreas))
            {
                NMHit = NHit;
                i = 3;
            }
        }

        m_agent.destination = new Vector3(NMHit.position.x, 0, NMHit.position.z);

        transform.forward = m_agent.destination - transform.position;
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
        SearchingCoffe = true;

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
