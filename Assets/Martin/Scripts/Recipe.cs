using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour 
{
    //	#########################################
    //	O			Recipe				        O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date: 22.04.2018					O
    //	O	Edited: X							O
    //	O	Description: This Class is the Base	O
    //	O                class for the          O
    //	O                Recepies.	            O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public struct Ingredient
    {
        public string ItemName;
        public int ItemCount;

        public Ingredient(string _RecipeItemName, int _RecipeItemCount)
        {
            ItemName = _RecipeItemName;
            ItemCount = _RecipeItemCount;
        }
    }

    public Ingredient[] m_Ingredients;
    public string m_OutputName;

    public Recipe(string _OutputName, params Ingredient[] _Ingredients)
    {
        m_OutputName = _OutputName;
        m_Ingredients = _Ingredients;
    }
}
