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

    public string m_Name; 
    public byte[] m_Model = new byte[7];

    //public bool Gender
    //{
    //    get
    //    {
    //        return m_Model[0] == 0;
    //    }
    //    set
    //    {
    //        m_Model[0] = value ? (byte)0 : (byte)1;
    //    }
    //}
    //public byte SkinColor
    //{
    //    get
    //    {
    //        return m_Model[1];
    //    }
    //}
    //public byte Face
    //{
    //    get
    //    {
    //        return m_Model[2];
    //    }
    //}
    //public byte Ears
    //{
    //    get
    //    {
    //        return m_Model[3];
    //    }
    //}
    //public byte Eyes
    //{
    //    get
    //    {
    //        return m_Model[4];
    //    }
    //}
    //public byte Accessories
    //{
    //    get
    //    {
    //        return m_Model[5];
    //    }
    //}
    //public byte Hair
    //{
    //    get
    //    {
    //        return m_Model[6];
    //    }
    //}

    public CharacterStats(string _name, byte[] _model)
    {
        m_Name = _name;
        m_Model = _model;
    }
}