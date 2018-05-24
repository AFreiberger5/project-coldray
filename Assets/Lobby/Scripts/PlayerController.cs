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

public class PlayerController : NetworkBehaviour
{
    public Camera m_PlayerCamera;
    public Transform m_CamAnchor;
    public Transform m_PlayerBodyT;

    private Rigidbody m_playerRigidBody;
    private float m_playerSpeed = 5.0f;
    private float m_playerRotSpeed = 10.0f;
    private Vector3 m_playerMovement;

    private void Start()
    {
        m_playerRigidBody = GetComponent<Rigidbody>();
        m_PlayerCamera = Camera.main;
        SetCamRedirect(m_CamAnchor);
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            FaceMousePosition();
        }
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            //MovePlayer();

            //m_playerMovement.Set(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            //m_playerMovement = m_playerMovement.normalized * m_playerSpeed * Time.deltaTime;

            //m_playerRigidBody.MovePosition(transform.position + m_playerMovement);

            m_playerMovement.Set(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            m_playerMovement = m_PlayerCamera.transform.TransformDirection(m_playerMovement).normalized * m_playerSpeed * Time.deltaTime;

            m_playerRigidBody.MovePosition(transform.position + m_playerMovement);

            //Vector3 tv = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            ////m_playerMovement.Set(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            //m_playerMovement = m_PlayerCamera.transform.TransformDirection(tv).normalized * m_playerSpeed/* * Time.deltaTime*/;
            //
            ////m_playerRigidBody.MovePosition(transform.position + m_playerMovement);
            //m_playerRigidBody.AddForce(m_playerMovement-m_playerRigidBody.velocity, ForceMode.VelocityChange);
        }
    }

    private void MovePlayer()
    {
        if (isLocalPlayer)
        {
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetVelocity = m_PlayerCamera.transform.TransformDirection(targetVelocity).normalized;
            targetVelocity *= m_playerSpeed;

            Vector3 velocity = m_playerRigidBody.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -m_playerSpeed/2, m_playerSpeed/2);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -m_playerSpeed/2, m_playerSpeed/2);
            velocityChange.y = 0;
            m_playerRigidBody.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    private void FaceMousePosition()
    {
        if (isLocalPlayer)
        {
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

    public void SetCamRedirect(Transform _trf)
    {
        if (isLocalPlayer)
        {
            Camera.main.GetComponent<CamController>().SetCamPos(_trf);
        }
    }

    public void OnPlayerTakeDamage(float _damage, EDamageType _dmgType)
    {

    }
}