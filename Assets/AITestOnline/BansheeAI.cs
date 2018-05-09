/******************************************
*                                         *
*   Script made by Alexander Freiberger   *
*                                         *
*   Edited by:                            *
*                                         *
******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.AI;
using System;

public class BansheeAI : AIBase
{
    //public variables for debug/testing/design access
    public float m_aggroDistance;
    public float m_deAggroDistance;
    public float m_hitdistance;
    public Transform m_head;


    private EAIState CurrentState;
    private EAIState previousState;
    private List<GameObject> m_TargetPlayers;
    [SyncVar] private GameObject m_Target;
    private bool m_PlayerInRange;
    private NavMeshAgent m_agent;

    [SerializeField] private int m_HealthPoints;

    private NetworkAnimator m_animator;
    private int IDliving;
    private int IDmove;
    private int IDattack;
    private int IDdeath;

    private Helper helper;

    [ServerCallback]
    private void Awake()
    {
        m_TargetPlayers = new List<GameObject>();
        //Initialise the NPC
        OnNPCSpawn();
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<NetworkAnimator>();
        //  m_animator.SetParameterAutoSend(0, true);
        IDliving = Animator.StringToHash("isliving");
        IDmove = Animator.StringToHash("ismoving");
        IDattack = Animator.StringToHash("isattacking");
        IDdeath = Animator.StringToHash("isdied");
        helper = new Helper();

    }

    [ServerCallback]
    void Update()
    {
        //for debug testing only
        if (Input.GetKeyDown(KeyCode.V))
            CurrentState = CurrentState | EAIState.DYING;

        if (!isLocalPlayer)
            NPCDecision();

        if (CurrentState != previousState)
            ChangeAnimations();
        
        
    }

    public void PrintTest(string _name)
    {
        print(_name);
    }

    protected override void KillNPC()
    {
        CurrentState = EAIState.DYING;
        
        //OnNPCDeath();
    }


    protected override void NPCDecision()
    {
        if (CurrentState.HasFlag(EAIState.IDLE) && !CurrentState.HasFlag(EAIState.DEAD))
        {
            if (m_PlayerInRange)
            {
                List<RaycastHit> hits = new List<RaycastHit>();
                RaycastHit hit;

                foreach (GameObject Player in m_TargetPlayers)
                {

                    if (Physics.Raycast(m_head.transform.position, (Player.transform.position - m_head.transform.position).normalized
                        , out hit, m_aggroDistance * m_aggroDistance, LayerMask.GetMask(new string[] { "Walls", "Player" })
                        , QueryTriggerInteraction.Ignore))
                    {
                        Vector3 playerAng = Player.transform.position;
                        playerAng.y = m_head.transform.position.y;
                        float angle = Vector3.SignedAngle((playerAng - m_head.transform.position), m_head.forward, Vector3.up);
                        if (angle >= -90 && angle <= 90)
                        {
                            hits.Add(hit);
                        }
                    }
                }
                if (hits.Count > 0)
                {
                    float distance = m_aggroDistance * m_aggroDistance + 1;
                    foreach (RaycastHit RHit in hits)
                    {
                        float dist = (RHit.transform.position - m_head.transform.position).sqrMagnitude;
                        if (dist < distance)
                        {
                            m_Target = RHit.transform.gameObject;
                            distance = dist;

                        }
                    }
                    CurrentState -= EAIState.IDLE;
                    CurrentState = CurrentState | EAIState.MOVING;

                }
                else
                {
                    return;
                }

            }
        }
        if (CurrentState.HasFlag(EAIState.MOVING) && !CurrentState.HasFlag(EAIState.DEAD))
        {

            m_agent.destination = m_Target.transform.position;


            if (m_agent.remainingDistance < m_hitdistance
                && !m_agent.pathPending)
            {
                
                Attack();

                return;
            }
            else
            {
                NotAttacking();
            }


            if ((m_agent.transform.position - m_Target.transform.position).sqrMagnitude >= m_deAggroDistance)
            {
                m_agent.SetDestination(m_Target.transform.position);

                if (CurrentState.HasFlag(EAIState.ATTACKING))
                    CurrentState -= EAIState.ATTACKING;

                CurrentState -= EAIState.MOVING;
                CurrentState = CurrentState | EAIState.IDLE;
                return;
            }

        }
    }

    private void NotAttacking()
    {
        if (CurrentState.HasFlag(EAIState.ATTACKING))
            CurrentState -= EAIState.ATTACKING;
    }

    [Server]
    protected override void OnNPCDeath()
    {
        CurrentState = EAIState.DEAD;
        helper.SpawnLoot(EDropTable.BANSHEE, transform.position);
        NetworkServer.Destroy(gameObject);
    }

    [Server]
    protected override void OnNPCSpawn()
    {
        //Banshee is by default Idlling, moves on agression or sight
        CurrentState = EAIState.ALIVE | EAIState.IDLE;

        m_HealthPoints = 100;

    }


    [Server]
    private void OnTriggerEnter(Collider other)
    {

        if (!m_TargetPlayers.Contains(other.gameObject) && other.tag == "Player")
        {
            m_TargetPlayers.Add(other.gameObject);
            m_PlayerInRange = true;
        }

    }

    [Server]
    private void OnTriggerExit(Collider other)
    {
        if (m_TargetPlayers.Contains(other.gameObject))
        {
            m_TargetPlayers.Remove(other.gameObject);

            if (m_TargetPlayers.Capacity < 1)
                m_PlayerInRange = false;
        }
    }

    private void Attack()
    {
        
        if (!CurrentState.HasFlag(EAIState.ATTACKING))
            CurrentState = CurrentState | EAIState.ATTACKING;
    }

    private void ChangeAnimations()
    {

        if (CurrentState.HasFlag(EAIState.ALIVE))
            m_animator.animator.SetBool(IDliving, true);
        if (CurrentState.HasFlag(EAIState.IDLE))
        {
            m_animator.animator.SetBool(IDmove, false);
            m_animator.animator.SetBool(IDattack, false);
            m_animator.animator.SetBool(IDdeath, false);
        }
        if (CurrentState.HasFlag(EAIState.MOVING))
            m_animator.animator.SetBool(IDmove, true);
        if (CurrentState.HasFlag(EAIState.ATTACKING))
            m_animator.animator.SetBool(IDattack, true);
        if (!CurrentState.HasFlag(EAIState.ATTACKING) && previousState.HasFlag(EAIState.ATTACKING))
            m_animator.animator.SetBool(IDattack, false);
        if (CurrentState.HasFlag(EAIState.DYING))
        {
            m_animator.animator.SetBool(IDliving, false);

            //now called via animation event
            //KillNPC();
        }
        if (CurrentState.HasFlag(EAIState.DEAD))
        {
            m_animator.animator.SetBool(IDdeath, true);
        }

        previousState = CurrentState;
        
    }

    /// <summary>
    /// Called from Player when interacting with the NPC for Combat and Quests
    /// </summary>
    public override void OnInteraction()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Called from Player when interacting with the NPC for Combat and Quests
    /// </summary>
    /// <param name="_value">Damage/Value received</param>
    /// <param name="_damageType">Type of Attack (Use NONE if not an attack)</param>
    public override void OnInteraction(float _value, EDamageType _damageType)
    {
        
        throw new NotImplementedException();
    }

    /// <summary>
    /// Called from Player when interacting with the NPC for Combat and Quests
    /// </summary>
    /// <param name="_obj">(Quest-)object from Player</param>
    public override void OnInteraction(object _obj)
    {
        throw new NotImplementedException();
    }
}
