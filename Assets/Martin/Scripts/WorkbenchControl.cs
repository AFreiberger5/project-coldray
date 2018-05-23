using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WorkbenchControl : MonoBehaviour
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

    public GameObject WorkBenchPanel;
    private Workbench WorkBenchscript;
    private Dropdown m_WorkbenchDropdown;
    private WorkbenchFind m_workbenchFind;

    // Use this for initialization
    void Awake()
    {
        WorkBenchscript = GetComponent<Workbench>();
        m_workbenchFind = GetComponent<WorkbenchFind>();
        m_WorkbenchDropdown = GameObject.Find("Workbench_Dropdown").GetComponent<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        if (WorkBenchscript.m_InRange)
        {
            if (Input.GetMouseButtonDown(0) && !WorkBenchPanel.activeInHierarchy)
            {
                Open(WorkBenchPanel);
            }
        }
        else
        {
            Close(WorkBenchPanel);
        }
    }

    public void Open(GameObject _WorkbenchPanel)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<FakeHero>().m_currentInteractingObject = GetComponent<Workbench>().m_Identification;
        m_WorkbenchDropdown.ClearOptions();
        m_workbenchFind.m_PossibleOutputList.Clear();
        WorkBenchPanel.SetActive(true);
    }

    public void Close(GameObject _WorkbenchPanel)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<FakeHero>().m_currentInteractingObject = "";
        WorkBenchPanel.SetActive(false);
    }

    private GameObject GetWorkbench(string _Name)
    {
        return GameObject.Find(_Name);
    }
}
