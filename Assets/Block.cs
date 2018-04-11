using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{

    public enum ECubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };
    public enum EBlockType : byte { GRASS, DIRT, STONE, BEDROCK, REDSTONE, DIAMOND, AIR };

    public EBlockType m_BlockType;
    public bool m_IsSolid;
    private Chunk m_owner;
    private GameObject m_parent;
    private Vector3 m_position;


    Vector2[,] blockUVs =
    {
        
		/*GRASS TOP*/		{new Vector2( 0.125f, 0.375f ), new Vector2( 0.1875f, 0.375f),
                                new Vector2( 0.125f, 0.4375f ),new Vector2( 0.1875f, 0.4375f )},
		/*GRASS SIDE*/		{new Vector2( 0.1875f, 0.9375f ), new Vector2( 0.25f, 0.9375f),
                                new Vector2( 0.1875f, 1.0f ),new Vector2( 0.25f, 1.0f )},
		/*DIRT*/			{new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f),
                                new Vector2( 0.125f, 1.0f ),new Vector2( 0.1875f, 1.0f )},
		/*STONE*/			{new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f),
                                new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )},
		/*BEDROCK*/			{new Vector2( 0.3125f, 0.8125f ), new Vector2( 0.375f, 0.8125f),
                                new Vector2( 0.3125f, 0.875f ),new Vector2( 0.375f, 0.875f )},
		/*REDSTONE*/		{new Vector2( 0.1875f, 0.75f ), new Vector2( 0.25f, 0.75f),
                                new Vector2( 0.1875f, 0.8125f ),new Vector2( 0.25f, 0.8125f )},
		/*DIAMOND*/			{new Vector2( 0.125f, 0.75f ), new Vector2( 0.1875f, 0.75f),
                                new Vector2( 0.125f, 0.8125f ),new Vector2( 0.1875f, 0.8125f )}
    };

    public Block(EBlockType _b, Vector3 _pos, GameObject _p, Chunk _o)
    {
        m_BlockType = _b;
        m_owner = _o;
        m_parent = _p;
        m_position = _pos;
        if (m_BlockType == EBlockType.AIR)
            m_IsSolid = false;
        else
            m_IsSolid = true;
    }

    /// <summary>
    /// Builds a Quad for a given side of a Cube
    /// </summary>
    /// <param name="side"></param>
	void CreateQuad(ECubeside _side)
    {
        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh" + _side.ToString();

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];

        //all possible UVs
        Vector2 uv00;
        Vector2 uv10;
        Vector2 uv01;
        Vector2 uv11;

        if (m_BlockType == EBlockType.GRASS && _side == ECubeside.TOP)
        {
            uv00 = blockUVs[0, 0];
            uv10 = blockUVs[0, 1];
            uv01 = blockUVs[0, 2];
            uv11 = blockUVs[0, 3];
        }
        else if (m_BlockType == EBlockType.GRASS && _side == ECubeside.BOTTOM)
        {
            uv00 = blockUVs[(int)(EBlockType.DIRT + 1), 0];
            uv10 = blockUVs[(int)(EBlockType.DIRT + 1), 1];
            uv01 = blockUVs[(int)(EBlockType.DIRT + 1), 2];
            uv11 = blockUVs[(int)(EBlockType.DIRT + 1), 3];
        }
        else
        {
            uv00 = blockUVs[(int)(m_BlockType + 1), 0];
            uv10 = blockUVs[(int)(m_BlockType + 1), 1];
            uv01 = blockUVs[(int)(m_BlockType + 1), 2];
            uv11 = blockUVs[(int)(m_BlockType + 1), 3];
        }

        //all possible vertices 
        Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

        switch (_side)
        {
            case ECubeside.BOTTOM:
                vertices = new Vector3[] { p0, p1, p2, p3 };
                normals = new Vector3[] {Vector3.down, Vector3.down,
                                            Vector3.down, Vector3.down};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case ECubeside.TOP:
                vertices = new Vector3[] { p7, p6, p5, p4 };
                normals = new Vector3[] {Vector3.up, Vector3.up,
                                            Vector3.up, Vector3.up};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case ECubeside.LEFT:
                vertices = new Vector3[] { p7, p4, p0, p3 };
                normals = new Vector3[] {Vector3.left, Vector3.left,
                                            Vector3.left, Vector3.left};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case ECubeside.RIGHT:
                vertices = new Vector3[] { p5, p6, p2, p1 };
                normals = new Vector3[] {Vector3.right, Vector3.right,
                                            Vector3.right, Vector3.right};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case ECubeside.FRONT:
                vertices = new Vector3[] { p4, p5, p1, p0 };
                normals = new Vector3[] {Vector3.forward, Vector3.forward,
                                            Vector3.forward, Vector3.forward};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case ECubeside.BACK:
                vertices = new Vector3[] { p6, p7, p3, p2 };
                normals = new Vector3[] {Vector3.back, Vector3.back,
                                            Vector3.back, Vector3.back};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("Quad");
        quad.transform.position = m_position;
        quad.transform.parent = this.m_parent.transform;

        MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;

    }

    /// <summary>
    /// Converts Coordinates out of Chunksize to local Coordinates in neighbouring Chunk 
    /// </summary>
    /// <param name="_i"></param>
    /// <returns></returns>
	int ConvertBlockIndexToLocal(int _i)
    {
        if (_i == -1)
            _i = World.CHUNKSIZE - 1;
        else if (_i == World.CHUNKSIZE)
            _i = 0;
        return _i;
    }

    /// <summary>
    /// Check if neighbouring block is in another Chunk and if its solid
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    /// <returns></returns>
	public bool HasSolidNeighbour(int _x, int _y, int _z)
    {
        Block[,,] chunks;

        if (_x < 0 || _x >= World.CHUNKSIZE ||
           _y < 0 || _y >= World.CHUNKSIZE ||
           _z < 0 || _z >= World.CHUNKSIZE)
        {  //block in a neighbouring chunk

            Vector3 neighbourChunkPos = this.m_parent.transform.position +
                                        new Vector3((_x - (int)m_position.x) * World.CHUNKSIZE,
                                                    (_y - (int)m_position.y) * World.CHUNKSIZE,
                                                    (_z - (int)m_position.z) * World.CHUNKSIZE);
            string neighbourName = World.BuildChunkName(neighbourChunkPos);

            _x = ConvertBlockIndexToLocal(_x);
            _y = ConvertBlockIndexToLocal(_y);
            _z = ConvertBlockIndexToLocal(_z);

            Chunk neighbourChunk;
            if (World.CHUNKS.TryGetValue(neighbourName, out neighbourChunk))
            {
                chunks = neighbourChunk.m_ChunkData;
            }
            else // if no neighbour found
                return false;
        }
        else //block in this chunk
            chunks = m_owner.m_ChunkData;

        try
        {
            return chunks[_x, _y, _z].m_IsSolid;
        }
        catch (System.IndexOutOfRangeException) { }

        return false;
    }

    /// <summary>
    /// Creates Quad for each Cubeside without a solid neighbour
    /// </summary>
	public void Draw()
    {
        if (m_BlockType == EBlockType.AIR) return;

        if (!HasSolidNeighbour((int)m_position.x, (int)m_position.y, (int)m_position.z + 1))
            CreateQuad(ECubeside.FRONT);
        if (!HasSolidNeighbour((int)m_position.x, (int)m_position.y, (int)m_position.z - 1))
            CreateQuad(ECubeside.BACK);
        if (!HasSolidNeighbour((int)m_position.x, (int)m_position.y + 1, (int)m_position.z))
            CreateQuad(ECubeside.TOP);
        if (!HasSolidNeighbour((int)m_position.x, (int)m_position.y - 1, (int)m_position.z))
            CreateQuad(ECubeside.BOTTOM);
        if (!HasSolidNeighbour((int)m_position.x - 1, (int)m_position.y, (int)m_position.z))
            CreateQuad(ECubeside.LEFT);
        if (!HasSolidNeighbour((int)m_position.x + 1, (int)m_position.y, (int)m_position.z))
            CreateQuad(ECubeside.RIGHT);
    }
}
