using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour 
{
    //	#########################################
    //	O			Workbench   				O
    //	O---------------------------------------O
    //	O	Author:         					O
    //	O	Date: 02.05.2018				    O
    //	O	Edited: X							O
    //	O	Description: Workbench Script		O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public string m_Identification;

    public bool m_InRange;
    public GameObject m_PlayerObject;
    public float m_SightDistance = 5f;

    public GameObject m_Panel;

    public GameObject m_Slot_1;
    public GameObject m_Slot_2;
    public GameObject m_Slot_3;
    public GameObject m_Slot_4;
    public GameObject m_Slot_Output;

    public Slot m_VirtualSlot_1;
    public Slot m_VirtualSlot_2;
    public Slot m_VirtualSlot_3;
    public Slot m_VirtualSlot_4;
    public Slot m_VirtualSlot_Output;

    private void Awake()
    {
        // Links the virtual Slots with the physical SLots.
        CreateWorkspace();
    }

    // Update is called once per frame
    void Update () 
	{
        m_InRange = CheckForPlayers(m_PlayerObject, m_SightDistance);
        UpdateWorkspace();
	}

    private bool CheckForPlayers(GameObject _Player, float _Range)
    {
        if (GetDistanceTo(_Player.transform.position) <= _Range)
        {
            return true;
        }
        return false;
    }

    private float GetDistanceTo(Vector3 _Target)
    {
        return Vector3.Distance(this.gameObject.transform.position, _Target);
    }

    private void CreateWorkspace()
    {
        m_VirtualSlot_1 = new Slot();
        m_VirtualSlot_1.m_Item = new ItemFood(-1, "Placeholder", "Placeholder!", 1, 0, 0);
        m_Slot_1.GetComponent<Slot>().ChangeSlot(m_VirtualSlot_1);
        m_VirtualSlot_2 = new Slot();
        m_VirtualSlot_2.m_Item = new ItemFood(-1, "Placeholder", "Placeholder!", 1, 0, 0);
        m_Slot_2.GetComponent<Slot>().ChangeSlot(m_VirtualSlot_2);
        m_VirtualSlot_3 = new Slot();
        m_VirtualSlot_3.m_Item = new ItemFood(-1, "Placeholder", "Placeholder!", 1, 0, 0);
        m_Slot_3.GetComponent<Slot>().ChangeSlot(m_VirtualSlot_3);
        m_VirtualSlot_4 = new Slot();
        m_VirtualSlot_4.m_Item = new ItemFood(-1, "Placeholder", "Placeholder!", 1, 0, 0);
        m_Slot_4.GetComponent<Slot>().ChangeSlot(m_VirtualSlot_4);
        m_VirtualSlot_Output = new Slot();
        m_VirtualSlot_Output.m_Item = new ItemFood(-1, "Placeholder", "Placeholder!", 1, 0, 0);
        m_Slot_Output.GetComponent<Slot>().ChangeSlot(m_VirtualSlot_Output);
    }

    private void UpdateWorkspace()
    {
        m_VirtualSlot_1 = m_Slot_1.GetComponent<Slot>();
        m_VirtualSlot_2 = m_Slot_2.GetComponent<Slot>();
        m_VirtualSlot_3 = m_Slot_3.GetComponent<Slot>();
        m_VirtualSlot_4 = m_Slot_4.GetComponent<Slot>();
        m_VirtualSlot_Output = m_Slot_Output.GetComponent<Slot>();
    }
}
