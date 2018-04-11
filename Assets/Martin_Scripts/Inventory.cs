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

        public DataPair(int _ItemCount, int _LootCount, int _StackSize)
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

            //AddItem(m_ItemManager.ItemsFood, 1, RandomInt);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            int RandomInt = Random.Range(1, 10);

            //AddItem(m_ItemManager.ItemsWeapon, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            int RandomInt = Random.Range(1, 10);

            //AddItem(m_ItemManager.ItemsArmor, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            //Debug.Log(m_inventory[0].m_Name);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            //RemoveItemFromSlot(FindSlot(m_slots, "Apfel"), 5);
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
        //bool OperationDone = false;

        while (true)
        {
            // If the Index is somewhere between the lowest and highest posible Number...
            if (_Index <= (m_slots.Count - 1) && _Index >= 0)
            {
                // If the Itemname at the Slot with the Index of _Index is "Placeholder", which means that the slot is "Empty"
                if (m_inventory[_Index].m_Name == "Placeholder")
                {
                    // Fill the Item in
                    m_slots[_Index].m_Item = _ItemToAdd;
                    m_inventory[_Index] = _ItemToAdd;
                    m_ItemToOverwrite = Instantiate(m_slots[_Index].m_ItemPrefab, m_GridPanel.transform.GetChild(_Index).transform);
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
                    if (m_slots[_Index].m_Amount < m_slots[_Index].m_Item.m_StackSize && m_slots[_Index].m_Item.m_Name == _ItemToAdd.m_Name)
                    {

                        // If the Slot will be full, Split the Amount of Items that you will insert to the Slot
                        // Loot Checken
                        if (_AmountOfItems + m_slots[_Index].m_Amount > m_slots[_Index].m_Item.m_StackSize)
                        {
                            // Devide the Amount of Items that should be added.
                            DataPair DataSplit = new DataPair(m_slots[_Index].m_Amount, _AmountOfItems, m_slots[_Index].m_Item.m_StackSize);

                            // Add X to the CurrentAmount.
                            m_slots[_Index].m_Amount += DataSplit.IValue;

                            // Change the text to the new Amount.
                            m_slots[_Index].m_ItemPrefab.transform.GetChild(0).GetComponent<Text>().text = "" + m_slots[_Index].m_Amount;
                            Debug.Log("Add-Index: " + _Index + "| Count: " + m_slots[_Index].m_Amount + "Added: " + _AmountOfItems + " x " + _ItemToAdd.m_Name);

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

                            // Change the text to the new Amount.
                            m_slots[_Index].m_ItemPrefab.transform.GetChild(0).GetComponent<Text>().text = "" + m_slots[_Index].m_Amount;
                            Debug.Log("Add-Index: " + _Index + "| Count: " + m_slots[_Index].m_Amount + "Added: " + _AmountOfItems + " x " + _ItemToAdd.m_Name);
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
            if (m_slots[slot].m_Item.m_Name == "Placeholder")
            {
                return slot;
            }
            else
            {
                if (m_slots[slot].m_Item.m_Name == _ItemToAdd.m_Name && m_slots[slot].m_Amount < m_slots[slot].m_Item.m_StackSize)
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

    public int ChangeSlotToRemove(Item _ItemToRemove)
    {
        // Alle SLots durchgehen.
        for (int slot = 0; slot <= m_slots.Count - 1; slot++)
        {
            if (m_slots[slot].m_Item.m_Name == _ItemToRemove.m_Name && m_slots[slot].m_Amount > 0)
            {
                return slot;
            }
            else
            {
                return -1;
            }
        }

        // Kein freier Platz gefunden!
        return -1;
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

                if (tmp.m_Name == _ItemName)
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

                if (tmp.m_Name == _ItemName)
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

                if (tmp.m_Name == _ItemName)
                {
                    return item;
                }
            }
        }

        // If there is no item found...
        return null;
    }

    public Slot FindSlot(List<Slot> _ListToLookAt, string _Name)
    {
        // If there is at least one Slot to look at...
        if (_ListToLookAt.Count > 0)
        {
            foreach (Slot slot in _ListToLookAt)
            {
                // If the Slots Name matches the Name the function is looking for...
                if (slot.m_Item.m_Name == _Name)
                {
                    return slot;
                }
            }
        }

        // If there is no Slot found...
        return null;
    }

    public void RemoveItemFromSlot(Slot _SlotWithItem, int _RemoveCount)
    {
        // Solange die Anzahl der zu entfernenden Items nicht 0 ist...
        while (_RemoveCount > 0)
        {
            // Falls im Slot genug items sind...
            if (_SlotWithItem.m_Amount >= _RemoveCount && _SlotWithItem.m_Amount > 0)
            {
                _SlotWithItem.m_Amount -= _RemoveCount;

                ChangeAmountText(_SlotWithItem);

                _RemoveCount = 0;

                break;
            }
            else
            {
                // Reduce the amount of items by the number of items in the slot!
                _RemoveCount -= _SlotWithItem.m_Amount;

                _SlotWithItem.m_Amount = 0;

                ChangeAmountText(_SlotWithItem);

                ChangeSlotToRemove(_SlotWithItem.m_Item);

                //DestroyIconFromSlot(_SlotWithItem);

                break;
            }
        }

    }

    private void DestroyIconFromSlot(Slot _SlotToExecute)
    {
        if (_SlotToExecute.m_imageSlot != null)
        {
            _SlotToExecute.m_Item = new ItemFood(-1, "Placeholder", "Placeholder!", 1, 0, 0);
            Destroy(_SlotToExecute.m_ItemPrefab);
        }
    }

    private void ChangeAmountText(Slot _SlotToChangeText)
    {
        // Change the text to the new Amount.
        _SlotToChangeText.m_ItemPrefab.transform.GetChild(0).GetComponent<Text>().text = "" + _SlotToChangeText.m_Amount;
    }

    //      To DO:
    //      
    //  -   wichtig aktuell noch verlust bei loot größer gleich stacksize, da rest nicht impliziert werden kann, falls das inventar voll ist!!!
}