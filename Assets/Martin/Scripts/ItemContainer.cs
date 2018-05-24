using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour 
{
    //	#########################################
    //	O			ItemContainer				O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date: 09.05.2018					O
    //	O	Edited: X							O
    //	O	Description: Contains all Data for  O
    //	O	             the Item.              O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public string m_ContainedName;
    [HideInInspector]
    public Item m_Item;
    public int m_Amount;
    [HideInInspector]
    public GameObject m_Parent;

    //public ItemContainer DragData()
    //{
    //    return this;
    //}

    public void DragSlotData(Slot _SlotToDrag)
    {
        this.m_Amount = _SlotToDrag.m_Amount;
        this.m_Item = _SlotToDrag.m_Item;
        this.m_ContainedName = m_Item.m_IName;
        this.m_Parent = _SlotToDrag.gameObject;
    }
}
