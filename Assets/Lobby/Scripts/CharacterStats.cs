﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class stores all information relevant to the player character
/// </summary>
[Serializable]
public class CharacterStats
{
    public string m_StatsName; 
    public int[] m_Model = new int[9]
    {
        0,// gender
        0,// skin color
        0,// head
        0,// ears
        0,// eyes
        0,// accessories
        0,// hair
        0,// hair color
        0//  eye color
    };

    public CharacterStats(string _name, int[] _model)
    {
        m_StatsName = _name;
        m_Model = _model;
    }
}