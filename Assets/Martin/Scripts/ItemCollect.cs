using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemCollect : NetworkBehaviour
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
    private Inventory[] m_itemInventorys;
    private Inventory m_localInventory;

    private void Start()
    {
        // Finds the ItemManager in the Scene.
        m_itemManager = FindObjectOfType<ItemManager>();

    }

    public override void OnStartLocalPlayer()
    {
        // Finds the Inventory in the Scene.
        m_itemInventorys = FindObjectsOfType<Inventory>();

        m_localInventory = GetLocalInventory(m_itemInventorys);
    }

    private Inventory GetLocalInventory(Inventory[] _Inventorys)
    {
        foreach (Inventory inv in _Inventorys)
        {
            if (inv.gameObject.transform.parent.name == "Bobby")
            {
                return inv;
            }
        }

        return null;
    }

    private void OnTriggerEnter(Collider _col)
    {
        // If the ItemManager is not null...
        if (m_itemManager != null && m_localInventory != null)
        {
            // If the Inventory is not null...
            if (_col.gameObject.tag == "Player")
            {
                // Adds an Item to the Inventory or collects it.
                m_localInventory.AddItem(m_itemManager.ItemLists, List_ID, Item_ID, ItemCount);
                // Destroys the gameObject.
                Destroy(gameObject);
                Network.Destroy(gameObject);
            }
        }
    }
}
