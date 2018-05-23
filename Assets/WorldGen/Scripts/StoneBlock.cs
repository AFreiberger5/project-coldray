using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBlock : Block
{
    public StoneBlock(Vector3 _pos, GameObject _parent, Chunk _owner, Material _atlas)
    {
        m_BlockType = EBlockType.STONE;
        m_parent = _parent;
        m_Position = _pos;
        m_owner = _owner;
        m_Atlas = _atlas;
        m_HasMesh = true;
        m_IsSolid = true;
    }

    public override TextureTile TexturePosition(ECubeside _side)
    {  
        TextureTile tile = new TextureTile();
         tile.x =1;
         tile.y =15;
        return tile;        
    }
}
