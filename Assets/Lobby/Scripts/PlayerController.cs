using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//||||||||||||||||||||||||||||||||||||||||||||||||||||\\
//||                                                ||\\
//||            Script by Gregor Hempel             ||\\
//||            16.05.2018                          ||\\
//||            Edits: Alexander Blomenkamp         ||\\
//||                                                ||\\
//||||||||||||||||||||||||||||||||||||||||||||||||||||\\

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    [Header("Requirements")]
    public Camera m_PlayerCamera;
    public Transform m_CamAnchor;
    public Transform m_PlayerBodyT;

    private Rigidbody m_playerRigidBody;
    private float m_playerSpeed = 5.0f;
    private float m_playerRotSpeed = 10.0f;
    private Vector3 m_playerMovement;

    /// <summary>
    /// basic setup of the player
    /// </summary>
    private void Start()
    {
        m_playerRigidBody = GetComponent<Rigidbody>();
        m_PlayerCamera = Camera.main;
        SetCamRedirect(m_CamAnchor);
    }

    /// <summary>
    /// player rotation
    /// </summary>
    private void Update()
    {
        if (isLocalPlayer)
        {
            FaceMousePosition();
        }
    }

    /// <summary>
    /// player movement
    /// </summary>
    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            MovePlayer();

            //m_playerMovement.Set(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            //m_playerMovement = m_playerMovement.normalized * m_playerSpeed * Time.deltaTime;

            //m_playerRigidBody.MovePosition(transform.position + m_playerMovement);
        }
    }

    /// <summary>
    /// applies a force onto the players rigidbody
    /// uses the cameras transform instead of the players
    /// </summary>
    private void MovePlayer()
    {
        if (isLocalPlayer)
        {
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetVelocity = m_PlayerCamera.transform.TransformDirection(targetVelocity).normalized* m_playerSpeed;

            Vector3 velocity = m_playerRigidBody.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -m_playerSpeed, m_playerSpeed);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -m_playerSpeed, m_playerSpeed);
            velocityChange.y = 0;
            m_playerRigidBody.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    /// <summary>
    /// rotates the body around the y axis
    /// </summary>
    private void FaceMousePosition()
    {
        if (isLocalPlayer)
        {
            if (m_PlayerCamera == null)
                m_PlayerCamera = Camera.main;

            Ray ray = m_PlayerCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, m_PlayerBodyT.position.y, 0));
            float distance = 0.0f;

            if (plane.Raycast(ray, out distance))
            {
                Vector3 dir = ray.GetPoint(distance) - m_PlayerBodyT.position;

                m_PlayerBodyT.rotation = Quaternion.Slerp(m_PlayerBodyT.rotation, Quaternion.LookRotation(dir), m_playerRotSpeed * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_trf"></param>
    public void SetCamRedirect(Transform _trf)
    {
        if (isLocalPlayer)
        {
            Camera.main.GetComponent<CamController>().SetCamPos(_trf);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_damage"></param>
    /// <param name="_dmgType"></param>
    public void OnPlayerTakeDamage(float _damage, EDamageType _dmgType)
    {

    }
}