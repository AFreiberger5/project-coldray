﻿/******************************************
*                                         *
*   Script made by Alexander Blomenkamp   *
*                                         *
*   Edited by:                            *
*                                         *
******************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : NetworkBehaviour
{
    public Camera m_PlayerCam;
    PlayerMotor m_motor;

    // Use this for initialization
    void Start()
    {
        m_motor = GetComponent<PlayerMotor>();
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Vector3 inputDirection = GetInput();
        m_motor.MovePlayer(inputDirection);
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }   

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
}
