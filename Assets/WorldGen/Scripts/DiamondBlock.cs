using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondBlock : Block
{
    public DiamondBlock(Vector3 _pos, GameObject _parent, Chunk _owner, Material _atlas)
    {
        m_BlockType = EBlockType.DIAMOND;
        m_parent = _parent;
        m_position = _pos;
        m_owner = _owner;
        m_Atlas = _atlas;
        m_HasMesh = true;
        m_IsSolid = true;
        // m_BlockUVs = myUVs;

    }

    public override TextureTile TexturePosition(ECubeside _side)
    {
        TextureTile tile = new TextureTile();
        tile.x = 0;
        tile.y = 12;
        return tile;
    }
}
