using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InventoryInGameControll : NetworkBehaviour
{
    //	#########################################
    //	O			InventoryInGameControll	    O
    //	O---------------------------------------O
    //	O	Author:	Martin Lohse				O
    //	O	Date: 24.05.2018					O
    //	O	Edited:	X							O
    //	O	Description: Handles Open and Close	O
    //	O	             or Controll-Stuff for  O
    //	O	             Inventory.             O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public GameObject m_InventoryObject;
    public GameObject m_WorkbenchObject;
    private Inventory PlayerInventory;
    private WorkbenchControl m_Workbench;

    public bool m_InitDone;
    public bool m_InventoryToggle;
    public bool m_InventoryToggleState;

    public string m_currentInteractingObject;

    private void Initialize()
    {
        PlayerInventory = GetComponent<Inventory>();
        m_WorkbenchObject = GameObject.Find("WorkbenchObject");
        m_InventoryObject = PlayerInventory.m_InventoryObject;
        m_Workbench = m_WorkbenchObject.GetComponent<WorkbenchControl>();

    }

    // Update is called once per frame
    void Update()
    {
        if (m_InitDone)
        {
            Initialize();
            m_InitDone = false;
        }

        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                m_InventoryToggleState = !m_InventoryToggleState;
                m_InventoryToggle = true;
            }
        }

        ToggleInventory(m_InventoryToggle);
    }

    private void ToggleInventory(bool _B)
    {
        if (m_InventoryToggleState)
        {
            PlayerInventory.Open(m_InventoryObject);
            _B = false;
        }
        else
        {
            PlayerInventory.Close(m_InventoryObject);
            _B = false;
        }
    }
}
