using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerMotorOld))]
[RequireComponent(typeof(PlayerSetup))]

public class PlayerControllerOld : NetworkBehaviour
{
    public Camera m_PlayerCam;
    PlayerHealth m_health;
    PlayerAttack m_shoot;
    PlayerMotorOld m_motor;
    PlayerSetup m_setup;

    Vector3 m_originalPosition;
    NetworkStartPosition[] m_spawnPoints;

    public GameObject m_spawnFx;

    // Use this for initialization
    void Start()
    {
        m_health = GetComponent<PlayerHealth>();
        m_shoot = GetComponent<PlayerAttack>();
        m_motor = GetComponent<PlayerMotorOld>();
        m_setup = GetComponent<PlayerSetup>();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        m_spawnPoints = GameObject.FindObjectsOfType<NetworkStartPosition>();
        m_originalPosition = transform.position;

    }

    void FixedUpdate()
    {
        if (!isLocalPlayer || m_health.m_isDead)
        {
            return;
        }

        Vector3 inputDirection = GetInput();
        m_motor.MovePlayer(inputDirection);
    }

    void Update()
    {
        if (!isLocalPlayer || m_health.m_isDead)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_shoot.SpellCast();
        }

        #region 2nd Bodypart-Rotation

        // Vector3 inputDirection = GetInput();


        /* if ( inputDirection.sqrMagnitude > 0.25f)
         {
             m_pMotor.RotateChassis(inputDirection);
         }
         */
        #endregion

        Vector3 bodyDir = Utility.ScreenToWorldPoint(Input.mousePosition, m_motor.m_playerBody.position.y, m_PlayerCam) - m_motor.m_playerBody.position;
        m_motor.RotatePlayerBody(bodyDir);
    }

    Vector3 GetInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        return new Vector3(h, 0, v);
    }

    private void Disable()
    {
        Debug.Log("Player Died");
        StartCoroutine("Respawn");
    }

    IEnumerator Respawn()
    {
        transform.position = GetRandomSpawnPosition();
        m_motor.m_rigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(5f);
        m_health.Reset();
        if (m_spawnFx != null)
        {
            GameObject spawnFx = Instantiate(m_spawnFx, transform.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
            Destroy(spawnFx, 3f);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        if (m_spawnPoints != null)
        {
            if (m_spawnPoints.Length > 0)
            {
                NetworkStartPosition startPos = m_spawnPoints[Random.Range(0, m_spawnPoints.Length)];
                return startPos.transform.position;
            }
        }
        return m_originalPosition;
    }
}
