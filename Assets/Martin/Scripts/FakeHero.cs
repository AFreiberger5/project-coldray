using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeHero : MonoBehaviour
{
    //	#########################################
    //	O			FakeHero				    O
    //	O---------------------------------------O
    //	O	Author:								O
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

    private Inventory PlayerInventory;
    private WorkbenchControl m_Workbench;
    private GameObject m_InventoryObject;

    public string m_currentInteractingObject;

    private void Awake()
    {
        PlayerInventory = GetComponent<Inventory>();

        if (GameObject.Find("Workbench") != null)
        {
            m_Workbench = GameObject.Find("Workbench").GetComponent<WorkbenchControl>();
        }

        PlayerInventory.m_GridPanel = GameObject.Find("InventoryPanel");
        m_InventoryObject = PlayerInventory.m_GridPanel.transform.parent.transform.parent.gameObject;
        Debug.Log("Inventory closed after start.");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerInventory.Open(m_InventoryObject);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerInventory.Close(m_InventoryObject);
        }
    }
}
