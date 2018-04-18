using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : Item 
{
    //	#########################################
    //	O			ItemWeapon				    O
    //	O---------------------------------------O
    //	O	Author:	Martin Lohse				O
    //	O	Date: 26.03.2018					O
    //	O	Edited: X							O
    //	O	Description: This is the Class for  O
    //  O   Armory-Items                        O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public bool m_IsEquipt;

    public int m_WeaponID;

    public ItemWeapon(int _WeaponID, string _WeaponName, string _WeaponDescription, int _WeaponStackSize) : base(_WeaponName, _WeaponDescription, _WeaponStackSize)
    {
        m_WeaponID = _WeaponID;

        // Weapons are not automaticly equipt!
        m_IsEquipt = false;
    }

    public override void OnMouseLeftClick()
    {
        throw new NotImplementedException();
    }

    public override void OnMouseMiddleClick()
    {
        throw new NotImplementedException();
    }

    public override void OnMouseRightClick()
    {
        throw new NotImplementedException();
    }
}
