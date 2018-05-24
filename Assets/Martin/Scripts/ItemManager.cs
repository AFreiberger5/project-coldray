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
    [HideInInspector]
    public List<Item> ItemsMaterial;
    [HideInInspector]
    public List<List<Item>> ItemLists;

    public enum EItemType
    {
        Food,
        Armor,
        Weapon,
        Material
    }


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
        // All the Material-Items
        ItemsMaterial = new List<Item>();

        // All the Lists with Items.
        ItemLists = new List<List<Item>>();
        ItemLists.Add(ItemsFood);
        ItemLists.Add(ItemsArmor);
        ItemLists.Add(ItemsWeapon);
        ItemLists.Add(ItemsMaterial);
    }

    private void ItemFill()
    {
        // Here is the place, where all the Lists will be filled!
        Item Apfel = new ItemFood(01, "Apfel", "Ein saftiger Apfel.", 100, 2, 0,
                     new Recipe("Apfel", new Recipe.Ingredient()));
        Item Brustplatte = new ItemArmor(01, "Brustplatte", "Eine Brustplatte.", 1,
                           new Recipe("Brustplatte", new Recipe.Ingredient()));
        Item Grastunika = new ItemArmor(02, "Grastunika", "Eine Tunika aus Gras.", 1,
                           new Recipe("Grastunika", new Recipe.Ingredient("Grasfasern", 6)));
        Item Grashose = new ItemArmor(03, "Grashose", "Eine Hose aus Gras.", 1,
                           new Recipe("Grashose", new Recipe.Ingredient("Grasfasern", 5)));
        Item Grasstiefel = new ItemArmor(04, "Grasstiefel", "Ein paar Stiefel aus Gras.", 1,
                           new Recipe("Grasstiefel", new Recipe.Ingredient("Grasfasern", 4)));
        Item Grashelm = new ItemArmor(05, "Grashelm", "Ein Helm aus Gras.", 1,
                           new Recipe("Grashelm", new Recipe.Ingredient("Grasfasern", 4)));
        Item Holzschwert = new ItemWeapon(01, "Holzschwert", "Ein kleines Schwert aus Holz.", 1,
                           new Recipe("Holzschwert", new Recipe.Ingredient("Schwertgriff", 1), new Recipe.Ingredient("Holz", 2)));
        Item Eisenschwert = new ItemWeapon(02, "Eisenschwert", "Ein Schwert aus Eisen.", 1,
                           new Recipe("Eisenschwert", new Recipe.Ingredient("Schwertgriff", 1), new Recipe.Ingredient("Eisenbarren", 2)));
        Item Holz = new ItemMaterial(01, "Holz", "Ein Stück Holz.", 100,
                    new Recipe("Holz", new Recipe.Ingredient()));
        Item Stofffetzen = new ItemMaterial(02, "Stofffetzen", "Ein verschmutztes Stück Stoff.", 100,
                          new Recipe("Stofffetzen", new Recipe.Ingredient()));
        Item Schwertgriff = new ItemMaterial(03, "Schwertgriff", "Ein Griff für ein Schwert.", 100,
                          new Recipe("Schwertgriff", new Recipe.Ingredient("Holz", 3)));
        Item Eisenbarren = new ItemMaterial(04, "Eisenbarren", "Ein Barren aus einfachem Eisen.", 100,
                          new Recipe("Eisenbarren", new Recipe.Ingredient()));
        Item Gras = new ItemMaterial(05, "Gras", "Einfaches Gras.", 100,
                          new Recipe("Gras", new Recipe.Ingredient()));
        Item Grasfasern = new ItemMaterial(06, "Grasfasern", "Fasern aus wildem Gras.", 100,
                          new Recipe("Grasfasern", new Recipe.Ingredient("Gras", 1)));

        // Fill the Items into the List
        ItemsFood.Add(Apfel);
        ItemsArmor.Add(Brustplatte);
        ItemsArmor.Add(Grastunika);
        ItemsArmor.Add(Grashose);
        ItemsArmor.Add(Grasstiefel);
        ItemsArmor.Add(Grashelm);
        ItemsWeapon.Add(Holzschwert);
        ItemsWeapon.Add(Eisenschwert);
        ItemsMaterial.Add(Holz);
        ItemsMaterial.Add(Stofffetzen);
        ItemsMaterial.Add(Schwertgriff);
        ItemsMaterial.Add(Eisenbarren);
        ItemsMaterial.Add(Gras);
        ItemsMaterial.Add(Grasfasern);
    }
}
