using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemArmor : Item 
{
    //	#########################################
    //	O			ItemArmor				    O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date:   25.03.2018					O
    //	O	Edited: X							O
    //	O	Description: The Script for the     O
    //  O   Armor-Items.   						O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    // The ID of the Armor-Item.
    public int m_ArmorID;

    /// <summary>
    /// Constructor of the Armor-Items.
    /// </summary>
    /// <param name="_ArmorID"></param>
    /// <param name="_ArmorName"></param>
    /// <param name="_ArmorDescription"></param>
    /// <param name="_ArmorStackSize"></param>
    /// <param name="_ArmorRecipes"></param>
    public ItemArmor(int _ArmorID, string _ArmorName, string _ArmorDescription, int _ArmorStackSize, params Recipe[] _ArmorRecipes) : base(_ArmorName, _ArmorDescription, _ArmorStackSize, _ArmorRecipes)
    {
        m_ListID = 1;
        m_ArmorID = _ArmorID;
    }

    // Checks if the Left Mouse-Button is clicked.
    public override void OnMouseLeftClick()
    {
    }

    // Checks if the Middle Mouse-Button is clicked.
    public override void OnMouseMiddleClick()
    {
    }

    // Checks if the Right Mouse-Button is clicked.
    public override void OnMouseRightClick()
    {
    }

    // Returns a string containing all the needed data of an Item.
    public override string GetSerializable()
    {
        string S = "";

        S += this.m_ListID;
        S += "~";
        S += this.m_ArmorID;
        S += "~";

        return S;
    }
}
