using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour 
{
    //	#########################################
    //	O			ItemManager				    O
    //	O---------------------------------------O
    //	O	Author:	Martin Lohse				O
    //	O	Date:	25.03.2018					O
    //	O	Edited:	X							O
    //	O	Description: This Script just sorts O
    //  O   and handles all	the important Stuff O 
    //  O   for Items and brings them into the  O 
    //  O   Game.					            O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    [HideInInspector]
    public List<Item> ItemsFood;
    [HideInInspector]
    public List<Item> ItemsWeapon;
    [HideInInspector]
    public List<Item> ItemsArmor;

    public void Awake()
    {
        // Register all the Lists that contain the Items of the Game.
        ItemRegistry();
        ItemFill();
    }

    private void ItemRegistry()
    {
        // All the Food-Items
        ItemsFood = new List<Item>();
        // All the Armor-Items
        ItemsArmor = new List<Item>();
        // All the Weapon-Items
        ItemsWeapon = new List<Item>();
    }

    private void ItemFill()
    {
        // Here is the place, where all the Lists will be filled!
        ItemsFood.Add(new ItemFood      (01, "Apfel", "Ein saftiger Apfel.", 100, 2, 0));
        ItemsArmor.Add(new ItemArmor    (01, "Brustplatte", "Eine Brustplatte.", 1));
        ItemsWeapon.Add(new ItemWeapon  (01, "Holzschwert","Ein kleines Schert aus Holz.", 1));
    }
}
