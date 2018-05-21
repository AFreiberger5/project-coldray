using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropPoint : Block
{

    public PropPoint(EBlockType _type, Vector3 _pos, GameObject _parent, Chunk _owner, Material _atlas)
    {
        m_BlockType = EBlockType.PROP;
        m_parent = _parent;
        m_position = _pos;
        m_owner = _owner;
        m_Atlas = _atlas;
        m_HasMesh = false;
        m_IsSolid = false;
        m_RootBlock = _type;
    }
}
