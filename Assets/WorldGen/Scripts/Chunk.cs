using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
class BlockData
{
    public Block.EBlockType[,,] Matrix;

    public BlockData()
    {
    }

    public BlockData(Block[,,] _b)
    {
        Matrix = new Block.EBlockType[World.CHUNKSIZE, World.CHUNKSIZE, World.CHUNKSIZE];
        for (int z = 0; z < World.CHUNKSIZE; z++)
            for (int y = 0; y < World.CHUNKSIZE; y++)
                for (int x = 0; x < World.CHUNKSIZE; x++)
                {
                    Matrix[x, y, z] = _b[x, y, z].m_BlockType;
                }
    }
}

public class Chunk
{

    public Material m_CubeMaterial;
    public Block[,,] m_ChunkData;
    public GameObject m_Chunk;
    public enum EStatus { DRAW, DONE, KEEP };
    public EStatus m_CurrentStatus;
    private BlockData m_blockData;

    private string BuildChunkFileName(Vector3 _v)
    {
        return Application.persistentDataPath + "/MapData/Chunk_"
                                                + (int)_v.x + "_"
                                                   + _v.y + "_"
                                                       + _v.z + "_"
                                                           + World.CHUNKSIZE + "_"
                                                               + World.RADIUS + ".dat";
    }

    private bool Load()
    {
        string chunkFile = BuildChunkFileName(m_Chunk.transform.position);
        if (File.Exists(chunkFile))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(chunkFile, FileMode.Open);
            m_blockData = (BlockData)bf.Deserialize(file);
            file.Close();
            // Debug.Log(" Loading chunk from File:" + chunkFile);
            return true;
        }
        return false;
    }

    public void Save()
    {
        string chunkFile = BuildChunkFileName(m_Chunk.transform.position);
        if (!File.Exists(chunkFile))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(chunkFile));
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(chunkFile, FileMode.OpenOrCreate);
        m_blockData = new BlockData(m_ChunkData);
        bf.Serialize(file, m_blockData);
        file.Close();
        // Debug.Log("Saving chunk to File:" + chunkFile);
    }

    public Chunk(Vector3 _position, Material _c)
    {

        m_Chunk = new GameObject(World.BuildChunkName(_position));
        m_Chunk.transform.position = _position;
        m_CubeMaterial = _c;
        BuildChunk();
    }

    /// <summary>
    /// Creates a Chunk.
    /// Populates the Chunk based on Fractal Brownian Motion
    /// </summary>
	private void BuildChunk()
    {
        bool fileData = false;
        fileData = Load();
        
        m_ChunkData = new Block[World.CHUNKSIZE, World.CHUNKSIZE, World.CHUNKSIZE];

        for (int z = 0; z < World.CHUNKSIZE; z++)
            for (int y = 0; y < World.CHUNKSIZE; y++)
                for (int x = 0; x < World.CHUNKSIZE; x++)
                {
                    // Block position
                    Vector3 pos = new Vector3(x, y, z);
                    // Block position in the World to compare to NoiseMap
                    int worldX = (int)(x + m_Chunk.transform.position.x);
                    int worldY = (int)(y + m_Chunk.transform.position.y);
                    int worldZ = (int)(z + m_Chunk.transform.position.z);

                    if (fileData)
                    {
                        m_ChunkData[x, y, z] = new Block(m_blockData.Matrix[x, y, z], pos, m_Chunk.gameObject, this);

                        continue;
                    }

                    // World Layers from bottom to top 
                    if (Utils.FBM3D(worldX, worldY, worldZ, 0.1f, 4) < 0.40f)
                        m_ChunkData[x, y, z] = new Block(Block.EBlockType.AIR, pos,
                                        m_Chunk.gameObject, this);
                    if (worldY <= Utils.GenerateCliffHeight(worldX, worldZ))
                        m_ChunkData[x, y, z] = new Block(Block.EBlockType.AIR, pos,
                                        m_Chunk.gameObject, this);
                    else if (worldY > 15)
                        m_ChunkData[x, y, z] = new Block(Block.EBlockType.AIR, pos,
                                        m_Chunk.gameObject, this);

                    else if (worldY == 0)
                        m_ChunkData[x, y, z] = new Block(Block.EBlockType.BEDROCK, pos,
                                        m_Chunk.gameObject, this);
                    else if (worldY <= Utils.GenerateStoneHeight(worldX, worldZ))
                    {
                        if (Utils.FBM3D(worldX, worldY, worldZ, 0.01f, 2) < 0.5f && worldY < 16)
                            m_ChunkData[x, y, z] = new Block(Block.EBlockType.DIAMOND, pos,
                                        m_Chunk.gameObject, this);
                        else if (Utils.FBM3D(worldX, worldY, worldZ, 0.03f, 3) < 0.41f && worldY < 16)
                            m_ChunkData[x, y, z] = new Block(Block.EBlockType.REDSTONE, pos,
                                        m_Chunk.gameObject, this);
                        else
                            m_ChunkData[x, y, z] = new Block(Block.EBlockType.STONE, pos,
                                        m_Chunk.gameObject, this);
                    }
                    else if (worldY == Utils.GenerateHeight(worldX, worldZ)) // Grass equals the heightvalue returned by the function 
                        m_ChunkData[x, y, z] = new Block(Block.EBlockType.GRASS, pos,
                                        m_Chunk.gameObject, this);
                    else if (worldY < Utils.GenerateHeight(worldX, worldZ))
                        m_ChunkData[x, y, z] = new Block(Block.EBlockType.DIRT, pos,
                                        m_Chunk.gameObject, this);
                    else
                        m_ChunkData[x, y, z] = new Block(Block.EBlockType.AIR, pos,
                                        m_Chunk.gameObject, this);

                    m_CurrentStatus = EStatus.DRAW;
                }
    }

    /// <summary>
    /// Draws the Chunk based on its Blockdata.
    /// Adds a CollisionMesh equal to the ChunkMesh
    /// </summary>
	public void DrawChunk()
    {
        for (int z = 0; z < World.CHUNKSIZE; z++)
            for (int y = 0; y < World.CHUNKSIZE; y++)
                for (int x = 0; x < World.CHUNKSIZE; x++)
                {
                    m_ChunkData[x, y, z].Draw();
                }
        CombineQuads();
        MeshCollider collider = m_Chunk.gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        collider.sharedMesh = m_Chunk.transform.GetComponent<MeshFilter>().mesh;
    }

    /// <summary>
    /// Combines all Meshes in the Chunk, adds MeshFilter and MeshRenderer
    /// </summary>
    void CombineQuads()
    {
        //1. Combine all children meshes
        MeshFilter[] meshFilters = m_Chunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        //2. Create a new mesh on the parent object
        MeshFilter mf = (MeshFilter)m_Chunk.gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();

        //3. Add combined meshes on children as the parents mesh
        mf.mesh.CombineMeshes(combine);

        //4. Create a renderer for the parent
        MeshRenderer renderer = m_Chunk.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = m_CubeMaterial;

        //5. Delete all uncombined children
        foreach (Transform quad in m_Chunk.transform)
        {
            GameObject.Destroy(quad.gameObject);
        }

    }

}