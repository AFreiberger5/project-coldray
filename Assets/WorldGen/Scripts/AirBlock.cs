using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBlock :Block {

    public AirBlock(Vector3 _pos, GameObject _parent, Chunk _owner, Material _atlas)
    {
        m_BlockType = EBlockType.AIR;
        m_parent = _parent;
        m_Position = _pos;
        m_owner = _owner;
        m_Atlas = _atlas;
        m_HasMesh = false;
        m_IsSolid = false;
    }
}
