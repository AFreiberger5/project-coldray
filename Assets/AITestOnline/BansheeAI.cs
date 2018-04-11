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

public class BansheeAI : AIBase
{
    public float m_aggroDistance;
    public Transform m_head;

    private AIState CurrentState;
    private List<GameObject> m_TargetPlayers;
    private GameObject m_Target;
    private bool m_PlayerInRange;
    private void Awake()
    {
        OnNPCSpawn();
    }

    [Server]
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            NPCDecision();
    }

    public override void KillNPC()
    {

    }

    [Server]
    public override void NPCDecision()
    {
        if (CurrentState.HasFlag(AIState.IDLE) && !CurrentState.HasFlag(AIState.DEAD))
        {
            if (m_PlayerInRange)
            {
                RaycastHit hit;

                foreach (GameObject Player in m_TargetPlayers)
                {

                }

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

}
