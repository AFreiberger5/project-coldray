using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    //	#########################################
    //	O			Slot				        O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date: 26.03.2018					O
    //	O	Edited:	X							O
    //	O	Description: The Class for the      O 
    //  O   Slots insiede the Inventory			O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public string path = "/Assets//";

    public Image m_imageSlot;
    public Sprite m_imageIcon;
    public Text m_textAmount;
    public Text m_textKey;

    public GameObject m_SlotPrefab;
    public GameObject m_ItemPrefab;

    public Item m_Item;

    public int m_Amount;

    public Slot(GameObject _SlotPrefab, GameObject _ItemPrefab)
    {
        m_SlotPrefab = _SlotPrefab;
        m_imageSlot = m_SlotPrefab.GetComponent<Image>();
        m_ItemPrefab = _ItemPrefab;
        m_imageIcon = m_ItemPrefab.GetComponent<Image>().sprite;
        m_textAmount = m_ItemPrefab.transform.GetChild(0).GetComponent<Text>();
        m_textKey = m_ItemPrefab.transform.GetChild(1).GetComponent<Text>();

        // The Item inside the Slot is null as default!
        m_Item = null;
        m_Amount = 0;
    }

    public Slot()
    {
        // Creates a Gameobject from out of nowhere, wich represents the SlotPrefab
        m_SlotPrefab = new GameObject("Slot");
        // Adds a fresh RectTransform to that new Slot
        m_SlotPrefab.AddComponent<RectTransform>();
        // Adds a fresh CanvasRenderer to the Slot
        m_SlotPrefab.AddComponent<CanvasRenderer>();
        // And then it adds a new Imgage to the Slot
        m_imageSlot = m_SlotPrefab.AddComponent<Image>();

        // The Item inside the Slot is null as default!
        m_Item = null;
        m_Amount = 0;
    }

    public void ApplyToSlot(Item _ItemToApply)
    {
        m_ItemPrefab.GetComponent<Image>().sprite = _ItemToApply.m_Icon;
        m_ItemPrefab.transform.GetChild(0).GetComponent<Text>().text = this.m_textAmount.text;
        m_ItemPrefab.transform.GetChild(1).GetComponent<Text>().text = this.m_textKey.text;
    }

    
}
