using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkbenchInventorySetup : MonoBehaviour 
{
    //	#########################################
    //	O			WorkbenchInventorySetup		O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date: 01.05.2018					O
    //	O	Edited: X							O
    //	O	Description: This Script is just    O
    //	O	             recreating the         O
    //	O	             Inventory on the Work- O
    //	O	             bench.                 O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    private Inventory m_playerInventory;
    public GameObject m_PlayerObject;
    private GameObject m_workbenchInventoryPanel;

    // Use this for initialization
    void Awake () 
	{
        m_playerInventory = m_PlayerObject.GetComponent<Inventory>();
        m_workbenchInventoryPanel = transform.GetChild(1).gameObject;
	}

    private void Start()
    {
        CreateInventory();
    }

    // Update is called once per frame
    void Update () 
	{
		
	}

    public void CreateInventory()
    {
    }
}
