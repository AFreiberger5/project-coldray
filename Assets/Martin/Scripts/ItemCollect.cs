using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollect : MonoBehaviour
{
    //	#########################################
    //	O			ItemCollect				    O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse			    O
    //	O	Date: 03.05.2018					O
    //	O	Edited: X							O
    //	O	Description: Script that handles    O
    //	O	             Player-Interaction.    O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public int List_ID;
    public int Item_ID;
    public int ItemCount;

    private ItemManager m_itemManager;
    private Inventory m_itemInventory;

    private void Awake()
    {
        // Finds the ItemManager in the Scene.
        m_itemManager = FindObjectOfType<ItemManager>();
        // Finds the Inventory in the Scene.
        m_itemInventory = FindObjectOfType<Inventory>();
    }

    private void OnTriggerEnter(Collider _col)
    {
        // If the ItemManager is not null...
        if (m_itemManager != null && m_itemInventory != null)
        {
            // If the Inventory is not null...
            if (_col.gameObject.tag == "Player")
            {
                // Adds an Item to the Inventory or collects it.
                m_itemInventory.AddItem(m_itemManager.ItemLists, List_ID, Item_ID, ItemCount);
                // Destroys the gameObject.
                Destroy(gameObject);
            }
        }
    }
}
