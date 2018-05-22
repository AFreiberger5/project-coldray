/******************************************
*                                         *
*   Script made by Alexander Bloomenkamp  *
*                                         *
*   Edited by:  Alexander Freiberger      *
*                                         *
******************************************/
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
    public GameObject m_Turret;

    private GameObject[] m_Turrets;


    public void CloseWalls(byte[] _DoorInfo)
    {
        for (int i = 0; i < _DoorInfo.Length; i++)
        {
            switch (i)
            {
                case 0:
                    if (_DoorInfo[i] == 0)
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

    public void SpawnEnemies(int _maxX, int _maxY)
    {
        m_Turrets = new GameObject[Helper.MAX_PLAYERCOUNT];
        if (!isServer)
            return;

        if (_maxX <= 0 || _maxY <= 0)
        {
            Debug.Log("Room way too small???" + gameObject.name + transform.position);
            return;
        }
        int spawns = Random.Range(0, Helper.MAX_PLAYERCOUNT);
        //Lowering the chance the room with a lot of mobs
        spawns = (spawns < Helper.MAX_PLAYERCOUNT / 2) ? spawns : Random.Range(0, spawns);
        for (int i = 0; i < spawns; i++)
        {
            //Spawn Turrets and add it to a list
            m_Turrets[i] = Instantiate(m_Turret, new Vector3(Random.Range(-_maxX, _maxX), transform.position.y, Random.Range(-_maxY, _maxY)), transform.rotation);
            m_Turrets[i].SetActive(false);
            NetworkServer.Spawn(m_Turrets[i]);
            
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject Turret in m_Turrets)
            {
                Turret.SetActive(true);
                
            }
            other.GetComponent<PlayerController>().SetCamRedirect(m_RoomCamPos);
        }
    }
}
