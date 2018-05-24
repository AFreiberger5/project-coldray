using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InventoryPlayerSetUpOnline : NetworkBehaviour
{
    //	#########################################
    //	O			InventoryPlayerSetUpOnline  O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse			    O
    //	O	Date: 24.05.2018					O
    //	O	Edited: X							O
    //	O	Description: A Setup for the        O
    //	O	             Inventory. (Online)	O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    private Inventory m_InventoryAtPlayer;
    private ItemManager m_ItemManagerInScene;
    private TestSafeLoad m_TestSafeLoad;
    private GameObject m_workbench;
    private InventoryInGameControll m_InventoryInGameControll;
    private DragAndDropManager m_DragAndDropManagerAtPlayer;

    public override void OnStartLocalPlayer()
    {
        m_InventoryAtPlayer = GetComponent<Inventory>();
        m_TestSafeLoad = GetComponent<TestSafeLoad>();
        m_InventoryInGameControll = GetComponent<InventoryInGameControll>();
        m_ItemManagerInScene = GameObject.Find("ItemManager").GetComponent<ItemManager>();

        m_InventoryAtPlayer.m_ItemManager = m_ItemManagerInScene;
        m_InventoryAtPlayer.m_GridPanel = GameObject.Find("InventoryPanel");
        m_InventoryAtPlayer.m_InventoryObject = GameObject.Find("Panel_Inventory");
        m_DragAndDropManagerAtPlayer = m_InventoryAtPlayer.gameObject.GetComponent<DragAndDropManager>();

        if (GameObject.Find("WorkbenchObject") != null)
        {
            m_workbench = GameObject.Find("WorkbenchObject");
        }

        // Setup of the Player done.
        m_InventoryAtPlayer.DoneSetup = true;
        m_workbench.GetComponent<Workbench>().m_InitDone = true;
        m_TestSafeLoad.InitDone = true;
        m_InventoryInGameControll.m_InitDone = true;
        m_DragAndDropManagerAtPlayer.m_InitDone = true;
    }
}
