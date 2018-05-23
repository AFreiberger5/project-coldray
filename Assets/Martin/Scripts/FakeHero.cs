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

    public GameObject m_InventoryObject;
    public GameObject m_WorkbenchObject;
    private Inventory PlayerInventory;
    private WorkbenchControl m_Workbench;

    public string m_currentInteractingObject;

    private void Awake()
    {
        PlayerInventory = FindObjectOfType<Inventory>();
        m_Workbench = m_WorkbenchObject.GetComponent<WorkbenchControl>();
    }

    // Update is called once per frame
    void Update () 
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
