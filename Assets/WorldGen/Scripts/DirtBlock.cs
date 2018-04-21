using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtBlock : Block
{

    public Vector2[,] myUVs =
    {
        /*TOP*/			{new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f),
                                new Vector2( 0.125f, 1.0f ),new Vector2( 0.1875f, 1.0f )},
		/*SIDE*/		{new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f),
                                new Vector2( 0.125f, 1.0f ),new Vector2( 0.1875f, 1.0f )},
		/*BOTTOM*/		{new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f),
                                new Vector2( 0.125f, 1.0f ),new Vector2( 0.1875f, 1.0f )}
    };

    public DirtBlock(Vector3 _pos, GameObject _parent, Chunk _owner, Material _atlas)
    {
        m_BlockType = EBlockType.DIRT;
        m_parent = _parent;
        m_position = _pos;
        m_owner = _owner;
        m_Atlas = _atlas;
        m_IsSolid = true;
        m_HasMesh = true;
        m_BlockUVs = myUVs;

    }

}



