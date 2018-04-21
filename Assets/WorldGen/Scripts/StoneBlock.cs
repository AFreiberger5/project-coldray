using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBlock : Block {

    public Vector2[,] myUVs =
    {
        /*TOP*/			{new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f),
                                new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )},
		/*SIDE*/		{new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f),
                                new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )},
		/*BOTTOM*/		{new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f),
                                new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )}
    };

    public StoneBlock(Vector3 _pos, GameObject _parent, Chunk _owner, Material _atlas)
    {
        m_BlockType = EBlockType.STONE;
        m_parent = _parent;
        m_position = _pos;
        m_owner = _owner;
        m_Atlas = _atlas;
        m_HasMesh = true;
        m_IsSolid = true;
        m_BlockUVs = myUVs;

    }

}
