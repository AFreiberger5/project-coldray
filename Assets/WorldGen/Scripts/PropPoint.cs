using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropPoint : Block
{

    public PropPoint(EBlockType _type, Vector3 _pos,Vector3 _worldPos, GameObject _parent, Chunk _owner, Material _atlas)
    {
        m_BlockType = EBlockType.PROP;
        m_parent = _parent;
        m_Position = _pos;
        m_WorldPos = _worldPos;
        m_owner = _owner;
        m_Atlas = _atlas;
        m_HasMesh = false;
        m_IsSolid = false;
        m_RootBlock = _type;
    }

    public override TextureTile TexturePosition(ECubeside _side)
    {
        TextureTile tile = new TextureTile();
        tile.y = 0;
        tile.x = 0;
        return tile;
    }

    public override void Draw()
    {
        if (!HasSolidNeighbour((int)m_Position.x, (int)m_Position.y, (int)m_Position.z + 1))
            CreateQuad(ECubeside.FRONT);
        if (!HasSolidNeighbour((int)m_Position.x, (int)m_Position.y, (int)m_Position.z - 1))
            CreateQuad(ECubeside.BACK);        
        if (!HasSolidNeighbour((int)m_Position.x - 1, (int)m_Position.y, (int)m_Position.z))
            CreateQuad(ECubeside.LEFT);
        if (!HasSolidNeighbour((int)m_Position.x + 1, (int)m_Position.y, (int)m_Position.z))
            CreateQuad(ECubeside.RIGHT);
    }

    public override bool HasSolidNeighbour(int _x, int _y, int _z)
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
            return chunks[_x, _y-1, _z].m_IsSolid;
        }
        catch (System.IndexOutOfRangeException) { }

        return false;
    }

    public override void CreateQuad(ECubeside _side)
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
            case ECubeside.LEFT:
                vertices = new Vector3[] { p7, p4, p0, p3 };
                normals = new Vector3[] {Vector3.right, Vector3.right,
                                            Vector3.right, Vector3.right};
                uvs = GetFaceUVs(_side);
                triangles = new int[] { 1,2,3,0,1,3 };
                break;
            case ECubeside.RIGHT:
                vertices = new Vector3[] { p5, p6, p2, p1 };
                normals = new Vector3[] {Vector3.left, Vector3.left,
                                            Vector3.left, Vector3.left};
                uvs = GetFaceUVs(_side);
                triangles = new int[] { 1, 2, 3, 0, 1, 3 };
                break;
            case ECubeside.FRONT:
                vertices = new Vector3[] { p4, p5, p1, p0 };
                normals = new Vector3[] {Vector3.back, Vector3.back,
                                            Vector3.back, Vector3.back};
                uvs = GetFaceUVs(_side);
                triangles = new int[] { 1, 2, 3, 0, 1, 3 };
                break;
            case ECubeside.BACK:
                vertices = new Vector3[] { p6, p7, p3, p2 };
                normals = new Vector3[] {Vector3.forward, Vector3.forward,
                                            Vector3.forward, Vector3.forward};
                uvs = GetFaceUVs(_side);
                triangles = new int[] { 1, 2, 3, 0, 1, 3 };
                break;        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("Quad");
        quad.transform.position = m_Position;
        quad.transform.parent = this.m_parent.transform;

        MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
    }
}
