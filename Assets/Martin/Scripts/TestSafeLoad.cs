using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSafeLoad : MonoBehaviour 
{
    //	#########################################
    //	O			TestSafeLoad				O
    //	O---------------------------------------O
    //	O	Author:	P.Enis						O
    //	O	Date:								O
    //	O	Edited:								O
    //	O	Description:						O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public GameObject Player;
    public GameObject InventoryPanel;
    public string Serialisa;

    private Inventory Inventata;
    private ItemManager ItemManalulu;

    private void Awake()
    {
        Inventata = Player.GetComponent<Inventory>();
        ItemManalulu = GetComponent<ItemManager>();
    }

    // Update is called once per frame
    void Update () 
	{
        if (Input.GetKeyDown(KeyCode.T))
        {
            Serialisa = Inventata.MakeSerializible(InventoryPanel);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Inventata.Deserialize(Serialisa, InventoryPanel, ItemManalulu);
            Inventata.BuildInventory(InventoryPanel);
        }
    }
}
