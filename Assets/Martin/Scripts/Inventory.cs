using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //	#########################################
    //	O			Inventory		            O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date: 26.03.2018					O
    //	O	Edited: X							O
    //	O	Description:						O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################


    public struct SlotAnchors
    {
        public float MinX;
        public float MinY;
        public float MaxX;
        public float MaxY;

        public SlotAnchors(float _MinX, float _MaxX, float _MinY, float _MaxY)
        {
            MinX = _MinX;
            MinY = _MinY;
            MaxX = _MaxX;
            MaxY = _MaxY;
        }
    }

    public struct DataPair
    {
        // The Value as an int
        public int IValue;
        // The rest, that comes from the amount of items that are over the stacksize
        public int IRest;

        public DataPair(int _OperationType, int _ItemCount, int _LootCount, int _StackSize)
        {
            if (_OperationType == 0)
            {
                // If the Stacksize is intersected, then split the Data
                if (_ItemCount + _LootCount > _StackSize)   // (96 + 9 == 105) > 100
                {
                    IValue = _StackSize - _ItemCount;   // 100 - 96 = 4
                    IRest = _LootCount - IValue;        // rest = 9 - 4 == 5
                }
                else
                {
                    IValue = _LootCount;
                    IRest = 0;
                }
            }
            else    // Everything greater than 0!
            {
                // If the Stacksize is intersected, then split the Data
                if ((_ItemCount - _LootCount) < 0)   // (6 - 9 == -3) < 0
                {
                    IValue = _ItemCount;   // 6 (Amount)
                    IRest = _LootCount - _ItemCount;        // rest = 9 - 6 == 3
                }
                else
                {
                    IValue = _LootCount;
                    IRest = 0;
                }
            }
        }
    }

    public struct ItemIndexIdentifier
    {
        public int IdentIndex;
        public Slot IdentSlot;

        public ItemIndexIdentifier(int _ItemIndex, Slot _ContainedSlot)
        {
            IdentIndex = _ItemIndex;
            IdentSlot = _ContainedSlot;
        }
    }

    public ItemManager m_ItemManager;
    public GameObject m_GridPanel;

    public GameObject m_SlotPrefab;
    public GameObject m_ItemPrefab;

    [HideInInspector]
    public GameObject m_ItemToOverwrite;

    private List<Item> m_inventory;
    private List<Slot> m_slots;

    public int m_Slots = 50;
    public int m_SlotsPerRow = 10;

    void Awake()
    {
        // Finds and sets the ItemManager-Script.
        m_ItemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();

        // Create the List for the Items in the Inventory.
        m_inventory = new List<Item>();

        // Create the List for the needed Slots.
        m_slots = new List<Slot>();

        // Creates the Inventory's Slot-Grid
        CreateOnStart(m_Slots, m_SlotsPerRow);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            int RandomInt = Random.Range(1, 10);
            AddItem(m_ItemManager.ItemsFood, 1, RandomInt);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            int RandomInt = Random.Range(1, 10);
            AddItem(m_ItemManager.ItemsWeapon, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            int RandomInt = Random.Range(1, 10);
            AddItem(m_ItemManager.ItemsArmor, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(m_slots[0].m_Item.m_IName);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (FindSlot(m_slots, "Apfel") != -1)
            {
                RemoveItem(m_ItemManager.ItemsFood, 1, 5);
            }
            else
            {
                Debug.Log("AAHHHHH !!!");
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (FindSlot(m_slots, "Holzschwert") != -1)
            {
                RemoveItem(m_ItemManager.ItemsWeapon, 1, 1);
            }
            else
            {
                Debug.Log("AAHHHHH !!!");
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (FindSlot(m_slots, "Brustplatte") != -1)
            {
                RemoveItem(m_ItemManager.ItemsArmor, 1, 5);
            }
            else
            {
                Debug.Log("AAHHHHH !!!");
            }
        }

    }

    /// <summary>
    /// Adds an Item to your Inventory.
    /// </summary>
    /// <param name="_ListOfItems"></param>
    /// <param name="_ItemID"></param>
    public void AddItem(List<Item> _ListOfItems, int _ItemID, int _AmountOfItemsToAdd)
    {
        // If the List we are peeking into is the List for Food...
        if (_ListOfItems == m_ItemManager.ItemsFood)
        {
            // Look at each of the Items within the List
            foreach (Item item in m_ItemManager.ItemsFood)
            {
                // Create a temporary Item to look for the ID
                ItemFood tmp = (ItemFood)item;

                // If the items ID matches the Id of the searched Item...
                if (tmp.m_FoodID == _ItemID)
                {
                    AddItemToSlot(ChangeSlotToAdd(tmp), tmp, _AmountOfItemsToAdd);
                    break;
                }
            }

        }

        // If the List we are peeking into is the List for Armor...
        if (_ListOfItems == m_ItemManager.ItemsArmor)
        {
            // Look at each of the Items within the List
            foreach (Item item in m_ItemManager.ItemsArmor)
            {
                // Create a temporary Item to look for the ID
                ItemArmor tmp = (ItemArmor)item;

                // If the items ID matches the Id of the searched Item...
                if (tmp.m_ArmorID == _ItemID)
                {
                    AddItemToSlot(ChangeSlotToAdd(tmp), tmp, _AmountOfItemsToAdd);
                    break;
                }
            }
        }

        // If the List we are peeking into is the List for Weapons...
        if (_ListOfItems == m_ItemManager.ItemsWeapon)
        {
            // Look at each of the Items within the List
            foreach (Item item in m_ItemManager.ItemsWeapon)
            {
                // Create a temporary Item to look for the ID
                ItemWeapon tmp = (ItemWeapon)item;

                // If the items ID matches the Id of the searched Item...
                if (tmp.m_WeaponID == _ItemID)
                {
                    AddItemToSlot(ChangeSlotToAdd(tmp), tmp, _AmountOfItemsToAdd);
                    break;
                }
            }
        }
    }

    private void RemoveItem(List<Item> _ListOfItems, int _ItemID, int _AmountOfItemsToRemove)
    {
        if (_ListOfItems == m_ItemManager.ItemsFood)
        {
            foreach (Item item in _ListOfItems)
            {
                ItemFood tmp = (ItemFood)item;

                if (tmp.m_FoodID == _ItemID)
                {
                    RemoveItemFromSlot(ChangeSlotToRemove(tmp), tmp, _AmountOfItemsToRemove);
                    break;
                }
            }
        }

        if (_ListOfItems == m_ItemManager.ItemsArmor)
        {
            foreach (Item item in _ListOfItems)
            {
                ItemArmor tmp = (ItemArmor)item;

                if (tmp.m_ArmorID == _ItemID)
                {
                    RemoveItemFromSlot(ChangeSlotToRemove(tmp), tmp, _AmountOfItemsToRemove);
                    break;
                }
            }
        }

        if (_ListOfItems == m_ItemManager.ItemsWeapon)
        {
            foreach (Item item in _ListOfItems)
            {
                ItemWeapon tmp = (ItemWeapon)item;

                if (tmp.m_WeaponID == _ItemID)
                {
                    RemoveItemFromSlot(ChangeSlotToRemove(tmp), tmp, _AmountOfItemsToRemove);
                    break;
                }
            }
        }
    }

    private void CreateOnStart(int _Slots, int _SlotsPerRow)
    {
        float Destination = _Slots / (float)_SlotsPerRow;

        // Vectors for the new anchors
        Vector2 StartAnchorMin = new Vector2(0, 1 - (1 / Destination));
        Vector2 StartAnchorMax = new Vector2(0 + (1 / (float)_SlotsPerRow), 1);

        // Calculate the Inventory's Offset of each Slot
        CalculateOffset(StartAnchorMin, StartAnchorMax, _Slots, _SlotsPerRow);
    }

    private void CalculateOffset(Vector2 _StartAnchorMin, Vector2 _StartAnchorMax, int _MaxSlots, int _SlotWidth)
    {
        // Add Counter
        int StepCounter = 1;

        // Anchors for the Last Image.
        Vector2 LastAnchorMin = _StartAnchorMin;
        Vector2 LastAnchorMax = _StartAnchorMax;

        // Vectors for the new anchors
        Vector2 NewAnchorMin = Vector2.zero;
        Vector2 NewAnchorMax = Vector2.zero;

        Slot FirstSlot = new Slot(m_SlotPrefab, m_ItemPrefab);
        Slot SlotToDraw;
        GameObject DrawnSlot;


        // Creates an Item-Template/ Placeholder
        Item Placeholder = new ItemFood(-1, "Placeholder", "Placeholder!", 1, 0, 0);

        // Create the first Slot
        FirstSlot.m_imageSlot.rectTransform.anchorMin = _StartAnchorMin;
        FirstSlot.m_imageSlot.rectTransform.anchorMax = _StartAnchorMax;
        FirstSlot.m_imageSlot.rectTransform.sizeDelta = Vector2.zero;
        DrawnSlot = Instantiate(FirstSlot.m_SlotPrefab, m_GridPanel.transform);

        // Squeezes the Placeholder into the Slots Item.
        m_slots.Add(FirstSlot);
        m_slots[0].m_Item = Placeholder;
        m_inventory.Add(Placeholder);

        // Run the Loop until all Slots are created
        for (int i = 1; i < _MaxSlots; i++)
        {
            // If the current Row is full, recalculate the Positions to switch to a new row
            if ((StepCounter % _SlotWidth) > 0)
            {
                // Change Positions to the new Row
                NewAnchorMin = new Vector2(LastAnchorMax.x, LastAnchorMin.y);
                NewAnchorMax = new Vector2(LastAnchorMax.x + (1 / (float)_SlotWidth), LastAnchorMax.y);

            }
            else
            {
                // Change Positions to the last Slots Positions
                NewAnchorMin = new Vector2(0, LastAnchorMin.y - (1 / (float)(_MaxSlots / _SlotWidth)));
                NewAnchorMax = new Vector2(1 / (float)_SlotWidth, LastAnchorMin.y);
            }

            SlotToDraw = new Slot(m_SlotPrefab, m_ItemPrefab);

            // Create the first Slot
            SlotToDraw.m_imageSlot.rectTransform.anchorMin = NewAnchorMin;
            SlotToDraw.m_imageSlot.rectTransform.anchorMax = NewAnchorMax;
            SlotToDraw.m_imageSlot.rectTransform.sizeDelta = Vector2.zero;
            DrawnSlot = Instantiate(SlotToDraw.m_SlotPrefab, m_GridPanel.transform);

            // Adding the Slot that should be placed.
            m_slots.Add(SlotToDraw);

            // Squeezes the Placeholder into the Slots Item.
            m_slots[i].m_Item = Placeholder;

            // Adding a placeholder to the Inventory
            m_inventory.Add(Placeholder);

            LastAnchorMin = NewAnchorMin;
            LastAnchorMax = NewAnchorMax;

            // Counts up the StepCounter
            StepCounter++;
        }
    }

    private bool CheckStackSize(Slot _CurrentSlot, Item _ItemToAdd)
    {
        if (int.Parse(_CurrentSlot.m_textAmount.text) >= _ItemToAdd.m_StackSize)
        {
            // Slot ist voll nimm den nächsten!
            return false;
        }
        else
        {
            // Slot ist noch nicht voll. Hierhin bitte!
            return true;
        }
    }

    private void AddItemToSlot(int _Index, Item _ItemToAdd, int _AmountOfItems)
    {
        while (true)
        {
            // If the Index is somewhere between the lowest and highest posible Number...
            if (_Index <= (m_slots.Count - 1) && _Index >= 0)
            {
                // If the Itemname at the Slot with the Index of _Index is "Placeholder", which means that the slot is "Empty"
                if (m_slots[_Index].m_Item.m_IName == "Placeholder")
                {
                    // Fill the Item in
                    m_slots[_Index].m_Item = _ItemToAdd;
                    m_inventory[_Index] = _ItemToAdd;
                    //m_ItemToOverwrite = Instantiate(m_slots[_Index].m_ItemPrefab, m_GridPanel.transform.GetChild(_Index).transform);
                    m_ItemToOverwrite = Instantiate(m_ItemPrefab, m_GridPanel.transform.GetChild(_Index).transform);
                    m_slots[_Index].m_ItemPrefab = m_ItemToOverwrite;
                    m_ItemToOverwrite.GetComponent<Image>().sprite = _ItemToAdd.m_Icon;
                    m_ItemToOverwrite.GetComponent<RectTransform>().anchorMin = new Vector2(0.05f, 0.05f);
                    m_ItemToOverwrite.GetComponent<RectTransform>().anchorMax = new Vector2(0.95f, 0.95f);
                    m_ItemToOverwrite.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
                    m_ItemToOverwrite.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                    m_slots[_Index].m_Amount += _AmountOfItems;
                    m_ItemToOverwrite.transform.GetChild(0).GetComponent<Text>().text = "" + m_slots[_Index].m_Amount;
                    break;
                }
                else
                {
                    // If the Slots Amount of items is at the limit, seach for another Slot
                    if (m_slots[_Index].m_Amount < m_slots[_Index].m_Item.m_StackSize && m_slots[_Index].m_Item.m_IName == _ItemToAdd.m_IName)
                    {

                        // If the Slot will be full, Split the Amount of Items that you will insert to the Slot
                        // Loot Checken
                        if (_AmountOfItems + m_slots[_Index].m_Amount > m_slots[_Index].m_Item.m_StackSize)
                        {
                            // Devide the Amount of Items that should be added.
                            DataPair DataSplit = new DataPair(0, m_slots[_Index].m_Amount, _AmountOfItems, m_slots[_Index].m_Item.m_StackSize);

                            // Add X to the CurrentAmount.
                            m_slots[_Index].m_Amount += DataSplit.IValue;

                            // Change the text to the new Amount.
                            m_slots[_Index].m_ItemPrefab.transform.GetChild(0).GetComponent<Text>().text = "" + m_slots[_Index].m_Amount;

                            // The new amount of items that should be added, is the rest of the DataPair.
                            _AmountOfItems = DataSplit.IRest;
                            // Searched for a new Slot.
                            _Index = ChangeSlotToAdd(_ItemToAdd);

                            continue;
                        }
                        else
                        {
                            // Add X to the CurrentAmount.
                            m_slots[_Index].m_Amount += _AmountOfItems;

                            ChangeAmountText(m_slots[_Index]);

                            break;
                        }


                    }
                    else
                    {
                        // The items Stacksize is at the limit, so another item cant be placed.
                        int tmp_index = ChangeSlotToAdd(_ItemToAdd);

                        // If there is still Space in the Inventory...
                        if (tmp_index >= 0)
                        {
                            _Index = ChangeSlotToAdd(_ItemToAdd);
                            break;
                        }
                        else
                        {
                            Debug.Log("Inventar ist voll!");
                            break;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Inventar ist voll!");
                break;
            }
        }
    }

    private int ChangeSlotToAdd(Item _ItemToAdd)
    {
        // Alle SLots durchgehen.
        for (int slot = 0; slot <= m_slots.Count - 1; slot++)
        {
            // Wenn der aktuelle Slot leer ist oder die stacksize nicht erreicht wurde und das entsprechnde Item im Slot vorhanden ist...
            if (m_slots[slot].m_Item.m_IName == "Placeholder")
            {
                return slot;
            }
            else
            {
                if (m_slots[slot].m_Item.m_IName == _ItemToAdd.m_IName && m_slots[slot].m_Amount < m_slots[slot].m_Item.m_StackSize)
                {
                    return slot;
                }
                else
                    // slot ist nicht leer.
                    continue;
            }
            // Sonst nochmal durchsuchen oder weiter suchen.
        }

        // Kein freier Platz gefunden!
        return -1;
    }

    private int ChangeSlotToRemove(Item _ItemToRemove)
    {

        List<ItemIndexIdentifier> SlotsWithItem = FindSlotsWithItems(m_slots, _ItemToRemove);

        int SmallestSize = _ItemToRemove.m_StackSize * 2;
        ItemIndexIdentifier ToReturn = new ItemIndexIdentifier(0, new Slot());

        foreach (ItemIndexIdentifier ident in SlotsWithItem)
        {
            if (ident.IdentSlot.m_Amount <= SmallestSize)
            {
                ToReturn = ident;
                SmallestSize = ident.IdentSlot.m_Amount;
            }
        }

        // Gibt den index vom slot zurück, der die kleinste menge des bestimmten objektes hat!
        return ToReturn.IdentIndex;
    }


    // GetItemByIndex(ItemManager.ItemsFood, 1) == Apfel 

    public Item GetItemByIndex(List<Item> _ListOFItems, int _Index)
    {
        if (_ListOFItems == m_ItemManager.ItemsFood)
        {
            foreach (Item item in m_ItemManager.ItemsFood)
            {
                ItemFood tmp = (ItemFood)item;

                if (tmp.m_FoodID == _Index)
                {
                    return item;
                }
            }
        }

        if (_ListOFItems == m_ItemManager.ItemsArmor)
        {
            foreach (Item item in m_ItemManager.ItemsArmor)
            {
                ItemArmor tmp = (ItemArmor)item;

                if (tmp.m_ArmorID == _Index)
                {
                    return item;
                }
            }
        }

        if (_ListOFItems == m_ItemManager.ItemsWeapon)
        {
            foreach (Item item in m_ItemManager.ItemsWeapon)
            {
                ItemWeapon tmp = (ItemWeapon)item;

                if (tmp.m_WeaponID == _Index)
                {
                    return item;
                }
            }
        }

        // If there is no item found...
        return null;
    }

    public Item GetItemByName(List<Item> _ListOFItems, string _ItemName)
    {
        if (_ListOFItems == m_ItemManager.ItemsFood)
        {
            foreach (Item item in m_ItemManager.ItemsFood)
            {
                ItemFood tmp = (ItemFood)item;

                if (tmp.m_IName == _ItemName)
                {
                    return item;
                }
            }
        }

        if (_ListOFItems == m_ItemManager.ItemsArmor)
        {
            foreach (Item item in m_ItemManager.ItemsArmor)
            {
                ItemArmor tmp = (ItemArmor)item;

                if (tmp.m_IName == _ItemName)
                {
                    return item;
                }
            }
        }

        if (_ListOFItems == m_ItemManager.ItemsWeapon)
        {
            foreach (Item item in m_ItemManager.ItemsWeapon)
            {
                ItemWeapon tmp = (ItemWeapon)item;

                if (tmp.m_IName == _ItemName)
                {
                    return item;
                }
            }
        }

        // If there is no item found...
        return null;
    }

    public int FindSlot(List<Slot> _ListToLookAt, string _Name)
    {
        // If there is at least one Slot to look at...
        if (_ListToLookAt.Count > 0)
        {
            for (int i = 0; i < _ListToLookAt.Count; i++)
            {
                if (m_slots[i].m_Item.m_IName == _Name)
                {

                    Debug.Log("SlotIndex = " + i);
                    return i;
                }
            }

        }
        return -1;
    }

    public void RemoveItemFromSlot(int _SlotIndex, Item _ItemToRemove, int _RemoveCount)
    {
        if (_SlotIndex != -1)
        {
            while (true)
            {
                // Falls im Slot genug items sind...
                if (m_slots[_SlotIndex].m_Amount >= _RemoveCount)
                {
                    m_slots[_SlotIndex].m_Amount -= _RemoveCount;

                    ChangeAmountText(m_slots[_SlotIndex]);

                    CheckToDestroy(m_slots[_SlotIndex].m_Amount, _SlotIndex);

                    break;
                }
                else
                {
                    if (FindSlotsWithItems(m_slots, _ItemToRemove).Count > 1)
                    {
                        // Split the Data for the Amount of Items that need to be removed.
                        DataPair RemoveDataSplit = new DataPair(1, m_slots[_SlotIndex].m_Amount, _RemoveCount, m_slots[_SlotIndex].m_Amount);

                        //Reduce the amount of Items by the rest of that Slot
                        m_slots[_SlotIndex].m_Amount -= RemoveDataSplit.IValue;

                        ChangeAmountText(m_slots[_SlotIndex]);

                        CheckToDestroy(m_slots[_SlotIndex].m_Amount, _SlotIndex);

                        // Reduce the amount of items by the matching part of the DataPair.
                        _RemoveCount = RemoveDataSplit.IRest;

                        _SlotIndex = ChangeSlotToRemove(_ItemToRemove);

                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    private void CheckToDestroy(int _Amount, int _SlotIndex)
    {
        if (_Amount == 0)
        {
            DestroyIconFromSlot(_SlotIndex);
        }
    }

    private void DestroyIconFromSlot(int _SlotIndex)
    {
        // Grid panel child an der stelle I ... DESTROY
        for (int i = 0; i < m_slots.Count; i++)
        {
            // If the current Iterator matches the SlotIndex...
            if (i == _SlotIndex)
            {
                // If the Child of the Grid_Panel at that specific location is not empty...
                if (m_GridPanel.transform.GetChild(_SlotIndex) != null)
                {
                    // Set the Item at the targeted Slot to a Placeholder-Item
                    m_slots[_SlotIndex].m_Item = new ItemFood(-1, "Placeholder", "Placeholder!", 1, 0, 0);

                    // Finds the Child-Object of that specific Slot we are looking at.
                    Debug.Log(_SlotIndex);
                    GameObject ObjectToEliminate = m_GridPanel.transform.GetChild(_SlotIndex).transform.GetChild(0).gameObject;

                    // Destroy the Image from the Inventory-Grid-Panel
                    Destroy(ObjectToEliminate);

                    break;
                }
            }
        }

    }

    private void ChangeAmountText(Slot _SlotToChangeText)
    {
        // Change the text to the new Amount.
        _SlotToChangeText.m_ItemPrefab.transform.GetChild(0).GetComponent<Text>().text = "" + _SlotToChangeText.m_Amount;
    }

    private List<ItemIndexIdentifier> FindSlotsWithItems(List<Slot> _InventorySlots, Item _ItemToRemove)
    {
        List<ItemIndexIdentifier> ListTargets = new List<ItemIndexIdentifier>();

        for (int i = 0; i < _InventorySlots.Count; i++)
        {
            if (_InventorySlots[i].m_Item.m_IName == _ItemToRemove.m_IName)
            {
                ListTargets.Add(new ItemIndexIdentifier(i, m_slots[i]));
            }
        }

        return ListTargets;
    }
}