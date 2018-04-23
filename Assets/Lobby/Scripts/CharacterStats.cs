using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterStats
{
    // m/w = 0
    // color = 1
    // face = 2
    // ears = 3
    // eyes = 4
    // accessories = 5
    // hair = 6
    // arraylength = 7

    public string m_StatsName; 
    public int[] m_Model = new int[7]
    {
        0,
        0,
        0,
        0,
        0,
        0,
        0
    };

    public CharacterStats(string _name, int[] _model)
    {
        m_StatsName = _name;
        m_Model = _model;
    }
}