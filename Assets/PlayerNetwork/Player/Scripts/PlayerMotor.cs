using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : NetworkBehaviour {

    public Rigidbody m_rigidbody;

    // public Transform m_chassis; //2nd Bodypart if needed
    public Transform m_playerBody;

    public float m_moveSpeed = 200f;
    // public float m_chassisRotSpeed = 1f; // Rotationspeed for 2nd Bodypart if needed
    public float m_bodyRotSpeed = 500f;



    // Use this for initialization
    void Start ()
    {
        m_rigidbody = GetComponent<Rigidbody>();

	}
	
	public void MovePlayer(Vector3 _dir)
    {
        Vector3 moveDirection = _dir * m_moveSpeed * Time.deltaTime;
        m_rigidbody.velocity = moveDirection;
    }

    public void FaceDirection(Transform _trf, Vector3 _dir, float _rotSpeed)
    {
        if (_dir != Vector3.zero && _trf != null)
        {
            Quaternion desiredRot = Quaternion.LookRotation(_dir);
            _trf.rotation = Quaternion.Slerp(_trf.rotation, desiredRot, _rotSpeed * Time.deltaTime);
        }
    }

    /*
    public void RotateChassis (Vector3 _dir)
    {
        FaceDirection(m_chassis, _dir, m_chassisRotSpeed);
    }
    */

    public void RotatePlayerBody(Vector3 _dir)
    {
        FaceDirection(m_playerBody, _dir, m_bodyRotSpeed);
    }
}
