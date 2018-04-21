using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAttack : NetworkBehaviour
{

    public Rigidbody m_projectilePrefab;
    public Transform m_projectileSpawn;

    public int m_shotsPerBurst = 2;
    public float m_reloadTime = 1f;

    private int m_shotsLeft;
    private bool m_isReloading;


    // Use this for initialization
    void Start()
    {
        m_shotsLeft = m_shotsPerBurst;
        m_isReloading = false;
    }


    public void SpellCast()
    {
        if (m_isReloading || m_projectilePrefab == null)
        {
            return;
        }

        CmdSpellCast();

        m_shotsLeft--;
        if (m_shotsLeft <= 0)
        {
            StartCoroutine("Reload");
        }

    }

    [Command]
    private void CmdSpellCast()
    {
        CreateProjectile();
        RpcCreateProjectile();
    }

    [ClientRpc]
    void RpcCreateProjectile()
    {
        if (!isServer)
        {
            CreateProjectile();
        }
    }

    void CreateProjectile()
    {
        Projectile projectile = null;
        projectile = m_projectilePrefab.GetComponent<Projectile>();

        Rigidbody rbody = Instantiate(m_projectilePrefab, m_projectileSpawn.position, m_projectileSpawn.rotation) as Rigidbody;

        if (rbody != null)
        {
            rbody.velocity = projectile.m_speed * m_projectileSpawn.transform.forward;
        }
    }

    IEnumerator Reload()
    {
        m_shotsLeft = m_shotsPerBurst;
        m_isReloading = true;
        yield return new WaitForSeconds(m_reloadTime);
        m_isReloading = false;
    }
}
