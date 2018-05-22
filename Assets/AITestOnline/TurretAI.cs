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

public class TurretAI : AIBase
{
    public GameObject m_bullet;
    public List<PlayerController> m_Players;

    private Animator m_animator;
    private int IDAttack;
    private int IDDeath;

    void Awake()
    {
        IDAttack = Animator.StringToHash("IsAtacking");
        IDDeath = Animator.StringToHash("IsDying");
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot()
    {

    }
    protected override void KillNPC()
    {
        throw new System.NotImplementedException();
    }

    protected override void NPCDecision()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnNPCDeath()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnNPCSpawn()
    {
        throw new System.NotImplementedException();
    }

    public override void OnInteraction()
    {
        throw new System.NotImplementedException();
    }

    public override void OnInteraction(float _value, EDamageType _damageType)
    {
        throw new System.NotImplementedException();
    }

    public override void OnInteraction(object _obj)
    {
        throw new System.NotImplementedException();
    }

}
