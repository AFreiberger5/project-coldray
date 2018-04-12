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

public class BansheeAI : AIBase
{
    public float m_aggroDistance;
    public float m_deAggroDistance;
    public Transform m_head;

    
    private AIState CurrentState;
    private AIState previousState;
    private List<GameObject> m_TargetPlayers;
    [SyncVar] private GameObject m_Target;
    private bool m_PlayerInRange;
    private NavMeshAgent m_agent;
    private NetworkAnimator m_animator;
    [SerializeField] private float m_hitdistance;
    private int IDliving;
    private int IDmove;
    private int IDattack;
    private int IDdeath;

    [ServerCallback]
    private void Awake()
    {
        m_TargetPlayers = new List<GameObject>();
        OnNPCSpawn();
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<NetworkAnimator>();
        //  m_animator.SetParameterAutoSend(0, true);
        IDliving = Animator.StringToHash("isliving");
        IDmove = Animator.StringToHash("ismoving");
        IDattack = Animator.StringToHash("isattacking");
        IDdeath = Animator.StringToHash("isdied");
        m_hitdistance = 10;
    }
    
    void Update()
    {
        CurrentState = AIState.ALIVE | AIState.IDLE;
        print(isLocalPlayer);
        if (!isLocalPlayer)
            NPCDecision();

        if (CurrentState != previousState)
            ChangeAnimations();

    }

    public override void KillNPC()
    {

    }

   
    public override void NPCDecision()
    {
        print(m_TargetPlayers);
        if (CurrentState.HasFlag(AIState.IDLE) && !CurrentState.HasFlag(AIState.DEAD))
        {
            if (m_PlayerInRange)
            {
                List<RaycastHit> hits = new List<RaycastHit>();
                RaycastHit hit;
                
                foreach (GameObject Player in m_TargetPlayers)
                {
                    if (Physics.Raycast(m_head.transform.position, Player.transform.position,
                        out hit, m_aggroDistance, LayerMask.GetMask(new string[] { "Walls", "Player" }),
                        QueryTriggerInteraction.Ignore))
                    {
                        float angle = Vector3.SignedAngle((Player.transform.position - m_head.transform.position), m_head.forward, Vector3.up);
                        if (angle >= -45 && angle <= 45)
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
                    CurrentState -= AIState.IDLE;
                    CurrentState = CurrentState | AIState.ATTACKING;
                }
                else
                {
                    return;
                }

            }
        }
        if (CurrentState.HasFlag(AIState.ATTACKING) && !CurrentState.HasFlag(AIState.DEAD))
        {
            if (m_agent.destination == Vector3.zero && m_Target.transform.position != Vector3.zero)
                m_agent.destination = m_Target.transform.position;
            else if (m_agent.destination == null)
                return;

            if (m_agent.isStopped && (m_agent.transform.position - m_Target.transform.position).sqrMagnitude <= m_hitdistance)
            {
                Attack();
                m_agent.destination = m_Target.transform.position;
                return;
            }
            else if (m_agent.isStopped)
            {
                m_agent.destination = m_Target.transform.position;
            }

            if ((m_agent.transform.position - m_Target.transform.position).sqrMagnitude <= m_deAggroDistance)
            {
                m_agent.destination = m_agent.transform.position;
                CurrentState -= AIState.ATTACKING;
                CurrentState = CurrentState | AIState.IDLE;
                return;
            }

        }
    }

    public override void OnNPCDeath()
    {

    }

    public override void OnNPCSpawn()
    {
        CurrentState = AIState.ALIVE | AIState.IDLE;

    }

    private void OnDrawGizmos()
    {
        if (m_aggroDistance > 0)
            Gizmos.DrawWireSphere(m_head.transform.position, m_aggroDistance);
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

    }

    private void ChangeAnimations()
    {
        previousState = CurrentState;

        if (CurrentState.HasFlag(AIState.ALIVE))
            m_animator.animator.SetBool(IDliving, true);
        if (CurrentState.HasFlag(AIState.IDLE))
        {
            m_animator.animator.SetBool(IDmove, false);
            m_animator.animator.SetBool(IDattack, false);
            m_animator.animator.SetBool(IDdeath, false);
        }
        if (CurrentState.HasFlag(AIState.MOVING))
            m_animator.animator.SetBool(IDmove, true);
        if (CurrentState.HasFlag(AIState.ATTACKING))
            m_animator.animator.SetBool(IDattack, true);
        if (CurrentState.HasFlag(AIState.DYING) || CurrentState.HasFlag(AIState.DEAD))
        {
            m_animator.animator.SetBool(IDliving, false);
            m_animator.animator.SetBool(IDdeath, true);
        }
    }
}
