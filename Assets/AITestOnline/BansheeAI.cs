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

public class BansheeAI : AIBase 
{
    public float m_aggroDistance;
    public Transform m_head;
    private AIState CurrentState;
    private void Awake()
    {
        OnNPCSpawn();
    }

    [ServerCallback]
    // Update is called once per frame
    void Update () 
	{
        
	}

    public override void KillNPC()
    {

    }

    public override void NPCDecision()
    {

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
        if(m_aggroDistance > 0)
        Gizmos.DrawWireSphere(m_head.transform.position,m_aggroDistance);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        
    }

}
