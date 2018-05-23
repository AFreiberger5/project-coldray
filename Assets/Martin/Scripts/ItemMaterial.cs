using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaterial : Item
{
    //	#########################################
    //	O			ItemMaterial				O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date: 16.05.2018					O
    //	O	Edited: X							O
    //	O	Description: This is the base Class O
    //	O	             for the Materials.     O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public int m_MaterialID;

    public ItemMaterial(int _MaterialID, string _MaterialName, string _MaterialDescription, int _MaterialStackSize, params Recipe[] _Recipes) : base(_MaterialName, _MaterialDescription, _MaterialStackSize, _Recipes)
    {
        m_ListID = 3;
        m_MaterialID = _MaterialID;
    }

    public override void OnMouseLeftClick()
    {
    }

    public override void OnMouseMiddleClick()
    {
    }

    public override string GetSerializable()
    {
        string S = "";

        S += this.m_ListID;
        S += "~";
        S += this.m_MaterialID;
        S += "~";

        return S;
    }

    public override void OnMouseRightClick()
    {
        // If the Item is in the players hand, it shell be used!
        Use(0f, 0f);
    }

    private void Use(float _Positive, float _Negative)
    {
        // Calculate the given amount of Parameters!
        // X += _Positive;
        // X -= _Negative;
    }
}
