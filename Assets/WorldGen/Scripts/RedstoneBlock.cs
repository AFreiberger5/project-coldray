/******************************************
*                                         *
*   Script made by Alexander Bloomenkamp  *
*                                         *
*   Edited by:                            *
*                                         *
******************************************/
using UnityEngine;

public class RedstoneBlock : Block
{
    public RedstoneBlock(Vector3 _pos, GameObject _parent, Chunk _owner, Material _atlas)
    {
        m_BlockType = EBlockType.REDSTONE;
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
        tile.x = 1;
        tile.y = 12;
        return tile;
    }
}
