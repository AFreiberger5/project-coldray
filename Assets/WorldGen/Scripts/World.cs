using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Realtime.Messaging.Internal;

//WARNING: if u change CHUNKSIZE or COLUMNHEIGHT please remove old mapdatafiles to cleanup manually
// MapData is saved at :
// C:\Users\User\AppData\LocalLow\DefaultCompany\TerraVoxel\MapData

public class World : MonoBehaviour
{

    public Material m_TextureAtlas;
    public static int COLUMNHEIGHT = 1;
    public static int CHUNKSIZE = 32;
    public static int RADIUS = 4;
    public static ConcurrentDictionary<string, Chunk> CHUNKS;
    public GameObject m_TreePrefab;
    public GameObject m_PortalBPrefab;
    private Component[] m_playerComponents;
    private bool m_newWorld = true;
    private bool m_building = false;

    private NavMeshSurface m_surface;
    // ToDo: add NavMeshModifier Volumo for Kevin pathfinding
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

        // Playerposition based on Chunkposition
        int posx = (int)Mathf.Floor(GameStatus.GetInstance().GetWorldPos().x / CHUNKSIZE);
        int posz = (int)Mathf.Floor(GameStatus.GetInstance().GetWorldPos().z / CHUNKSIZE);
       

        // generates chunks in a radius around the Player
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
                        CHUNKS.TryAdd(c.m_Chunk.name, c);
                    }               

                }

        foreach (KeyValuePair<string, Chunk> c in CHUNKS)
        {
            if (c.Value.m_CurrentStatus == Chunk.EStatus.DRAW)
            {
                c.Value.DrawChunk();
            }

            yield return null;
            c.Value.m_CurrentStatus = Chunk.EStatus.DONE;        
        }
        //this.transform.position = new Vector3(0, 100, 0);

        foreach (KeyValuePair<string, Chunk> c in CHUNKS)
        {
            PropSeed(c.Value.m_Chunk, c.Value.m_ChunkData);
            c.Value.Save();
        }

        

        GameStatus.GetInstance().m_OverworldBuilt = true;
        GameStatus.GetInstance().m_building = false;
        Instantiate(m_PortalBPrefab, new Vector3(GameStatus.GetInstance().GetWorldPos().x,1, GameStatus.GetInstance().GetWorldPos().z), Quaternion.identity);
        yield return null;

        m_surface = GetComponent<NavMeshSurface>();
        m_surface.BuildNavMesh();

    }

    void PropSeed(GameObject _c, Block[,,] _b)
    {
        foreach (Block p in _b)
            if (p.m_BlockType == Block.EBlockType.PROP && p.m_RootBlock == Block.EBlockType.REDSTONE)
                Instantiate(m_TreePrefab, new Vector3(p.m_position.x + _c.transform.position.x,
                                                      p.m_position.y + _c.transform.position.y + 2,
                                                      p.m_position.z + _c.transform.position.z), Quaternion.identity);
    }


    public void StartBuild()
    {
        GameStatus.GetInstance().m_building = true;
        CHUNKS = new ConcurrentDictionary<string, Chunk>();
        this.transform.position =GameStatus.GetInstance().GetWorldPos();
        this.transform.rotation = Quaternion.identity;
        StartCoroutine(BuildWorld());

        
    }

}
