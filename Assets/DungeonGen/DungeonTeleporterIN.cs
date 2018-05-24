using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTeleporterIN : MonoBehaviour
{
    private Transform m_Player;
    private bool m_playerContact = false;

    void Start()
    {       
        WorldManager.GetInstance().CallBuildDungeon();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Player != null && m_playerContact)
        {            
            m_Player.position = new Vector3(0, 100, 0); 
            m_playerContact = false;            
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
