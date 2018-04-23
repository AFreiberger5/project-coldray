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

public class KevinAI : AIBase
{

    private Animator m_animator;
    private AIState m_currentState;
    private int IDliving;
    private int IDmove;
    private int IDattack;
    private int IDdeath;
    private Helper helper;
    private NavMeshAgent m_agent;
    
    void Start () 
	{
       
	}
	
	void Update () 
	{
		
	}
    public override void KillNPC()
    {

    }

    public override void NPCDecision()
    {

    }

    public override void OnNPCSpawn()
    {
        
    }

    protected override void OnNPCDeath()
    {
        
    }
}
