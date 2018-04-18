using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    //	#########################################
    //	O			Item				        O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date:   25.03.20181					O
    //	O	Edited:	X							O
    //	O	Description: The base of all the    O
    //  O   Items. It's their Parent-Class		O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public string m_NameMartin;
    public string m_Description;
    public Sprite m_Icon;

    public AssetLoader m_AssetLoader;

    // Values
    public int m_StackSize;

    public Item(string _Name, string _Description, int _StackSize)
    {
        m_NameMartin = _Name;
        m_Description = _Description;
        m_StackSize = _StackSize;

        m_AssetLoader = GameObject.Find("ItemManager").GetComponent<AssetLoader>();

        foreach (Sprite S in m_AssetLoader.m_Sprites)
        {
            if (S.name == _Name)
            {
                m_Icon = S;
            }
        }
    }

    public abstract void OnMouseRightClick();
    public abstract void OnMouseLeftClick();
    public abstract void OnMouseMiddleClick();
}
