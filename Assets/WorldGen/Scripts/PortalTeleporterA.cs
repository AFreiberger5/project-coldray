using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporterA : MonoBehaviour
{
    private Transform m_Player;
    private Transform m_portalB;
   // private Vector3 m_offset = new Vector3(0, 0, 3);
    private bool m_selfRegistered = false;
    private bool m_playerContact = false;


    // Update is called once per frame
    void Update()
    {
        if (m_selfRegistered == false)
        {
            GameStatus.GetInstance().SetPortalA(this.transform);
            m_selfRegistered = true;
        }

        if (m_portalB == null && GameStatus.GetInstance().GetNewPortalB())
        {
            m_portalB = GameStatus.GetInstance().GetPortalB();
            GameStatus.GetInstance().SetNewPortalB(false);
        }
        if (m_portalB != null)
        {

            if (m_Player != null && m_playerContact)
            {
                Vector3 portalToPlayer = m_Player.position - transform.position;
                float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
                if (dotProduct < 0f)
                {

                    float rotationDiff = -Quaternion.Angle(transform.rotation, m_portalB.rotation);
                    //rotationDiff += 180;
                    m_Player.Rotate(Vector3.up, rotationDiff);
                    Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f)*portalToPlayer;
                    m_Player.position = m_portalB.position + positionOffset + new Vector3(0,0,-1.5f);
                    m_playerContact = false;
                }

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_Player = other.transform;
            m_playerContact = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_playerContact = false;
        }
    }
}
