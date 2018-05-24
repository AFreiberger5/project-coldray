using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WorkbenchControl : NetworkBehaviour
{
    //	#########################################
    //	O			WorkbenchControl		    O
    //	O---------------------------------------O
    //	O	Author:	Martin Lohse				O
    //	O	Date: 27.04.2018					O
    //	O	Edited:	X							O
    //	O	Description: This is the Script		O
    //	O	             for the Workbench.     O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public GameObject m_WorkBenchPanel;
    private Workbench m_WorkBenchscript;
    private Dropdown m_WorkbenchDropdown;
    private WorkbenchFind m_workbenchFind;
    private Workbench m_WorkbenchMain;

    // Use this for initialization
    void Awake()
    {
        m_WorkBenchscript = GetComponent<Workbench>();
        m_workbenchFind = GetComponent<WorkbenchFind>();
        m_WorkbenchDropdown = GameObject.Find("Workbench_Dropdown").GetComponent<Dropdown>();
        m_WorkbenchMain = GetComponent<Workbench>();
        Close(m_WorkBenchPanel);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_WorkBenchscript.m_InRange)
        {
            if (Input.GetMouseButtonDown(0) && !m_WorkBenchPanel.activeInHierarchy)
            {
                Open(m_WorkBenchPanel);
            }
        }
        else
        {
            Close(m_WorkBenchPanel);
        }
    }

    public void Open(GameObject _WorkbenchPanel)
    {
        m_WorkbenchDropdown.ClearOptions();
        m_workbenchFind.m_PossibleOutputList.Clear();
        m_WorkBenchPanel.SetActive(true);
    }

    public void Close(GameObject _WorkbenchPanel)
    {
        m_WorkBenchPanel.SetActive(false);
    }

    private GameObject GetWorkbench(string _Name)
    {
        return GameObject.Find(_Name);
    }
}
