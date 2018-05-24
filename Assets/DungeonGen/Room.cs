/******************************************
*                                         *
*   Script made by Alexander Bloomenkamp  *
*                                         *
*   Edited by:  Alexander Freiberger      *
*                                         *
******************************************/
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
    public GameObject m_Turret;

    private List<Transform> m_blocksToRemove = new List<Transform>();
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
                        {
                            GameObject go = Instantiate(m_WallPrefab, t.position, Quaternion.identity);
                            m_blocksToRemove.Add(go.transform);
                        }
                    }
                    break;
                case 1:
                    if (_DoorInfo[i] == 0)
                    {
                        foreach (Transform t in m_RightWallPoints)
                        {
                            GameObject go = Instantiate(m_WallPrefab, t.position, Quaternion.identity);
                            m_blocksToRemove.Add(go.transform);
                        }
                    }
                    break;
                case 2:
                    if (_DoorInfo[i] == 0)
                    {
                        foreach (Transform t in m_BotWallPoints)
                        {
                            GameObject go = Instantiate(m_WallPrefab, t.position, Quaternion.identity);
                            m_blocksToRemove.Add(go.transform);
                        }
                    }
                    break;
                case 3:
                    if (_DoorInfo[i] == 0)
                    {
                        foreach (Transform t in m_LeftWallPoints)
                        {
                            GameObject go = Instantiate(m_WallPrefab, t.position, Quaternion.identity);
                            m_blocksToRemove.Add(go.transform);
                        }
                    }
                    break;
            }

        }
    }

    public void SpawnEnemies(int _maxX, int _maxY)
    {
        m_Turrets = new GameObject[Helper.MAX_PLAYERCOUNT + 2];
        

        if (_maxX <= 0 || _maxY <= 0)
        {
            Debug.Log("Room way too small???" + gameObject.name + transform.position);
            return;
        }
        int spawns = Random.Range(0, Helper.MAX_PLAYERCOUNT + 2);
        //Lowering the chance the room with a lot of mobs
        //ToDo: Balance how many spawns
        spawns = (spawns < (Helper.MAX_PLAYERCOUNT + 2) / 2) ? spawns : Random.Range(0, Helper.MAX_PLAYERCOUNT + 2);
        for (int i = 0; i < spawns; i++)
        {
            //Spawn Turrets and add it to a list
            m_Turrets[i] = Instantiate(m_Turret, new Vector3(Random.Range(-_maxX, _maxX) + transform.position.x, transform.position.y, Random.Range(-_maxY, _maxY) + transform.position.z), transform.rotation);
            NetworkServer.Spawn(m_Turrets[i]);
            

        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (m_Turrets != null)
                for (int i = 0; i < m_Turrets.Length; i++)
                {
                    if (m_Turrets[i] == null)
                        continue;
                    if (!m_Turrets[i].GetComponent<TurretAI>().Activated)
                    {
                        if (hasAuthority)
                        {

                            CmdActivateTurret(i);
                        }
                        else
                        {
                            m_Turrets[i].GetComponent<TurretAI>().ActivateTurret();
                        }

                    }

                    m_Turrets[i].GetComponent<TurretAI>().m_Players.Add(other.GetComponent<PlayerController>());

                }

            other.GetComponent<PlayerController>().m_PlayerInDungeon = true;

            other.GetComponent<PlayerController>().SetCamRedirect(m_RoomCamPos);

            Camera.main.orthographicSize = 9;

            // WorldManager.GetInstance().CmdDestroyWorld();
        }
    }
    [Command]
    private void CmdActivateTurret(int _index)
    {
        m_Turrets[_index].GetComponent<TurretAI>().ActivateTurret();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (m_Turrets != null)
                for (int f = 0; f < m_Turrets.Length; f++)
                {
                    if (m_Turrets[f] == null)
                        continue;
                    for (int i = 0; i < m_Turrets[f].GetComponent<TurretAI>().m_Players.Count; i++)
                    {
                        if (m_Turrets[f].GetComponent<TurretAI>().m_Players[i].GetComponent<PlayerCharacter>().m_PlayerId == other.GetComponent<PlayerCharacter>().m_PlayerId)
                            m_Turrets[f].GetComponent<TurretAI>().m_Players.Remove(m_Turrets[f].GetComponent<TurretAI>().m_Players[i]);
                        if (m_Turrets[f].GetComponent<TurretAI>().m_Players.Count <= 0)
                            m_Turrets[f].GetComponent<TurretAI>().DeactivateTurret();
                    }

                }

            other.GetComponent<PlayerController>().SetCamRedirect(other.GetComponent<PlayerController>().m_CamAnchor);

            other.GetComponent<PlayerController>().m_PlayerInDungeon = false;

            Camera.main.orthographicSize = 3;
        }
    }

    private void OnDestroy()
    {
   
        for (int i = 0; i < m_blocksToRemove.Count; i++)
        {
            Destroy(m_blocksToRemove[i].transform.gameObject);
        }
    }
}
