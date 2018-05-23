using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Networking;

//WARNING: if u change CHUNKSIZE or COLUMNHEIGHT please remove old mapdatafiles to cleanup manually
// MapData is saved at :
// C:\Users\User\AppData\LocalLow\DefaultCompany\TerraVoxel\MapData

public class World : NetworkBehaviour
{
    public class SyncListPropInfo : SyncListStruct<PropInfo>
    {

    }

    public struct PropInfo
    {
        public Vector3 WorldPosition;
        public byte PrefabIndex;

        public PropInfo(Vector3 _worldPosition, byte _prefabIndex)
        {
            WorldPosition = _worldPosition;
            PrefabIndex = _prefabIndex;
        }
    }

    public Material m_TextureAtlas;
    public static int COLUMNHEIGHT = 1;
    public static int CHUNKSIZE = 32;
    public static int RADIUS = 4;
    public static Dictionary<string, Chunk> CHUNKS;
    public GameObject m_TreePrefab;
    public GameObject m_PortalBPrefab;
    public GameObject m_PortalDungeonIn;
    public GameObject[] m_PropPrefabs;
    public bool m_newWorld = true;
    private bool m_building = false;
    public SyncListPropInfo m_WorldProps = new SyncListPropInfo();
    private List<PropInfo> m_propFlushList = new List<PropInfo>();
    private Dictionary<Vector3, byte> m_allPropPoints = new Dictionary<Vector3, byte>();
    private List<Vector3> m_freePropPoints = new List<Vector3>();
    private List<Vector3> m_occupiedPropPoints = new List<Vector3>();
    private NavMeshSurface m_surface;
    // ToDo: add NavMeshModifier Volumo for Kevin pathfinding

    public void StartBuild()
    {
        if (isServer)
        {
            WorldManager.GetInstance().ReportBuildWorldNow(true);
        }
        CHUNKS = new Dictionary<string, Chunk>();
        this.transform.position = WorldManager.GetInstance().GetWorldPos();
        this.transform.rotation = Quaternion.identity;
        StartCoroutine(BuildWorld());
    }

    /// <summary>
    /// Builds a name for the Chunk based on its position in the cartsian coordinate system
    /// </summary>
    /// <param name="_v"></param>
    /// <returns></returns>
    public static string BuildChunkName(Vector3 _v)
    {
        return (int)_v.x + "_" +
                     (int)_v.y + "_" +
                     (int)_v.z;
    }

    IEnumerator BuildWorld()
    {
        m_building = true;

        // Center of the World to be build
        int posx = (int)Mathf.Floor(WorldManager.GetInstance().GetWorldPos().x / CHUNKSIZE);
        int posz = (int)Mathf.Floor(WorldManager.GetInstance().GetWorldPos().z / CHUNKSIZE);


        // generates chunks in a radius around the world center
        for (int z = -RADIUS; z <= RADIUS; z++)
            for (int x = -RADIUS; x <= RADIUS; x++)
                for (int y = 0; y < COLUMNHEIGHT; y++)
                {
                    Vector3 chunkPosition = new Vector3((x + posx) * CHUNKSIZE,
                                                        y * CHUNKSIZE,
                                                        (posz + z) * CHUNKSIZE);
                    Chunk c;
                    string name = BuildChunkName(chunkPosition);
                    // Chunk already in Dictionary? 
                    if (CHUNKS.TryGetValue(name, out c))
                    {
                        break;
                    }
                    else // no match in Dictionary = new Chunk
                    {
                        c = new Chunk(chunkPosition, m_TextureAtlas);
                        c.m_Chunk.transform.parent = this.transform;
                        CHUNKS.Add(c.m_Chunk.name, c);
                    }

                    yield return null;
                }

        foreach (KeyValuePair<string, Chunk> c in CHUNKS)
        {
            if (c.Value.m_CurrentStatus == Chunk.EStatus.DRAW)
            {
                c.Value.DrawChunk();
            }

            c.Value.m_CurrentStatus = Chunk.EStatus.DONE;
            yield return null;
        }


        if (isServer)
        {
            foreach (KeyValuePair<string, Chunk> c in CHUNKS)
            {
                IsolatePropPoints(c.Value.m_Chunk, c.Value.m_ChunkData);
                //c.Value.Save();
                yield return null;
            }
            WorldManager.GetInstance().ReportWorldBuilt(true);
        }

    }

    IEnumerator LateBuildWorld()
    {
        yield return null;
    }


    [Command]
    public void CmdPopulateSyncList()
    {
        foreach (PropInfo p in m_propFlushList)
        {
            m_WorldProps.Add(p);
        }
        StartCoroutine(ClearLists());
        WorldManager.GetInstance().ReportPropsListDone(true);
        if (isServer)
        {
            WorldManager.GetInstance().OnPropsListDone(WorldManager.GetInstance().m_PropsListDone);
        }
    }

    public void PropSeedController()
    {
        SpawnPortal(0, 6);

        SpawnPortal(1, 6);

        StartCoroutine(SpawnProp(2, 0, 5));


    }

    public IEnumerator InstantiateProps()
    {
        int counter = 0;
        foreach (PropInfo p in m_WorldProps)
        {
            Instantiate(m_PropPrefabs[p.PrefabIndex], p.WorldPosition, Quaternion.identity);
            counter++;
            if (counter == 50)
            {
                counter = 0;
                yield return null;
            }
        }

        m_surface = GetComponent<NavMeshSurface>();
        m_surface.BuildNavMesh();
        WorldManager.GetInstance().ActivatePortalA();
    }

    void IsolatePropPoints(GameObject _c, Block[,,] _b)
    {
        foreach (Block p in _b)
            if (p.m_BlockType == Block.EBlockType.PROP)
            {
                m_allPropPoints.Add(p.m_WorldPos, (byte)p.m_RootBlock);
                m_freePropPoints.Add(p.m_WorldPos);
            }
    }


    void SpawnPortal(byte _prefabIndex, int _objRadius)
    {
        bool b = false;

        while (b == false)
        {
            Vector3 temp = m_freePropPoints[Random.Range(0, m_freePropPoints.Count + 1)];
            b = CheckPropSpace(temp, _objRadius);
            if (b == true)
            {
                m_propFlushList.Add(new PropInfo(new Vector3(temp.x, temp.y + 1, temp.z), _prefabIndex));
                for (int x = 0; x < _objRadius; x++)
                {
                    for (int z = 0; z < _objRadius; z++)
                    {
                        if (!m_occupiedPropPoints.Contains(new Vector3(temp.x - x, temp.y, temp.z + z)))
                            m_occupiedPropPoints.Add(new Vector3(temp.x - x, temp.y, temp.z + z));

                        if (!m_occupiedPropPoints.Contains(new Vector3(temp.x + x, temp.y, temp.z + z)))
                            m_occupiedPropPoints.Add(new Vector3(temp.x + x, temp.y, temp.z + z));

                        if (!m_occupiedPropPoints.Contains(new Vector3(temp.x + x, temp.y, temp.z - z)))
                            m_occupiedPropPoints.Add(new Vector3(temp.x + x, temp.y, temp.z - z));

                        if (!m_occupiedPropPoints.Contains(new Vector3(temp.x - x, temp.y, temp.z - z)))
                            m_occupiedPropPoints.Add(new Vector3(temp.x - x, temp.y, temp.z - z));
                    }
                }
            }
        }
    }

    public IEnumerator SpawnProp(byte _prefabIndex, int _objRadius, int _probability)
    {
        bool b = false;

        foreach (Vector3 v in m_freePropPoints)
        {
            byte type;
            if (m_allPropPoints.TryGetValue(v, out type))
            {
                if (type == 2)
                {
                    b = CheckPropSpace(v, _objRadius);
                    if (b == true)
                    {
                        int rnd = Random.Range(1, 101);
                        if (rnd <= _probability)
                        {
                            m_propFlushList.Add(new PropInfo(v, _prefabIndex));
                            for (int x = 0; x < _objRadius; x++)
                            {
                                for (int z = 0; z < _objRadius; z++)
                                {
                                    if (!m_occupiedPropPoints.Contains(new Vector3(v.x - x, v.y, v.z + z)))
                                        m_occupiedPropPoints.Add(new Vector3(v.x - x, v.y, v.z + z));

                                    if (!m_occupiedPropPoints.Contains(new Vector3(v.x + x, v.y, v.z + z)))
                                        m_occupiedPropPoints.Add(new Vector3(v.x + x, v.y, v.z + z));

                                    if (!m_occupiedPropPoints.Contains(new Vector3(v.x + x, v.y, v.z - z)))
                                        m_occupiedPropPoints.Add(new Vector3(v.x + x, v.y, v.z - z));

                                    if (!m_occupiedPropPoints.Contains(new Vector3(v.x - x, v.y, v.z - z)))
                                        m_occupiedPropPoints.Add(new Vector3(v.x - x, v.y, v.z - z));
                                    yield return null;
                                }
                            }
                        }
                    }
                }
            }
            //yield return null;
        }
        yield return null;
        WorldManager.GetInstance().ReportPropsDone(true);
    }

    IEnumerator ClearLists()
    {
        m_freePropPoints.Clear();
        yield return null;
        m_occupiedPropPoints.Clear();
        yield return null;
        m_allPropPoints.Clear();
        yield return null;
        m_propFlushList.Clear();
        yield return null;
    }

    bool CheckPropSpace(Vector3 _propPos, int _radius)
    {


        if (_radius == 0)
        {
            if (!m_occupiedPropPoints.Contains(_propPos))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            List<bool> check = new List<bool>();
            for (int x = 0; x < _radius; x++)
            {
                for (int z = 0; z < _radius; z++)
                {
                    Vector3 tmp1 = new Vector3(_propPos.x - x, _propPos.y, _propPos.z + z);
                    Vector3 tmp2 = new Vector3(_propPos.x + x, _propPos.y, _propPos.z + z);
                    Vector3 tmp3 = new Vector3(_propPos.x + x, _propPos.y, _propPos.z - z);
                    Vector3 tmp4 = new Vector3(_propPos.x - x, _propPos.y, _propPos.z - z);

                    if (m_allPropPoints.ContainsKey(tmp1)
                         && m_allPropPoints.ContainsKey(tmp2)
                            && m_allPropPoints.ContainsKey(tmp3)
                                && m_allPropPoints.ContainsKey(tmp4))
                    {
                        if (!m_occupiedPropPoints.Contains(tmp1)
                                && !m_occupiedPropPoints.Contains(tmp2)
                                    && !m_occupiedPropPoints.Contains(tmp3)
                                        && !m_occupiedPropPoints.Contains(tmp4))
                        {
                            check.Add(true);
                        }
                        // else
                        // {
                        //     check.Add(false);
                        // }
                    }
                    else
                    {
                        check.Add(false);
                    }
                }
            }
            if (!check.Contains(false))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
