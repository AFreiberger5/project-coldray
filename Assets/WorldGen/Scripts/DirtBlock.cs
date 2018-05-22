using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtBlock : Block
{
    public DirtBlock(Vector3 _pos, GameObject _parent, Chunk _owner, Material _atlas)
    {
        m_BlockType = EBlockType.DIRT;
        m_parent = _parent;
        m_Position = _pos;
        m_owner = _owner;
        m_Atlas = _atlas;
        m_IsSolid = true;
        m_HasMesh = true;
    }

    public override TextureTile TexturePosition(ECubeside _side)
    {
        TextureTile tile = new TextureTile();
        tile.x = 5;
        tile.y = 13;
        return tile;
    }
}



