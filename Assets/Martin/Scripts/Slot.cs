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
    public GameObject m_ItemPrefab;
    public Image m_imageSlot;
    public Text m_textAmount;
    public GameObject m_Parent;

    // Important for Serialization.
    public int m_Amount;
    public Item m_Item;
    public string m_ItemName;

    /// <summary>
    /// Initializes the Slot.
    /// </summary>
    /// <param name="_ParentObject"></param>
    public void Initialize(GameObject _ParentObject)
    {
        gameObject.AddComponent<IDP>();
        m_ItemPrefab = null;
        m_imageSlot = GetComponent<Image>();
        gameObject.transform.parent = _ParentObject.transform;
        m_imageSlot = gameObject.AddComponent<Image>();
        Sprite tmp = Resources.Load<Sprite>(path + "Prefabs/EmptySlot");
        m_imageSlot.sprite = tmp;

        // Reset SizeDelta and anchored Position!
        m_imageSlot.rectTransform.sizeDelta = Vector2.zero;
        m_imageSlot.rectTransform.anchoredPosition = Vector2.zero;
    }

    /// <summary>
    /// Creates an Object for the item and attaches it to the Parent.
    /// </summary>
    /// <param name="_ParentObject"></param>
    /// <returns></returns>
    public GameObject CreateItemPrefab(GameObject _ParentObject)
    {
        GameObject IconObject = new GameObject("Item");
        IconObject.AddComponent<RectTransform>();
        IconObject.AddComponent<Image>();
        IconObject.AddComponent<IDR>();
        IconObject.AddComponent<ItemContainer>();
        IconObject.transform.parent = _ParentObject.transform;
        //-----------------------------------------------
        GameObject AmountObject = new GameObject("Text_Amount");
        AmountObject.AddComponent<RectTransform>();
        AmountObject.AddComponent<CanvasRenderer>();
        AmountObject.AddComponent<Text>();
        AmountObject.GetComponent<Text>().raycastTarget = false;
        AmountObject.GetComponent<Text>().resizeTextForBestFit = true;
        AmountObject.GetComponent<Text>().resizeTextMinSize = 10;
        AmountObject.GetComponent<Text>().resizeTextMaxSize = 80;
        AmountObject.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        AmountObject.GetComponent<Text>().fontStyle = FontStyle.Bold;
        AmountObject.GetComponent<Text>().alignment = TextAnchor.LowerRight;
        AmountObject.AddComponent<Outline>();
        AmountObject.transform.parent = IconObject.transform;

        return IconObject;
    }

    /// <summary>
    /// Changes the Sprite of the Slot to a new Sprite. 
    /// </summary>
    /// <param name="UiObject"></param>
    /// <param name="_Sprite"></param>
    /// <returns></returns>
    public Sprite ChangeSprite(GameObject UiObject, Sprite _Sprite)
    {
        return UiObject.GetComponent<Image>().sprite = _Sprite;
    }

    /// <summary>
    /// Changes the name of the Item to the name of the current Item.
    /// </summary>
    public void ChangeItemName()
    {
        m_ItemName = m_Item.m_Name;
    }

    /// <summary>
    /// Changes the Data of a Slot to the Data of the Parameter.
    /// </summary>
    /// <param name="_SlotToChange"></param>
    public void ChangeSlot(Slot _SlotToChange)
    {
        this.m_Amount = _SlotToChange.m_Amount;
        this.m_Item = _SlotToChange.m_Item;
        this.m_Parent = _SlotToChange.m_Parent;
        //-----------------------
        this.m_ItemPrefab = null;
    }

    public void Clear()
    {
        this.m_Amount = 0;
        this.m_Item = new ItemFood(-1, "Placeholder", "Placeholder!", 1, 0, 0);
        this.m_ItemName = "";
        //-----------------------
        this.m_ItemPrefab = null;
    }

    public void DragItemContainer(ItemContainer _ItemContainer)
    {
        this.m_Amount = _ItemContainer.m_Amount;
        this.m_Item = _ItemContainer.m_Item;
        this.m_ItemName = _ItemContainer.m_Item.m_Name;
        this.m_ItemPrefab = _ItemContainer.gameObject;

        this.m_textAmount = GetComponentInChildren<Text>();
        //// If you are dragging an Object from an Object that is not parented...
        //if (this.transform.childCount > 0 && this.transform.GetChild(0).transform.childCount > 0)
        //{
        //    // Get the Text in the SLots children...
        //    this.m_textAmount = this.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        //}

        this.m_Parent = this.gameObject;
    }

    public string GetSerializable()
    {
        string S = "";

        S += this.m_Amount.ToString();
        S += "~";
        S += this.m_Item.GetSerializable();

        return S;
    }

    public void Deserialize(string _Serialized, ItemManager _ItemManager)
    {
        string[] SerializedSlot = _Serialized.Split('~');

        // 0    =   Amount
        // 1    =   List-ID
        // 2    =   Item-ID

        this.m_Amount = System.Convert.ToInt32(SerializedSlot[0]);
        this.m_Item = _ItemManager.ItemLists[System.Convert.ToInt32(SerializedSlot[1])][System.Convert.ToInt32(SerializedSlot[2]) - 1];
        this.m_ItemName = m_Item.m_Name;
        //Eventuell hier noch fehlende daten nachtragen!!!!!!!!!!!!!!!!!!!!
    }

    public void Merge(ItemContainer _ItemToMerge)
    {
        this.m_Amount += _ItemToMerge.m_Amount;
        m_textAmount.text = m_Amount.ToString();
        Destroy(_ItemToMerge.gameObject);
    }
}
