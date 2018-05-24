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
    

    private NetworkAnimator m_animator;
    private int IDAttack;
    private int IDDeath;
    private int IDIdle;
    private int IDDeactivated;
    private System.Random Rand;
    private readonly int m_bulletPower = 200;
    private bool IsAttacking;
    private float m_fireTime;
    [SyncVar]
    private bool IsActivated;

    public bool Activated { get { return IsActivated; } }

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
        IDDeactivated = Animator.StringToHash("IsDeactivated");
        
        m_fireTime = Random.Range(0f,1.5f);

        DefenseValues.Add(EDamageType.RANGED, 5);

    }


    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        if (IsActivated)
        {

            transform.LookAt(m_Players[0].transform);
            RpcRotation(transform.rotation);
            if (!IsAttacking)
            {

                m_fireTime -= Time.deltaTime;

                if (m_fireTime <= 0)
                {
                    m_fireTime = Random.Range(0f, 2f);
                    m_animator.animator.SetBool(IDAttack, true);
                    IsAttacking = true;

                }
            }
        }

    }
    [ClientRpc]
    public void RpcRotation(Quaternion _rotation)
    {
        transform.rotation = _rotation;
    }

    [Server]
    public void ActivateTurret()
    {
        IsActivated = true;
        m_animator.animator.SetBool(IDDeactivated, false);
    }
    [Server]
    public void DeactivateTurret()
    {
        IsActivated = false;
        m_HP = 100;
        m_animator.animator.SetBool(IDDeactivated, true);
    }
    [Server]
    public void Shoot()
    {
        GameObject bullet = Instantiate(m_bullet, transform.position + Vector3.up + Vector3.forward, transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * m_bulletPower);
        NetworkServer.Spawn(bullet);

    }
    
    [Server]
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
        Destroy(this.gameObject);
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
    
    public override void OnInteraction(float _value, EDamageType _damageType)
    {
        if (isServer)
        {
            m_HP -= base.DamageCalculation(_value, _damageType, DefenseValues);
            if (m_HP <= 0)
                m_animator.animator.SetBool(IDDeath, true);

        }
        else if(isClient)
        {
            CmdOnInteraction(_value, _damageType);
        }
    }

    [Command]
    public void CmdOnInteraction(float _value, EDamageType _damageType)
    {
        m_HP -= base.DamageCalculation(_value, _damageType, DefenseValues);
        if (m_HP <= 0)
            m_animator.animator.SetBool(IDDeath, true);
    }

    public override void OnInteraction(object _obj)
    {
        throw new System.NotImplementedException();
    }

}
