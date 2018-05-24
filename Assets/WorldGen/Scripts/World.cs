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
	public class SyncListStructVector3 : SyncListStruct<Vector3>
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
    public static int RADIUS = 2;
    public static Dictionary<string, Chunk> CHUNKS;
    public GameObject m_TreePrefab;
    public GameObject m_PortalBPrefab;
    public GameObject m_PortalDungeonIn;
    public GameObject[] m_PropPrefabs;
    public bool m_newWorld = true;
    private bool m_building = false;
	public SyncListPropInfo m_WorldProps = new SyncListPropInfo ();
    public SyncListStructVector3 m_PotentialSpawnpoints = new SyncListStructVector3();
    private List<PropInfo> m_propFlushList = new List<PropInfo>();
    private Dictionary<Vector3, byte> m_allPropPoints = new Dictionary<Vector3, byte>();
    private List<Vector3> m_freePropPoints = new List<Vector3>();
    private List<Vector3> m_occupiedPropPoints = new List<Vector3>();
    private List<Transform> m_objToRemove = new List<Transform>();
    private NavMeshSurface m_surface;
    // ToDo: add NavMeshModifier Volumo for Kevin pathfinding

    public void StartBuild()
    {
        if (isServer)
        {
            WorldManager.GetInstance().ReportBuildWorldNow(true);
        }
		CHUNKS = new Dictionary<string, Chunk> ();
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


    [Command]
    public void CmdPopulateSyncList()
    {		
        foreach (PropInfo p in m_propFlushList)
        {
            m_WorldProps.Add(p);
        }       

        for (int i = 0; i < m_freePropPoints.Count; i++)
        {
            if (!m_occupiedPropPoints.Contains(m_freePropPoints[i]))
            {
                m_PotentialSpawnpoints.Add(m_freePropPoints[i]);
            }
        }
        WorldManager.GetInstance().ReportPropsListDone(true);
        if (isServer)
        {
            WorldManager.GetInstance().OnPropsListDone(WorldManager.GetInstance().m_PropsListDone);
        }
    }

	[Command]
	void CmdClearSyncList()
	{
		for (int i = 0; i < m_WorldProps.Count; i++)
		{

			m_WorldProps.RemoveAt(i);
		}
        for (int i = 0; i < m_PotentialSpawnpoints.Count; i++)
        {

            m_PotentialSpawnpoints.RemoveAt(i);
        }
    }

    public void PropSeedController()
    {
		Debug.Log ("seedcontroller");
        SpawnPortal(0, 6);

        SpawnPortal(1, 6);

        StartCoroutine(SpawnProp(2, 0, 5));

		Debug.Log ("CmdBuildDungeonA()von world");
     WorldManager.GetInstance().CmdBuildDungeonA();
    }

    public IEnumerator InstantiateProps()
    {
        int counter = 0;
        for (int i = 0; i < m_WorldProps.Count; i++)
        {
            GameObject go = Instantiate(m_PropPrefabs[m_WorldProps[i].PrefabIndex], m_WorldProps[i].WorldPosition, Quaternion.identity);
            m_objToRemove.Add(go.transform);
            counter++;
            if (counter == 50)
            {
                counter = 0;
                yield return null;
            }
        }


        m_surface = GetComponent<NavMeshSurface>();
        m_surface.BuildNavMesh();
        WorldManager.GetInstance().SetPortalAActive(true);
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
        int counter = 0;
        while (b == false)
        {
            if (counter == 100)
            {
                break;
            }
            else
            {
                Vector3 temp = m_freePropPoints[Random.Range(0, m_freePropPoints.Count + 1)];
                b = CheckPropSpace(temp, _objRadius, true);
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
            
            counter++;
        }
    }

    public IEnumerator SpawnProp(byte _prefabIndex, int _objRadius, int _probability)
    {
        bool b = false;

        for (int i = 0; i < m_freePropPoints.Count; i++)
        {
            byte type;
            if (m_allPropPoints.TryGetValue(m_freePropPoints[i], out type))
            {
                if (type == 2)
                {
                    b = CheckPropSpace(m_freePropPoints[i], _objRadius, false);
                    if (b == true)
                    {
                        int rnd = Random.Range(1, 101);
                        if (rnd <= _probability)
                        {
                            m_propFlushList.Add(new PropInfo(m_freePropPoints[i], _prefabIndex));
                            for (int x = 0; x < _objRadius; x++)
                            {
                                for (int z = 0; z < _objRadius; z++)
                                {
                                    if (!m_occupiedPropPoints.Contains(new Vector3(m_freePropPoints[i].x - x, m_freePropPoints[i].y, m_freePropPoints[i].z + z)))
                                        m_occupiedPropPoints.Add(new Vector3(m_freePropPoints[i].x - x, m_freePropPoints[i].y, m_freePropPoints[i].z + z));

                                    if (!m_occupiedPropPoints.Contains(new Vector3(m_freePropPoints[i].x + x, m_freePropPoints[i].y, m_freePropPoints[i].z + z)))
                                        m_occupiedPropPoints.Add(new Vector3(m_freePropPoints[i].x + x, m_freePropPoints[i].y, m_freePropPoints[i].z + z));

                                    if (!m_occupiedPropPoints.Contains(new Vector3(m_freePropPoints[i].x + x, m_freePropPoints[i].y, m_freePropPoints[i].z - z)))
                                        m_occupiedPropPoints.Add(new Vector3(m_freePropPoints[i].x + x, m_freePropPoints[i].y, m_freePropPoints[i].z - z));

                                    if (!m_occupiedPropPoints.Contains(new Vector3(m_freePropPoints[i].x - x, m_freePropPoints[i].y, m_freePropPoints[i].z - z)))
                                        m_occupiedPropPoints.Add(new Vector3(m_freePropPoints[i].x - x, m_freePropPoints[i].y, m_freePropPoints[i].z - z));
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

    bool CheckPropSpace(Vector3 _propPos, int _radius, bool _edgesafe)
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
                    if (_edgesafe == true)
                    {
                        if (m_freePropPoints.Contains(tmp1)
                         && m_freePropPoints.Contains(tmp2)
                            && m_freePropPoints.Contains(tmp3)
                                && m_freePropPoints.Contains(tmp4))
                        {
                            if (!m_occupiedPropPoints.Contains(tmp1)
                                && !m_occupiedPropPoints.Contains(tmp2)
                                    && !m_occupiedPropPoints.Contains(tmp3)
                                        && !m_occupiedPropPoints.Contains(tmp4))
                            {
                                check.Add(true);
                            }
                            else
                                check.Add(false);
                        }
                    }
                    else
                    {
                        if (!m_occupiedPropPoints.Contains(tmp1)
                                && !m_occupiedPropPoints.Contains(tmp2)
                                    && !m_occupiedPropPoints.Contains(tmp3)
                                        && !m_occupiedPropPoints.Contains(tmp4))
                        {
                            check.Add(true);
                        }
                        else
                        {
                            check.Add(false);
                        }
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

    public void DestroyWorld()
    {
        StopCoroutine("BuildWorld");
        StopCoroutine("InstantiateProps");
        StopCoroutine("SpawnProp");
        if (isServer)
        {            
            WorldManager.GetInstance().ReportBuildWorldNow(false);
            WorldManager.GetInstance().ReportWorldBuilt(false);
            WorldManager.GetInstance().ReportPropsDone(false);
            WorldManager.GetInstance().ReportPropsListDone(false);
        }
        m_freePropPoints.Clear();       
        m_occupiedPropPoints.Clear();      
        m_allPropPoints.Clear();       
        m_propFlushList.Clear();
        
        WorldManager.GetInstance().SetPortalAActive(false);
        int counter = 0;

        for (int i = 0; i < m_objToRemove.Count; i++)
        {
            Destroy(m_objToRemove[i].transform.gameObject);
            counter++;
            if (counter == 50)
            {
                //yield return null;
            }
        }
        m_objToRemove.Clear();

        List<string> temp = new List<string>();
        temp.AddRange(CHUNKS.Keys);

        for (int i = 0; i < temp.Count; i++)
        {
            string s = temp[i];
            Chunk c;
            if (CHUNKS.TryGetValue(s, out c))
            {
                Destroy(c.m_Chunk);
                CHUNKS.Remove(s);                
            }
        }
		CmdClearSyncList ();
        WorldManager.GetInstance().m_IsDestroyingWorld = false;
    }
}
