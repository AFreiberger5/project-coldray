using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : NetworkBehaviour
{
    Rigidbody m_rigidbody;
    Collider m_collider;
    public int m_speed = 20;
    public float m_damage = -5f;
    public float m_lifetime = 2f;
    List<ParticleSystem> m_allParticles;
    public List<string> m_collisionTags;
    public ParticleSystem m_exploFx;


    // Use this for initialization
    void Start()
    {
        m_allParticles = GetComponentsInChildren<ParticleSystem>().ToList();
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        StartCoroutine("SelfDestruct");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(m_lifetime);
        Explode();
    }

    private void Explode()
    {
        m_collider.enabled = false;
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.Sleep();

        foreach (ParticleSystem p in m_allParticles)
        {
            p.Stop();
        }

        if (m_exploFx != null)
        {
            m_exploFx.transform.parent = null;
            m_exploFx.Play();
        }

        // if (isServer)
        //{
        foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
        {
            m.enabled = false;
        }

        Destroy(gameObject);
        // }
    }

    void CheckCollisions(Collision _col)
    {
        if (m_collisionTags.Contains(_col.collider.tag))
        {
            Debug.Log("Hit");
            Explode();
            PlayerHealth playerHealth = _col.gameObject.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Damage(m_damage);
            }
        }
    }
    private void OnCollisionEnter(Collision _col)
    {
        CheckCollisions(_col);
    }


    public void OnDestroy() // durch das unparenten verbleibt das Partikelsystem sonst in der Szene, Unitys Garbagecollector muss es löschen sobald das Gameobject zerstört wurde
    {
        if (m_exploFx != null)
            DestroyImmediate(m_exploFx.gameObject, true);
    }

}
