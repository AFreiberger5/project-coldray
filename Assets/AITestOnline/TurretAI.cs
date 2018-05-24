/**************************************************
*  Credits: Created by Alexander Freiberger		  *
*                                                 *
*  Additional Edits by:                           *
*                                                 *
*                                                 *
*                                                 *
***************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class TurretAI : AIBase
{

    public GameObject m_bullet;
    public List<PlayerController> m_Players;
    public int Seed;

    private NetworkAnimator m_animator;
    private int IDAttack;
    private int IDDeath;
    private int IDIdle;
    private System.Random Rand;
    private int m_bulletPower = 200;
    private bool IsAttacking;
    private float m_fireTime;

    private Dictionary<EDamageType, float> DefenseValues = new Dictionary<EDamageType, float>();
    [SyncVar]
    private float m_HP = 100;


    [ServerCallback]
    void Awake()
    {
        m_animator = GetComponent<NetworkAnimator>();
        IDAttack = Animator.StringToHash("IsAttacking");
        IDDeath = Animator.StringToHash("IsDying");
        IDIdle = Animator.StringToHash("IsIdle");
        Rand = new System.Random(Seed);
        m_fireTime = Rand.Next(1, 5);

        DefenseValues.Add(EDamageType.RANGED, 5);
        
    }
    
    void Start()
    {
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        transform.LookAt(m_Players[0].transform);
        
       if(!IsAttacking)
        {
                       
            m_fireTime -= Time.deltaTime;
           
            if(m_fireTime <= 0)
            {
                m_fireTime = Random.Range(1, 5);
                m_animator.animator.SetBool(IDAttack, true);
                IsAttacking = true;

            }
        }

    }

    [Server]
    public void Shoot()
    {
        GameObject bullet = Instantiate(m_bullet, transform.position + Vector3.up + Vector3.forward, transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * m_bulletPower);
        NetworkServer.Spawn(bullet);

    }
    public void AttackDone()
    {
        IsAttacking = false;
        m_animator.animator.SetBool(IDAttack, false);

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
        m_animator.animator.SetBool(IDDeath, false);
        m_animator.animator.SetBool(IDIdle, true);
        m_animator.animator.SetBool(IDAttack, false);
    }

    public override void OnInteraction()
    {
        throw new System.NotImplementedException();
    }
    [Server]
    public override void OnInteraction(float _value, EDamageType _damageType)
    {
        if (_value != 0)
        {
            m_HP -= base.DamageCalculation(_value, _damageType, DefenseValues);
            if (m_HP <= 0)
                m_animator.animator.SetBool(IDDeath, true);

        }
    }

    public override void OnInteraction(object _obj)
    {
        throw new System.NotImplementedException();
    }

}
