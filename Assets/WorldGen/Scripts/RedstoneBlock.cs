using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedstoneBlock : Block {

    public Vector2[,] myUVs =
    {
        /*TOP*/			{new Vector2( 0.1875f, 0.75f ), new Vector2( 0.25f, 0.75f),
                                new Vector2( 0.1875f, 0.8125f ),new Vector2( 0.25f, 0.8125f )},
		/*SIDE*/		{new Vector2( 0.1875f, 0.75f ), new Vector2( 0.25f, 0.75f),
                                new Vector2( 0.1875f, 0.8125f ),new Vector2( 0.25f, 0.8125f )},
		/*BOTTOM*/		{new Vector2( 0.1875f, 0.75f ), new Vector2( 0.25f, 0.75f),
                                new Vector2( 0.1875f, 0.8125f ),new Vector2( 0.25f, 0.8125f )}
    };

    public RedstoneBlock(Vector3 _pos, GameObject _parent, Chunk _owner, Material _atlas)
    {
        m_BlockType = EBlockType.REDSTONE;
        m_parent = _parent;
        m_position = _pos;
        m_owner = _owner;
        m_Atlas = _atlas;
        m_HasMesh = true;
        m_IsSolid = true;
        m_BlockUVs = myUVs;

    }

}
