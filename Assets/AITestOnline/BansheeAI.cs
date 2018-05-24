/******************************************
*                                         *
*   Script made by Alexander Freiberger   *
*                                         *
*   Edited by:                            *
*                                         *
******************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;
using System;

public class BansheeAI : AIBase
{
    //public variables for debug/testing/design access
    public float m_aggroDistance;
    public float m_deAggroDistance;
    public float m_hitdistance;
    public Transform m_head;

    [SyncVar]
    private EAIState m_currentState;
    [SyncVar]
    private EAIState m_previousState;
    private List<GameObject> m_TargetPlayers;
    [SyncVar]//Testing if this is necessary or can help with interpolation/lag/etc.
    private GameObject m_Target;
    private bool m_PlayerInRange;
    private NavMeshAgent m_agent;
    [SerializeField]
    private int m_Damage = 15;

    #region AnimatiorVariables
    private NetworkAnimator m_animator;
    private int IDLiving;
    private int IDMove;
    private int IDAttack;
    private int IDDeath;
    #endregion
    private Helper helper;
    public GameObject HitBox;

    [SyncVar, SerializeField]
    private float m_HP = -1;

    private Dictionary<EDamageType, float> DefenseValues = new Dictionary<EDamageType, float>();

    [ServerCallback]
    private void Awake()
    {
        
        //Add Defense Values for all Types of DamageSources
        #region AddDefenseValues

        DefenseValues.Add(EDamageType.MELEE, 0);
        DefenseValues.Add(EDamageType.RANGED, 0);
        DefenseValues.Add(EDamageType.MAGICAL, 0);
        DefenseValues.Add(EDamageType.PHYSICAL, 0);
        DefenseValues.Add(EDamageType.TRUE, 0);
        #endregion

        m_TargetPlayers = new List<GameObject>();
        //Initialise the NPC
        OnNPCSpawn();
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<NetworkAnimator>();
        //  m_animator.SetParameterAutoSend(0, true);
        IDLiving = Animator.StringToHash("isliving");
        IDMove = Animator.StringToHash("ismoving");
        IDAttack = Animator.StringToHash("isattacking");
        IDDeath = Animator.StringToHash("isdied");
        helper = new Helper();

    }

    [ServerCallback]
    void Update()
    {
        NPCDecision();

        if (m_currentState != m_previousState)
            ChangeAnimations();
    }


    [Server]
    protected override void KillNPC()
    {
        //Triggers Death Animation, End of animation calls OnNPCDeath()
        m_currentState = EAIState.DYING;

    }


    [Server]
    protected override void NPCDecision()
    {
        //If NPC is idle, check for nearby Players
        if (m_currentState.HasFlag(EAIState.IDLE) && !m_currentState.HasFlag(EAIState.DEAD))
        {
            if (m_PlayerInRange)
            {
                List<RaycastHit> hits = new List<RaycastHit>();
                RaycastHit hit;

                //Players get added via OnTriggerEnter
                foreach (GameObject Player in m_TargetPlayers)
                {
                    //Check if vision to player is blocked by Walls
                    if (Physics.Raycast(m_head.transform.position, (Player.transform.position - m_head.transform.position).normalized
                        , out hit, m_aggroDistance * m_aggroDistance, LayerMask.GetMask(new string[] { "Walls", "Player" })
                        , QueryTriggerInteraction.Ignore))
                    {
                        //Checks if enemey can be seen (Eye-angle)
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
                    //If target was found, make it the target
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
                    m_currentState -= EAIState.IDLE;
                    m_currentState = m_currentState | EAIState.MOVING;

                }
                else
                {
                    return;
                }

            }
        }
        if (m_currentState.HasFlag(EAIState.MOVING) && !m_currentState.HasFlag(EAIState.DEAD))
        {
            //If Banshee is following a target, check if it can be hit or no/if target is lost
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

                if (m_currentState.HasFlag(EAIState.ATTACKING))
                    m_currentState -= EAIState.ATTACKING;

                m_currentState -= EAIState.MOVING;
                m_currentState = m_currentState | EAIState.IDLE;
                return;
            }

        }
    }

    private void NotAttacking()
    {
        if (m_currentState.HasFlag(EAIState.ATTACKING))
            m_currentState -= EAIState.ATTACKING;
    }

    [Server]
    protected override void OnNPCDeath()
    {
        m_currentState = EAIState.DEAD;
        helper.SpawnLoot(EDropTable.BANSHEE, transform.position);
        NetworkServer.Destroy(gameObject);
    }

    [Server]
    protected override void OnNPCSpawn()
    {
        //Banshee is by default Idlling, moves on agression or sight
        m_currentState = EAIState.ALIVE | EAIState.IDLE;

        if (m_HP == -1)
            m_HP = 100;

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

    [Server]
    private void Attack()
    {

        if (!m_currentState.HasFlag(EAIState.ATTACKING))
            m_currentState = m_currentState | EAIState.ATTACKING;

        if (m_hitdistance * m_hitdistance <= (transform.position - m_Target.transform.position).sqrMagnitude)
        {
                //Banshee deals Magical damage and always focuses on the first enemy that faces her wrath
            m_Target.GetComponent<PlayerController>().OnPlayerTakeDamage(m_Damage, EDamageType.MELEE | EDamageType.MAGICAL);
            
        }
    }

    private void ChangeAnimations()
    {
        //Change animations based on current States

        if (m_currentState.HasFlag(EAIState.ALIVE))
            m_animator.animator.SetBool(IDLiving, true);
        if (m_currentState.HasFlag(EAIState.IDLE))
        {
            m_animator.animator.SetBool(IDMove, false);
            m_animator.animator.SetBool(IDAttack, false);
            m_animator.animator.SetBool(IDDeath, false);
        }
        if (m_currentState.HasFlag(EAIState.MOVING))
            m_animator.animator.SetBool(IDMove, true);
        if (m_currentState.HasFlag(EAIState.ATTACKING))
            m_animator.animator.SetBool(IDAttack, true);
        if (!m_currentState.HasFlag(EAIState.ATTACKING) && m_previousState.HasFlag(EAIState.ATTACKING))
            m_animator.animator.SetBool(IDAttack, false);
        if (m_currentState.HasFlag(EAIState.DYING))
        {
            m_animator.animator.SetBool(IDLiving, false);

            //now called via animation event
            //KillNPC();
        }
        if (m_currentState.HasFlag(EAIState.DEAD))
        {
            m_animator.animator.SetBool(IDDeath, true);
        }

        m_previousState = m_currentState;

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
        if (isServer)
        {
            if (!_damageType.HasFlag(EDamageType.NONE))
            {
                m_HP -= base.DamageCalculation(_value, _damageType, DefenseValues);
                if (m_HP <= 0)
                {
                    KillNPC();
                }
            }
        }
        else if(isClient)
        {
            CmdOnInteraction(_value, _damageType);
        }
    }

    [Command]
    public void CmdOnInteraction(float _value, EDamageType _damageType)
    {
        if (!_damageType.HasFlag(EDamageType.NONE))
        {
            m_HP -= base.DamageCalculation(_value, _damageType, DefenseValues);
            if (m_HP <= 0)
            {
                KillNPC();
            }
        }
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
