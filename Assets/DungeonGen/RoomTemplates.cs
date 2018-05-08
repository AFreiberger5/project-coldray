using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public bool m_SelfRegistered = false;
    public GameObject m_ClosedRoom;
    public GameObject[] m_BottomRooms;
    public GameObject[] m_LeftRooms;
    public GameObject[] m_TopRooms;
    public GameObject[] m_RightRooms;


    private void Update()
    {
        if (m_SelfRegistered == false)
        {
            GameStatus.GetInstance().SetTemplates(this);            
        }
    }
     
}
