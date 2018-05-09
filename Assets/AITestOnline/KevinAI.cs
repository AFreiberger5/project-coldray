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
    private EAIState m_currentState;
    private int IDliving;
    private int IDmove;
    private int IDattack;
    private int IDdeath;
    private Helper helper;
    private NavMeshAgent m_agent;
    private Vector3[] positions;
    private bool SearchingCoffe;
    private bool FoundCoffe;
    private int currentCoffePoint;

    void Start()
    {
        positions = new Vector3[5];
        currentCoffePoint = 1;
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.destination = transform.position;
        m_agent.autoBraking = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            RunAway();

        if (SearchingCoffe && m_agent.remainingDistance < 0.1f && !m_agent.pathPending )
        {
            SearchCoffe();
        }
        if (FoundCoffe)
        {
            //Head in ground
            m_agent.enabled = false;
        }

    }

    private void SearchCoffe()
    {

        if (currentCoffePoint >= positions.Length)
        {
            SearchingCoffe = false;
            FoundCoffe = true;
            return;
        }

        m_agent.destination = positions[currentCoffePoint];


        currentCoffePoint++;
    }
    protected override void KillNPC()
    {

    }

    protected override void NPCDecision()
    {

    }

    protected override void OnNPCSpawn()
    {

    }

    protected override void OnNPCDeath()
    {

    }

    private void RunAway()
    {
        // create a path by finding 5 waypoints, taken from current position/direction
        // must get away from local position,
        SearchingCoffe = true;
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
        m_agent.destination = transform.TransformPoint(positions[0]);
    }

    /// <summary>
    /// Called from Player when interacting with the NPC for Combat and Quests
    /// </summary>
    public override void OnInteraction()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Called from Player when interacting with the NPC for Combat and Quests
    /// </summary>
    /// <param name="_value">Damage/Value received</param>
    /// <param name="_damageType">Type of Attack (Use NONE if not an attack)</param>
    public override void OnInteraction(float _value, EDamageType _damageType)
    {
        throw new System.NotImplementedException();
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
