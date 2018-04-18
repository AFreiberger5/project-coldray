using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFood : Item 
{
    //	#########################################
    //	O			ItemFood				    O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date: 25.03.2018					O
    //	O	Edited:	X							O
    //	O	Description: The Script for the     O 
    //  O   Food-Items. 						O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public int m_FoodID;
    public float m_PositiveValue;
    public float m_NegativeValue;

    public ItemFood(int _FoodID, string _FoodName, string _FoodDescription, int _FoodStackSize, float _PositiveValue, float _NegativeValue) : base(_FoodName, _FoodDescription, _FoodStackSize)
    {
        m_FoodID = _FoodID;
        m_PositiveValue = _PositiveValue;
        m_NegativeValue = _NegativeValue;
    }

    public override void OnMouseLeftClick()
    {
    }

    public override void OnMouseMiddleClick()
    {
    }

    public override void OnMouseRightClick()
    {
        // If the Item is in the players hand, it shell be used!
        Use(m_PositiveValue, m_NegativeValue);
    }

    private void Use(float _Positive, float _Negative)
    {
        // Calculate the given amount of Parameters!
        // X += _Positive;
        // X -= _Negative;
    }
}


