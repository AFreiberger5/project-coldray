/******************************************
*                                         *
*   Script made by Alexander Bloomenkamp  *
*                                         *
*   Edited by:                            *
*                                         *
******************************************/
using UnityEngine;

public class Block
{
    const float constTileSize = 1 / 16f;
    public enum ECubeside : byte { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };
    public enum EBlockType : byte { GRASS, DIRT, STONE, REDSTONE, DIAMOND, AIR, PROP };
    public struct TextureTile { public int x; public int y; }
    public EBlockType m_BlockType;
    public EBlockType m_RootBlock;
    public Material m_Atlas;
    public Vector3 m_Position;
    public Vector3 m_WorldPos;
    public bool m_IsSolid;
    public bool m_HasMesh;
    protected GameObject m_parent;
    protected Chunk m_owner;

    public Block() { }

    public Block(EBlockType _b, Vector3 _pos, GameObject _p, Chunk _owner, Material _atlas)
    {
        m_BlockType = _b;
        m_parent = _p;
        m_Position = _pos;
        m_owner = _owner;
        m_Atlas = _atlas;
        m_HasMesh = true;
    }

    public virtual TextureTile TexturePosition(ECubeside _side)
    {
        TextureTile tile = new TextureTile();
        tile.x = 0;
        tile.y = 0;
        return tile;
    }

    public virtual Vector2[] GetFaceUVs(ECubeside _side)
    {
        Vector2[] UVs = new Vector2[4];
        TextureTile tilePos = TexturePosition(_side);
        UVs[0] = new Vector2(constTileSize * tilePos.x + constTileSize,
            constTileSize * tilePos.y);
        UVs[1] = new Vector2(constTileSize * tilePos.x + constTileSize,
            constTileSize * tilePos.y + constTileSize);
        UVs[2] = new Vector2(constTileSize * tilePos.x,
            constTileSize * tilePos.y + constTileSize);
        UVs[3] = new Vector2(constTileSize * tilePos.x,
            constTileSize * tilePos.y);
        return UVs;
    }

    /// <summary>
    /// Builds a Quad for a given side of a Cube
    /// </summary>
    /// <param name="side"></param>
    public virtual void CreateQuad(ECubeside _side)
    {
        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh" + _side.ToString();

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];

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
                vertices = new Vector3[] { p3, p0, p1, p2 };
                normals = new Vector3[] {Vector3.down, Vector3.down,
                                            Vector3.down, Vector3.down};
                uvs = GetFaceUVs(_side);
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case ECubeside.TOP:
                vertices = new Vector3[] { p6, p5, p4, p7 };
                normals = new Vector3[] {Vector3.up, Vector3.up,
                                            Vector3.up, Vector3.up};
                uvs = GetFaceUVs(_side);
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case ECubeside.LEFT:
                vertices = new Vector3[] { p3, p7, p4, p0 };
                normals = new Vector3[] {Vector3.left, Vector3.left,
                                            Vector3.left, Vector3.left};
                uvs = GetFaceUVs(_side);
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case ECubeside.RIGHT:
                vertices = new Vector3[] { p1, p5, p6, p2 };
                normals = new Vector3[] {Vector3.right, Vector3.right,
                                            Vector3.right, Vector3.right};
                uvs = GetFaceUVs(_side);
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case ECubeside.FRONT:
                vertices = new Vector3[] { p0, p4, p5, p1 };
                normals = new Vector3[] {Vector3.forward, Vector3.forward,
                                            Vector3.forward, Vector3.forward};
                uvs = GetFaceUVs(_side);
                triangles = new int[] { 3,1, 0, 3, 2, 1 };
                break;
            case ECubeside.BACK:
                vertices = new Vector3[] { p2, p6, p7, p3 };
                normals = new Vector3[] {Vector3.back, Vector3.back,
                                            Vector3.back, Vector3.back};
                uvs = GetFaceUVs(_side);
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        GameObject quad = new GameObject("Quad");
        quad.transform.position = m_Position;
        quad.transform.parent = this.m_parent.transform;

        MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;

    }

    /// <summary>
    /// Converts Coordinates out of Chunksize to local Coordinates in neighbouring Chunk 
    /// </summary>
    /// <param name="_i"></param>
    /// <returns></returns>
	public int ConvertBlockIndexToLocal(int _i)
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
	public virtual bool HasSolidNeighbour(int _x, int _y, int _z)
    {
        Block[,,] chunks;

        if (_x < 0 || _x >= World.CHUNKSIZE ||
           _y < 0 || _y >= World.CHUNKSIZE ||
           _z < 0 || _z >= World.CHUNKSIZE)
        {  //block in a neighbouring chunk

            Vector3 neighbourChunkPos = this.m_parent.transform.position +
                                        new Vector3((_x - (int)m_Position.x) * World.CHUNKSIZE,
                                                    (_y - (int)m_Position.y) * World.CHUNKSIZE,
                                                    (_z - (int)m_Position.z) * World.CHUNKSIZE);
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
	public virtual void Draw()
    {
        if (m_BlockType == EBlockType.AIR || m_HasMesh == false) return;

        if (!HasSolidNeighbour((int)m_Position.x, (int)m_Position.y, (int)m_Position.z + 1))
            CreateQuad(ECubeside.FRONT);
        if (!HasSolidNeighbour((int)m_Position.x, (int)m_Position.y, (int)m_Position.z - 1))
            CreateQuad(ECubeside.BACK);
        if (!HasSolidNeighbour((int)m_Position.x, (int)m_Position.y + 1, (int)m_Position.z))
            CreateQuad(ECubeside.TOP);
        if (!HasSolidNeighbour((int)m_Position.x, (int)m_Position.y - 1, (int)m_Position.z))
            CreateQuad(ECubeside.BOTTOM);
        if (!HasSolidNeighbour((int)m_Position.x - 1, (int)m_Position.y, (int)m_Position.z))
            CreateQuad(ECubeside.LEFT);
        if (!HasSolidNeighbour((int)m_Position.x + 1, (int)m_Position.y, (int)m_Position.z))
            CreateQuad(ECubeside.RIGHT);
    }
}
