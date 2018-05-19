using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Room : NetworkBehaviour
{
    public GameObject m_WallPrefab;
    public Transform[] m_TopWallPoints;
    public Transform[] m_RightWallPoints;
    public Transform[] m_BotWallPoints;
    public Transform[] m_LeftWallPoints;
    public Transform m_RoomCamPos;
    

    public void CloseWalls(byte[] _DoorInfo)
    {        
        for (int i = 0; i < _DoorInfo.Length; i++)
        {
            switch (i)
            {
                case 0:
                    if( _DoorInfo[i] == 0)
                    {
                        foreach (Transform t in m_TopWallPoints)
                            Instantiate(m_WallPrefab, t.position, Quaternion.identity);
                    }
                    break;
                case 1:
                    if (_DoorInfo[i] == 0)
                    {
                        foreach (Transform t in m_RightWallPoints)
                            Instantiate(m_WallPrefab, t.position, Quaternion.identity);
                    }
                    break;
                case 2:
                    if (_DoorInfo[i] == 0)
                    {
                        foreach (Transform t in m_BotWallPoints)
                            Instantiate(m_WallPrefab, t.position, Quaternion.identity);
                    }
                    break;
                case 3:
                    if (_DoorInfo[i] == 0)
                    {
                        foreach (Transform t in m_LeftWallPoints)
                            Instantiate(m_WallPrefab, t.position, Quaternion.identity);
                    }
                    break;
            }

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().SetCamRedirect(m_RoomCamPos);
        }
    }
}
