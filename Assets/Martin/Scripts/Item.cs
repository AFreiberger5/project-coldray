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

    public int m_ListID;
    public string m_Name;
    public string m_Description;
    public Sprite m_Icon;
    public List<Recipe> m_Recipes;

    public AssetLoader m_AssetLoader;

    public int m_StackSize;

    /// <summary>
    /// Constructor of the Item.
    /// </summary>
    /// <param name="_Name"></param>
    /// <param name="_Description"></param>
    /// <param name="_StackSize"></param>
    /// <param name="_Recipes"></param>
    public Item(string _Name, string _Description, int _StackSize, params Recipe[] _Recipes)
    {
        m_Name = _Name;
        m_Description = _Description;
        m_StackSize = _StackSize;
        m_AssetLoader = GameObject.Find("ItemManager").GetComponent<AssetLoader>();

        // For every Sprite in the AssetLoader. (Look it the Inspector at the ItemManager-GameObject)
        foreach (Sprite S in m_AssetLoader.m_Sprites)
        {
            // If the name of the Sprite equals to the name of the Sprite in the AssetLoader...
            if (S.name == _Name)
            {
                // Sets the Icon to the found Sprite.
                m_Icon = S;
            }
        }

        // A new List  of Recipes.
        m_Recipes = new List<Recipe>();
        // Adds all The Recipes to the Recipes of the Item.
        ListOfRecipes(m_Recipes, _Recipes);
    }

    /// <summary>
    /// Adds all The Recipes to the Recipes of the Item.
    /// </summary>
    /// <param name="_ListToAdd"></param>
    /// <param name="_Recipes"></param>
    private void ListOfRecipes(List<Recipe> _ListToAdd, params Recipe[] _Recipes)
    {
        // For every Recipe in the List of Recipes.
        foreach (Recipe Re in _Recipes)
        {
            // Add the Recipe.
            _ListToAdd.Add(Re);
        }
    }

    // Checks if the Right Mouse-Button is clicked.
    public abstract void OnMouseRightClick();
    // Checks if the Left Mouse-Button is clicked.
    public abstract void OnMouseLeftClick();
    // Checks if the Middle Mouse-Button is clicked.
    public abstract void OnMouseMiddleClick();

    // Returns a string containing all the needed data of an Item.
    public abstract string GetSerializable();
}
