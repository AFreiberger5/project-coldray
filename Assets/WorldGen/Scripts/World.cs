using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Realtime.Messaging.Internal;

//WARNING: if u change CHUNKSIZE or COLUMNHEIGHT please remove old mapdatafiles to cleanup manually
// MapData is saved at :
// C:\Users\User\AppData\LocalLow\DefaultCompany\TerraVoxel\MapData

public class World : MonoBehaviour
{

    public GameObject m_Player;
    public Material m_TextureAtlas;
    public static int COLUMNHEIGHT = 1;
    public static int CHUNKSIZE = 32;
    public static int RADIUS = 1;
    public static ConcurrentDictionary<string, Chunk> CHUNKS;
    public Slider m_LoadingAmount;
    public Camera m_Cam;
    public Button m_PlayButton;

    private bool m_newWorld = true;
    private bool m_building = false;
    private bool m_dynamicWorld = false;

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


    // private void BuildChunkAt(int _x, int _y, int _z)
    // {
    //     Vector3 chunkPosition = new Vector3(_x * CHUNKSIZE,
    //                                                    _y * CHUNKSIZE,
    //                                                    _z * CHUNKSIZE);
    //     Chunk c;
    //     string name = BuildChunkName(chunkPosition);
    //     // Chunk already in Dictionary? 
    //     if (CHUNKS.TryGetValue(name, out c))
    //     {
    //         c = new Chunk(chunkPosition, m_TextureAtlas);
    //         c.m_Chunk.transform.parent = this.transform;
    //         CHUNKS.TryAdd(c.m_Chunk.name, c);
    //     }
    // }
    //
    // IEnumerator RecursiveBuildWorld(int _x, int _y, int _z, int _rad)
    // {
    //     yield return null;
    // }
    //
    // IEnumerator DrawChunks()
    // {
    //     foreach (KeyValuePair<string, Chunk> c in CHUNKS)
    //     {
    //         if (c.Value.m_CurrentStatus == Chunk.EStatus.DRAW)
    //         {
    //             c.Value.Save();
    //             c.Value.DrawChunk();
    //         }
    //     }
    //     yield return null;
    // }

    IEnumerator BuildWorld()
    {
        m_building = true;

        // Playerposition based on Chunkposition
        int posx = (int)Mathf.Floor(m_Player.transform.position.x / CHUNKSIZE);
        int posz = (int)Mathf.Floor(m_Player.transform.position.z / CHUNKSIZE);

        // Number of Chunks to be created for Loading-slider as value
        float totalChunks = (Mathf.Pow(RADIUS * 2 + 1, 2) * COLUMNHEIGHT) * 2;
        int processCount = 0;

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
                        c.m_CurrentStatus = Chunk.EStatus.KEEP;
                        break;
                    }
                    else // no match in Dictionary = new Chunk
                    {
                        c = new Chunk(chunkPosition, m_TextureAtlas);
                        c.m_Chunk.transform.parent = this.transform;
                        CHUNKS.TryAdd(c.m_Chunk.name, c);
                    }
                    if (m_newWorld)
                    {
                        processCount++;
                        m_LoadingAmount.value = processCount / totalChunks * 100;
                    }

                    yield return null;
                }

        foreach (KeyValuePair<string, Chunk> c in CHUNKS)
        {
            if (c.Value.m_CurrentStatus == Chunk.EStatus.DRAW)
            {
                c.Value.Save();
                c.Value.DrawChunk();
            }

            c.Value.m_CurrentStatus = Chunk.EStatus.DONE;

            if (m_newWorld)
            {
                processCount++;
                m_LoadingAmount.value = processCount / totalChunks * 100;
            }
            yield return null;
        }
        if (m_newWorld)
        {
            m_Player.SetActive(true);
            m_LoadingAmount.gameObject.SetActive(false);
            m_Cam.gameObject.SetActive(false);
            m_PlayButton.gameObject.SetActive(false);
        }



    }


    public void StartBuild()
    {
        StartCoroutine(BuildWorld());
    }

    // Use this for initialization
    void Start()
    {
        m_Player.SetActive(false);
        CHUNKS = new ConcurrentDictionary<string, Chunk>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_dynamicWorld)
        {
            if (!m_building && !m_newWorld)
            {
                StartCoroutine(BuildWorld());
            }
        }
    }
}
