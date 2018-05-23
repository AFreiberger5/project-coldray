using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeCollection : MonoBehaviour 
{
    //	#########################################
    //	O			RecipeCollection		    O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse    			O
    //	O	Date: 22.04.2018					O
    //	O	Edited: X							O
    //	O	Description: This is a Collection	O
    //	O	             of Recipes for the     O
    //	O	             Items.                 O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public Recipe m_RecipeCrafting;

	public RecipeCollection(Recipe _RecipeCrafting)
    {
        m_RecipeCrafting = _RecipeCrafting;
    }
}
