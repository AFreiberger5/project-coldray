/******************************************
*                                         *
*   Script made by Alexander Bloomenkamp  *
*                                         *
*   Edited by:                            *
*                                         *
******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBlock : Block
{
    public GrassBlock(Vector3 _pos, GameObject _parent, Chunk _owner, Material _atlas)
    {
        m_BlockType = EBlockType.GRASS;
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
        switch (_side)
        {
            case ECubeside.TOP:
                tile.x = 0;
                tile.y = 15;
                return tile;
            case ECubeside.BOTTOM:
                tile.x = 2;
                tile.y = 15;
                return tile;
        }
        tile.x = 3;
        tile.y = 15;
        return tile;
    }
}
