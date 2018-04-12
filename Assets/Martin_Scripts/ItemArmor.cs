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

    public int m_ArmorID;

    private bool m_IsEquipt;

    public ItemArmor(int _ArmorID, string _ArmorName, string _ArmorDescription, int _ArmorStackSize) : base(_ArmorName, _ArmorDescription, _ArmorStackSize)
    {
        m_ArmorID = _ArmorID;

        //Once the Armor is created or dropped, it's not automaticly equiped.
        m_IsEquipt = false;
    }

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    public override void OnMouseLeftClick()
    {
    }

    public override void OnMouseMiddleClick()
    {
    }

    public override void OnMouseRightClick()
    {
        // Is the Armor is in the Players Hand it should be armored!
    }

    /// <summary>
    /// Attachs or equips the Item to the virtual Player.
    /// </summary>
    public void Equip()
    {
        m_IsEquipt = true;
    }

    /// <summary>
    /// Detaches or unequips the Item from the virtual Player.
    /// </summary>
    public void UnEquip()
    {
        m_IsEquipt = false;
    }
}
