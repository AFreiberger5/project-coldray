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


public class DunGen : NetworkBehaviour
{

    public GameObject m_EntryRoom;
    public int m_RoomSizeX = 32;
    public int m_RoomSizeZ = 18;
    public SyncListTile m_DungeonATiles = new SyncListTile();

    private int m_maxIterations = 5;
    private int m_iterations = 0;
    private Vector3 m_startPos;
    private Dictionary<Vector3, byte[]> m_rooms = new Dictionary<Vector3, byte[]>();
    private Dictionary<Vector3, byte[]> m_toProcess = new Dictionary<Vector3, byte[]>();
    private Dictionary<Vector3, byte[]> m_toAdd = new Dictionary<Vector3, byte[]>();
    private List<Vector3> m_occupiedPos = new List<Vector3>();


    public struct Tile
    {
        public Vector3 pos;
        public byte[] doors;

        public Tile(Vector3 _v, byte[] _b)
        {
            pos = _v;
            doors = _b;
        }
    }

    public class SyncListTile : SyncListStruct<Tile>
    {

    }

    public void PreGen()
    {
        if (isServer)
        {
            m_startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            m_occupiedPos.Add(m_startPos);
            m_toProcess.Add(m_startPos, new byte[] { 1, 1, 1, 1 });
            DunGenController();
        }
        
    }

    void DunGenController()
    {
        if (isServer)
        {
            if (m_toProcess.Count != 0)
            {
                Generate();
            }
            else if (m_toAdd.Count != 0)
            {
                m_iterations++;
                foreach (KeyValuePair<Vector3, byte[]> c in m_toAdd)
                {
                    m_toProcess.Add(c.Key, c.Value);
                }
                m_toAdd.Clear();
                Generate();
            }
            else
            {
                CmdGenerateSyncList();
                WorldManager.GetInstance().m_DungeonADone = true;
                CmdBuild();

            }
        }
    }

    void Generate()
    {
        if (isServer)
        {
            foreach (KeyValuePair<Vector3, byte[]> g in m_toProcess)
            {
                for (int i = 0; i < g.Value.Length; i++)
                {
                    if (g.Value[i] == 1)
                    {
                        Vector3 tmp;
                        tmp.x = g.Key.x;
                        tmp.y = g.Key.y;
                        tmp.z = g.Key.z + m_RoomSizeZ;
                        Vector3 tmp1;
                        tmp1.x = g.Key.x + m_RoomSizeX;
                        tmp1.y = g.Key.y;
                        tmp1.z = g.Key.z;
                        Vector3 tmp2;
                        tmp2.x = g.Key.x;
                        tmp2.y = g.Key.y;
                        tmp2.z = g.Key.z - m_RoomSizeZ;
                        Vector3 tmp3;
                        tmp3.x = g.Key.x - m_RoomSizeX;
                        tmp3.y = g.Key.y;
                        tmp3.z = g.Key.z;
                        byte b0 = (byte)Random.Range(0, 2);
                        byte b1 = (byte)Random.Range(0, 2);
                        byte b2 = (byte)Random.Range(0, 2);
                        byte b3 = (byte)Random.Range(0, 2);

                        switch (i)
                        {
                            case 0:
                                if (m_iterations == m_maxIterations)
                                {
                                    if (m_occupiedPos.Contains(tmp))
                                    {
                                        g.Value[i] = 0;
                                    }
                                    else
                                    {
                                        m_toAdd.Add(tmp, new byte[] { 0, 0, 2, 0 });
                                        m_occupiedPos.Add(tmp);
                                    }
                                }
                                else if (m_occupiedPos.Contains(tmp))
                                {
                                    g.Value[i] = 0;
                                }
                                else
                                {
                                    m_toAdd.Add(tmp, new byte[] { b0, b1, 2, b3 });
                                    m_occupiedPos.Add(tmp);
                                }
                                break;
                            case 1:
                                if (m_iterations == m_maxIterations)
                                {
                                    if (m_occupiedPos.Contains(tmp1))
                                    {
                                        g.Value[i] = 0;
                                    }
                                    else
                                    {
                                        m_toAdd.Add(tmp1, new byte[] { 0, 0, 0, 2 });
                                        m_occupiedPos.Add(tmp1);
                                    }
                                }
                                else if (m_occupiedPos.Contains(tmp1))
                                {
                                    g.Value[i] = 0;
                                }
                                else
                                {
                                    m_toAdd.Add(tmp1, new byte[] { b0, b1, b2, 2 });
                                    m_occupiedPos.Add(tmp1);
                                }
                                break;
                            case 2:
                                if (m_iterations == m_maxIterations)
                                {
                                    if (m_occupiedPos.Contains(tmp2))
                                    {
                                        g.Value[i] = 0;
                                    }
                                    else
                                    {
                                        m_toAdd.Add(tmp2, new byte[] { 2, 0, 0, 0 });
                                        m_occupiedPos.Add(tmp2);
                                    }
                                }
                                else if (m_occupiedPos.Contains(tmp2))
                                {
                                    g.Value[i] = 0;
                                }
                                else
                                {
                                    m_toAdd.Add(tmp2, new byte[] { 2, b1, b2, b3 });
                                    m_occupiedPos.Add(tmp2);
                                }
                                break;
                            case 3:
                                if (m_iterations == m_maxIterations)
                                {
                                    if (m_occupiedPos.Contains(tmp3))
                                    {
                                        g.Value[i] = 0;
                                    }
                                    else
                                    {
                                        m_toAdd.Add(tmp3, new byte[] { 0, 2, 0, 0 });
                                        m_occupiedPos.Add(tmp3);
                                    }
                                }
                                else if (m_occupiedPos.Contains(tmp3))
                                {
                                    g.Value[i] = 0;
                                }
                                else
                                {
                                    m_toAdd.Add(tmp3, new byte[] { b0, 2, b2, b3 });
                                    m_occupiedPos.Add(tmp3);
                                }
                                break;
                        }
                    }
                }
                m_rooms.Add(g.Key, g.Value);
            }
            m_toProcess.Clear();
            DunGenController();
        }
    }

    [Command]
    void CmdGenerateSyncList()
    {
        foreach (KeyValuePair<Vector3, byte[]> c in m_rooms)
        {
            m_DungeonATiles.Add(new Tile(c.Key, c.Value));
        }
    }

    public void Build()
    {
        if (!isServer)
        {
            for (int i = 0; i < m_DungeonATiles.Count; i++)
            {
                GameObject dummy = Instantiate(m_EntryRoom, m_DungeonATiles[i].pos, Quaternion.identity);
                Room dummyroom = dummy.GetComponent<Room>();
                dummyroom.CloseWalls(m_DungeonATiles[i].doors);
            }
        }
        
    }

    [Command]
    public void CmdBuild()
    {

        for (int i = 0; i < m_DungeonATiles.Count; i++)
        {                                               //bossraum == letzer eintrag in synchlist
            GameObject dummy = Instantiate(m_EntryRoom, m_DungeonATiles[i].pos, Quaternion.identity);
            Room dummyroom = dummy.GetComponent<Room>();
            dummyroom.CloseWalls(m_DungeonATiles[i].doors);
            dummyroom.SpawnEnemies((m_RoomSizeX/2)-2,(m_RoomSizeZ/2)-2);
        }
    }   
}
