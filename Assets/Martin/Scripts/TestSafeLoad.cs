using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestSafeLoad : NetworkBehaviour 
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

    public string NAME = "Bobby";
    public bool BobbyFound = false;
    public bool InitDone = false;

    private void Initialize()
    {
        Inventata = GetComponent<Inventory>();
        ItemManalulu = GameObject.Find("ItemManager").GetComponent<ItemManager>();

        InventoryPanel = Inventata.m_GridPanel;
    }

    private GameObject GetLocalPlayer(string _LocalPlayerName)
    {
        GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject Go in AllPlayers)
        {
            if (Go.GetComponent<Inventory>() != null && Go.name == _LocalPlayerName)
            {
                Debug.Log("Es ist ein richtiger Bobby /o_o| ... I can't beleve it!");
                BobbyFound = true;
                return Go;
            }
        }
        BobbyFound = false;
        return null;
    }

    // Update is called once per frame
    void Update () 
	{
        if (InitDone)
        {
            Initialize();
            InitDone = false;
        }

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
