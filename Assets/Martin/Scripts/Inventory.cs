using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    //	O	Description: The Class for the      O
    //	O                Inventory.				O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################


    /// <summary>
    /// All the Slot-Anchors at once.
    /// </summary>
    public struct SlotAnchors
    {
        public float MinX;
        public float MinY;
        public float MaxX;
        public float MaxY;

        /// <summary>
        /// COnstructor of Slot-Anchors.
        /// </summary>
        /// <param name="_MinX"></param>
        /// <param name="_MaxX"></param>
        /// <param name="_MinY"></param>
        /// <param name="_MaxY"></param>
        public SlotAnchors(float _MinX, float _MaxX, float _MinY, float _MaxY)
        {
            MinX = _MinX;
            MinY = _MinY;
            MaxX = _MaxX;
            MaxY = _MaxY;
        }
    }

    /// <summary>
    /// A pair of data, based on their membership.
    /// </summary>
    public struct DataPair
    {
        // The Value as an int
        public int IValue;
        // The rest, that comes from the amount of items that are over the stacksize
        public int IRest;

        /// <summary>
        /// Constructor of the DataPair.
        /// </summary>
        /// <param name="_OperationType"></param>
        /// <param name="_ItemCount"></param>
        /// <param name="_LootCount"></param>
        /// <param name="_StackSize"></param>
        public DataPair(int _OperationType, int _ItemCount, int _LootCount, int _StackSize)
        {
            // If the Operationn-Type is 0...
            if (_OperationType == 0)
            {
                // If the Stacksize is intersected, then split the Data
                if (_ItemCount + _LootCount > _StackSize)   // (96 + 9 == 105) > 100 (example)
                {
                    IValue = _StackSize - _ItemCount;   // 100 - 96 = 4 (example)
                    IRest = _LootCount - IValue;        // rest = 9 - 4 == 5 (example)
                }
                else
                {
                    // Sets the value to Loot-Count.
                    IValue = _LootCount;
                    // And the rest is 0.
                    IRest = 0;
                }
            }
            else    // Everything greater than 0!
            {
                // If the Stacksize is intersected, then split the Data
                if ((_ItemCount - _LootCount) < 0)   // (6 - 9 == -3) < 0 (example)
                {
                    IValue = _ItemCount;   // 6 (Amount) (example)
                    IRest = _LootCount - _ItemCount;        // rest = 9 - 6 == 3 (example)
                }
                else
                {
                    // Sets the value to Loot-Count.
                    IValue = _LootCount;
                    // And the rest is 0.
                    IRest = 0;
                }
            }
        }
    }

    /// <summary>
    /// The Identification of a Slot-Index.
    /// </summary>
    public struct ItemIndexIdentifier
    {
        public int IdentIndex;
        public Slot IdentSlot;

        /// <summary>
        /// Constructor of the ItemIndexIdentifier.
        /// </summary>
        /// <param name="_ItemIndex"></param>
        /// <param name="_ContainedSlot"></param>
        public ItemIndexIdentifier(int _ItemIndex, Slot _ContainedSlot)
        {
            IdentIndex = _ItemIndex;
            IdentSlot = _ContainedSlot;
        }
    }

    /// <summary>
    /// A compact collection of informations about the Item and it's Amount.
    /// </summary>
    public struct ItemDataPackage
    {
        public Item Item;
        public int ItemCount;

        /// <summary>
        /// Constructor of an ItemDataPackage.
        /// </summary>
        /// <param name="_Item"></param>
        /// <param name="_ItemCount"></param>
        public ItemDataPackage(Item _Item, int _ItemCount)
        {
            Item = _Item;
            ItemCount = _ItemCount;
        }
    }

    /// <summary>
    /// This struct does calculate if there are enough Items in "an" Inventory.
    /// </summary>
    public struct HasEnoughItems                    // A copy of the function with almost the same name.
    {
        public bool Result;

        /// <summary>
        /// Checks if there are enough Items in a Panel. Value = Result
        /// </summary>
        /// <param name="_Panel"></param>
        /// <param name="_ItemToCheckName"></param>
        /// <param name="_ItemCount"></param>
        public HasEnoughItems(List<Slot> _SlotList, string _ItemToCheckName, int _ItemCount)
        {
            // The Total-Amount of Items.
            int TotalItemCount = 0;

            // For every Slot in the Slot-List.
            for (int slot = 0; slot < _SlotList.Count; slot++)
            {
                // If the name of the Item in the Slot at Position slot equals to the desired name. 
                if (_SlotList[slot].m_Item.m_IName == _ItemToCheckName)
                {
                    // Add the Amount to the total-Amount
                    TotalItemCount += _SlotList[slot].m_Amount;
                }
            }

            // If the Total-Amount is greather than or equals to the Item-Count...
            if (TotalItemCount >= _ItemCount)
            {
                // Set the Result to true.
                Result = true;
            }
            else
            {
                // Set the Result to false.
                Result = false;
            }
        }
    }

    public ItemManager m_ItemManager;
    public GameObject m_GridPanel;
    public GameObject m_InventoryObject;

    public GameObject m_SlotPrefab;
    public GameObject m_ItemPrefab;

    [HideInInspector]
    public GameObject m_ItemToOverwrite;

    public List<Item> m_inventory;

    public int m_SlotCount = 50;
    public int m_SlotsPerRow = 10;

    void Awake()
    {
        // Finds and sets the ItemManager-Script.
        m_ItemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();

        // Create the List for the Items in the Inventory.
        m_inventory = new List<Item>();

        // Create the List for the needed Slots.
        //m_Slots = new List<Slot>();

        // Creates the Inventory's Slot-Grid
        CreateOnStart(m_inventory, m_GridPanel, m_SlotCount, m_SlotsPerRow);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            string S = MakeSerializible(m_GridPanel);
            Debug.Log(S);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (FindSlot(m_GridPanel, "Apfel") != -1)
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
            if (FindSlot(m_GridPanel, "Holzschwert") != -1)
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
            if (FindSlot(m_GridPanel, "Brustplatte") != -1)
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
    public void AddItem(List<List<Item>> _ListOfItems, int _ListID, int _ItemID, int _AmountOfItemsToAdd)
    {
        // If the List we are peeking into is the List for Food...
        if (_ListOfItems[_ListID] == m_ItemManager.ItemsFood)
        {
            // Look at each of the Items within the List
            foreach (Item item in m_ItemManager.ItemsFood)
            {
                // Create a temporary Item to look for the ID
                ItemFood tmp = (ItemFood)item;

                // If the items ID matches the Id of the searched Item...
                if (tmp.m_FoodID == _ItemID)
                {
                    // Adds the actual Item.
                    AddItemToSlot(ChangeSlotToAdd(tmp), tmp, _AmountOfItemsToAdd);
                    break;
                }
            }

        }

        // If the List we are peeking into is the List for Armor...
        if (_ListOfItems[_ListID] == m_ItemManager.ItemsArmor)
        {
            // Look at each of the Items within the List
            foreach (Item item in m_ItemManager.ItemsArmor)
            {
                // Create a temporary Item to look for the ID
                ItemArmor tmp = (ItemArmor)item;

                // If the items ID matches the Id of the searched Item...
                if (tmp.m_ArmorID == _ItemID)
                {
                    // Adds the actual Item.
                    AddItemToSlot(ChangeSlotToAdd(tmp), tmp, _AmountOfItemsToAdd);
                    break;
                }
            }
        }

        // If the List we are peeking into is the List for Weapons...
        if (_ListOfItems[_ListID] == m_ItemManager.ItemsWeapon)
        {
            // Look at each of the Items within the List
            foreach (Item item in m_ItemManager.ItemsWeapon)
            {
                // Create a temporary Item to look for the ID
                ItemWeapon tmp = (ItemWeapon)item;

                // If the items ID matches the Id of the searched Item...
                if (tmp.m_WeaponID == _ItemID)
                {
                    // Adds the actual Item.
                    AddItemToSlot(ChangeSlotToAdd(tmp), tmp, _AmountOfItemsToAdd);
                    break;
                }
            }
        }

        // If the List we are peeking into is the List for Materials...
        if (_ListOfItems[_ListID] == m_ItemManager.ItemsMaterial)
        {
            // Look at each of the Items within the List
            foreach (Item item in m_ItemManager.ItemsMaterial)
            {
                // Create a temporary Item to look for the ID
                ItemMaterial tmp = (ItemMaterial)item;

                // If the items ID matches the Id of the searched Item...
                if (tmp.m_MaterialID == _ItemID)
                {
                    // Adds the actual Item.
                    AddItemToSlot(ChangeSlotToAdd(tmp), tmp, _AmountOfItemsToAdd);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Removes an Item from an Inventory.
    /// </summary>
    /// <param name="_ListOfItems"></param>
    /// <param name="_ItemID"></param>
    /// <param name="_AmountOfItemsToRemove"></param>
    private void RemoveItem(List<Item> _ListOfItems, int _ItemID, int _AmountOfItemsToRemove)
    {
        // If the List of Items is the List of the specific Item.
        if (_ListOfItems == m_ItemManager.ItemsFood)
        {
            // For each item in the List of Food-Items. 
            foreach (Item item in _ListOfItems)
            {
                // Creates a temp-Item.
                ItemFood tmp = (ItemFood)item;

                // If the ID in the List matches the desired ID.
                if (tmp.m_FoodID == _ItemID)
                {
                    // Is there are enough Items in the Inventory...
                    if (HasEnoughItemsOf(m_GridPanel, tmp, _AmountOfItemsToRemove))
                    {
                        // Removes the specific Item from an Inventory.
                        RemoveItemFromSlot(m_GridPanel, ChangeSlotToRemove(m_GridPanel, tmp), tmp, _AmountOfItemsToRemove);
                        break;
                    }
                }
            }
        }

        // If the List of Items is the List of the specific Item.
        if (_ListOfItems == m_ItemManager.ItemsArmor)
        {
            // For each item in the List of Armor-Items. 
            foreach (Item item in _ListOfItems)
            {
                // Creates a temp-Item.
                ItemArmor tmp = (ItemArmor)item;

                // If the ID in the List matches the desired ID.
                if (tmp.m_ArmorID == _ItemID)
                {
                    // Is there are enough Items in the Inventory...
                    if (HasEnoughItemsOf(m_GridPanel, tmp, _AmountOfItemsToRemove))
                    {
                        // Removes the specific Item from an Inventory.
                        RemoveItemFromSlot(m_GridPanel, ChangeSlotToRemove(m_GridPanel, tmp), tmp, _AmountOfItemsToRemove);
                        break;
                    }
                }
            }
        }

        // If the List of Items is the List of the specific Item.
        if (_ListOfItems == m_ItemManager.ItemsWeapon)
        {
            // For each item in the List of Weapon-Items. 
            foreach (Item item in _ListOfItems)
            {
                // Creates a temp-Item.
                ItemWeapon tmp = (ItemWeapon)item;

                // If the ID in the List matches the desired ID.
                if (tmp.m_WeaponID == _ItemID)
                {
                    // Is there are enough Items in the Inventory...
                    if (HasEnoughItemsOf(m_GridPanel, tmp, _AmountOfItemsToRemove))
                    {
                        // Removes the specific Item from an Inventory.
                        RemoveItemFromSlot(m_GridPanel, ChangeSlotToRemove(m_GridPanel, tmp), tmp, _AmountOfItemsToRemove);
                        break;
                    }
                }
            }
        }

        // If the List of Items is the List of the specific Item.
        if (_ListOfItems == m_ItemManager.ItemsMaterial)
        {
            // For each item in the List of Material-Items. 
            foreach (Item item in _ListOfItems)
            {
                // Creates a temp-Item.
                ItemMaterial tmp = (ItemMaterial)item;

                // If the ID in the List matches the desired ID.
                if (tmp.m_MaterialID == _ItemID)
                {
                    // Is there are enough Items in the Inventory...
                    if (HasEnoughItemsOf(m_GridPanel, tmp, _AmountOfItemsToRemove))
                    {
                        // Removes the specific Item from an Inventory.
                        RemoveItemFromSlot(m_GridPanel, ChangeSlotToRemove(m_GridPanel, tmp), tmp, _AmountOfItemsToRemove);
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Creates an Inventory by the given Information.
    /// </summary>
    /// <param name="_Inventory"></param>
    /// <param name="_Panel"></param>
    /// <param name="_SlotCount"></param>
    /// <param name="_SlotsPerRow"></param>
    public void CreateOnStart(List<Item> _Inventory, GameObject _Panel, int _SlotCount, int _SlotsPerRow)
    {
        // Calculates the Destination.
        float Destination = _SlotCount / (float)_SlotsPerRow;

        // Vectors for the new anchors
        Vector2 StartAnchorMin = new Vector2(0, 1 - (1 / Destination));
        Vector2 StartAnchorMax = new Vector2(0 + (1 / (float)_SlotsPerRow), 1);

        // Calculate the Inventory's Offset of each Slot
        CalculateOffset(_Inventory, _Panel, StartAnchorMin, StartAnchorMax, _SlotCount, _SlotsPerRow);
    }

    /// <summary>
    /// Calculates the Offset for each SLot in the Inventory.
    /// </summary>
    /// <param name="_Inventory"></param>
    /// <param name="_Panel"></param>
    /// <param name="_StartAnchorMin"></param>
    /// <param name="_StartAnchorMax"></param>
    /// <param name="_MaxSlots"></param>
    /// <param name="_SlotWidth"></param>
    private void CalculateOffset(List<Item> _Inventory, GameObject _Panel, Vector2 _StartAnchorMin, Vector2 _StartAnchorMax, int _MaxSlots, int _SlotWidth)
    {
        // Add Counter
        int StepCounter = 1;

        // Anchors for the Last Image.
        Vector2 LastAnchorMin = _StartAnchorMin;
        Vector2 LastAnchorMax = _StartAnchorMax;

        // Vectors for the new anchors
        Vector2 NewAnchorMin = Vector2.zero;
        Vector2 NewAnchorMax = Vector2.zero;

        // Creates a new gameObject.
        GameObject FirstSlotObject = new GameObject("Slot");
        // Adds a Component to the Object.
        Slot FirstSlot = FirstSlotObject.AddComponent<Slot>();
        // Initializes the Slot.
        FirstSlot.Initialize(m_GridPanel);

        // Creates the Slot that should be Drawn.
        Slot SlotToDraw;

        // Creates an Item-Template/ Placeholder
        Item Placeholder = new ItemFood(-1, "Placeholder", "Placeholder!", 1, 0, 0);

        // Sets the Anchor-Min of the first Slot.
        FirstSlot.m_imageSlot.rectTransform.anchorMin = _StartAnchorMin;
        // Sets the Anchor-Max of the first Slot.
        FirstSlot.m_imageSlot.rectTransform.anchorMax = _StartAnchorMax;
        // Finds and chanches the Image.Sprite of the Slot
        FirstSlot.ChangeSprite(FirstSlotObject, Resources.Load<Sprite>("Prefabs/EmptySlot"));

        // Squeezes the Placeholder into the Slots Item.
        _Panel.transform.GetChild(0).GetComponent<Slot>().m_Item = Placeholder;
        // Adds the Placeholder in the virtual Iventory.
        _Inventory.Add(Placeholder);

        // Run the Loop until all Slots are created
        for (int i = 1; i < _MaxSlots; i++)
        {
            // If the current Row is full, recalculate the Positions to switch to a new Row.
            if ((StepCounter % _SlotWidth) > 0)
            {
                // Sets the Anchor-Min to the new Row.
                NewAnchorMin = new Vector2(LastAnchorMax.x, LastAnchorMin.y);
                // Sets the Anchor-Max to the new Row.
                NewAnchorMax = new Vector2(LastAnchorMax.x + (1 / (float)_SlotWidth), LastAnchorMax.y);

            }
            else
            {
                // Sets the Anchor-Min of the Slot to the same Row.
                NewAnchorMin = new Vector2(0, LastAnchorMin.y - (1 / (float)(_MaxSlots / _SlotWidth)));
                // Sets the Anchor-Max of the Slot to the same Row.
                NewAnchorMax = new Vector2(1 / (float)_SlotWidth, LastAnchorMin.y);
            }

            // Creates a new GameObject for the Slot that should be drawn now.
            GameObject SlotToDrawObject = new GameObject("Slot");
            // Adds a Slot-Component to the SlotToDraw.
            SlotToDraw = SlotToDrawObject.AddComponent<Slot>();
            // Initializes the SlotToDraw.
            SlotToDraw.Initialize(m_GridPanel);

            // Sets the Anchor-Min for the SlotToDraw.
            SlotToDraw.m_imageSlot.rectTransform.anchorMin = NewAnchorMin;
            // Sets the Anchor-Max for the SlotToDraw.
            SlotToDraw.m_imageSlot.rectTransform.anchorMax = NewAnchorMax;
            // Changes the Image.Sprite for the First Slot.
            FirstSlot.ChangeSprite(SlotToDrawObject, Resources.Load<Sprite>("Prefabs/EmptySlot"));

            // Squeezes the Placeholder into the Slots Item.
            _Panel.transform.GetChild(i).GetComponent<Slot>().m_Item = Placeholder;

            // Adding a placeholder to the Inventory
            _Inventory.Add(Placeholder);

            // Saves the Anchor-Min in Last-Anchor-Min.
            LastAnchorMin = NewAnchorMin;
            // Saves the Anchor-Max in Last-Anchor-Max.
            LastAnchorMax = NewAnchorMax;

            // Counts up the StepCounter
            StepCounter++;
        }
    }

    /// <summary>
    /// Is the Stacksize reached?
    /// </summary>
    /// <param name="_CurrentSlot"></param>
    /// <param name="_ItemToAdd"></param>
    /// <returns></returns>
    private bool CheckStackSize(Slot _CurrentSlot, Item _ItemToAdd)
    {
        // If the Amountof that Slot it greather than or equals to the Stacksize...
        if (int.Parse(_CurrentSlot.m_textAmount.text) >= _ItemToAdd.m_StackSize)
        {
            // Slot is full, take the next!
            return false;
        }
        else
        {
            // Slot is not full, take this!
            return true;
        }
    }

    /// <summary>
    /// Adds an Item to a Specific Slot.
    /// </summary>
    /// <param name="_Index"></param>
    /// <param name="_ItemToAdd"></param>
    /// <param name="_AmountOfItems"></param>
    private void AddItemToSlot(int _Index, Item _ItemToAdd, int _AmountOfItems)
    {
        // An ItemContainer
        ItemContainer ItemCon;

        // While it's not false.
        while (true)
        {
            // If the Index is somewhere between the lowest and highest posible Number...
            if (_Index <= (m_GridPanel.transform.childCount - 1) && _Index >= 0)
            {
                // If the Itemname at the Slot with the Index of _Index is "Placeholder", which means that the slot is "Empty"
                if (m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Item.m_IName == "Placeholder")
                {
                    // Sets the Item of the specific Slot to the Item that should be placed.
                    m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Item = _ItemToAdd;
                    // Saves the Item in the virtuall Inventory.
                    m_inventory[_Index] = _ItemToAdd;

                    // Creates and saves a new Item, that is going to be overwrited.
                    m_ItemToOverwrite = Instantiate(m_ItemPrefab, m_GridPanel.transform.GetChild(_Index).transform);
                    // Gets the ItemContainer from teh Item that should be overwrited.
                    ItemCon = m_ItemToOverwrite.GetComponent<ItemContainer>();

                    // Changes the Itemprefab of that specific Slot to the ItemPrefab ob the Item To Overwrite.
                    m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_ItemPrefab = m_ItemToOverwrite;
                    // Changes the ItemName of that specific Slot to the ItemName ob the Item To Overwrite.
                    m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_ItemName = _ItemToAdd.m_IName;
                    // Changes the Parent of that specific Slot to the Parent ob the Item To Overwrite.
                    m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Parent = m_GridPanel;
                    // Changes the Text-Object of that specific Slot to the Text-Object ob the Item To Overwrite.
                    m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_textAmount = m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().transform.GetChild(0).GetChild(0).GetComponent<Text>();

                    // Changes the items Sprite to the Icon of the Item, that is going to be added.
                    m_ItemToOverwrite.GetComponent<Image>().sprite = _ItemToAdd.m_Icon;
                    // Sets the Anchor-Min of the Item to x|y.
                    m_ItemToOverwrite.GetComponent<RectTransform>().anchorMin = new Vector2(0.05f, 0.05f);
                    // Sets the Anchor-Max of the Item to x|y.
                    m_ItemToOverwrite.GetComponent<RectTransform>().anchorMax = new Vector2(0.95f, 0.95f);
                    // Sets the Size of the new Item to zero, whitch makes it fitting.
                    m_ItemToOverwrite.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                    // Resets the Anchored-Position to zero.
                    m_ItemToOverwrite.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    // Adds the items-Amount by the AmountOfItems.
                    m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Amount += _AmountOfItems;
                    // Changes the AmountText to the new Amount.
                    m_ItemToOverwrite.transform.GetChild(0).GetComponent<Text>().text = "" + m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Amount;

                    // Changes the Amount og the ItemContainer to the Amount of the Slot.
                    ItemCon.m_Amount = m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Amount;
                    // Changes the name in the ItemContainer to the name of the ItemName inside the Slot.
                    ItemCon.m_ContainedName = m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Item.m_IName;
                    // Changes the Item in the ItemContainer to the Item inside the Slot.
                    ItemCon.m_Item = m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Item;
                    // Sets the Parent of the ItemContainer... Well. To it's Parent.
                    ItemCon.m_Parent = ItemCon.gameObject.transform.parent.gameObject;
                    // And stops the Loop.
                    break;
                }
                else
                {
                    // If the Slots Amount of items is at the limit, seach for another Slot
                    if (m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Amount < m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Item.m_StackSize && m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Item.m_IName == _ItemToAdd.m_IName)
                    {
                        // Sets the ItemCon to the ItemCOntainer of the specific Slot.
                        ItemCon = m_GridPanel.transform.GetChild(_Index).GetChild(0).GetComponent<ItemContainer>();

                        // If the Slot will be full, Split the Amount of Items that you will insert to the Slot
                        // Loot Checken
                        if (_AmountOfItems + m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Amount > m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Item.m_StackSize)
                        {
                            // Devide the Amount of Items that should be added.
                            DataPair DataSplit = new DataPair(0, m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Amount, _AmountOfItems, m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Item.m_StackSize);

                            // Add X to the CurrentAmount.
                            m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Amount += DataSplit.IValue;
                            ItemCon.m_Amount = m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Amount;

                            // Change the text to the new Amount.
                            m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_ItemPrefab.transform.GetChild(0).GetComponent<Text>().text = "" + m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Amount;

                            // The new amount of items that should be added, is the rest of the DataPair.
                            _AmountOfItems = DataSplit.IRest;
                            // Searched for a new Slot.
                            _Index = ChangeSlotToAdd(_ItemToAdd);
                            // Continues with the next.
                            continue;
                        }
                        else
                        {
                            // Add X to the CurrentAmount.
                            m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Amount += _AmountOfItems;
                            // Changes the ItemCons Amount to the Slots Amount.
                            ItemCon.m_Amount = m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>().m_Amount;
                            // Changes the Text of the Item in the Slot.
                            ChangeAmountText(m_GridPanel.transform.GetChild(_Index).GetComponent<Slot>());
                            // Breakes the loop.
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
                            // Changes the Index to the next Slot.
                            _Index = ChangeSlotToAdd(_ItemToAdd);
                            // Breaks the loop.
                            break;
                        }
                        else
                        {
                            // Prints stuff.
                            Debug.Log("Inventar ist voll!");
                            // Breaks the loop.
                            break;
                        }
                    }
                }
            }
            else
            {
                // Prints stuff.
                Debug.Log("Inventar ist voll!");
                // Breaks the loop.
                break;
            }
        }
    }

    /// <summary>
    /// Returns the Index of a Slot, in whitch you can place the Item.
    /// </summary>
    /// <param name="_ItemToAdd"></param>
    /// <returns></returns>
    private int ChangeSlotToAdd(Item _ItemToAdd)
    {
        // The List of ItemIndexIdentifier for all the Slots int the Inventory.
        List<ItemIndexIdentifier> ItemsInInventory = new List<ItemIndexIdentifier>();

        // For every Slot in the Inventory
        for (int slot = 0; slot <= m_GridPanel.transform.childCount - 1; slot++)
        {
            // If the slots item is the item we want to add...
            if (m_GridPanel.transform.GetChild(slot).GetComponent<Slot>().m_Item.m_IName == _ItemToAdd.m_IName)
            {
                // Adds the Existing slots to the List of slots.
                ItemsInInventory.Add(new ItemIndexIdentifier(slot, m_GridPanel.transform.GetChild(slot).GetComponent<Slot>()));
                // Prints the message.
                Debug.Log("Addet ident to list.");
            }
        }

        // If the Count of Items in the Inventory, that match the ItemToAdd...
        if (ItemsInInventory.Count == 0)
        {
            // Returns the first Slot that is free, so that you can place an Item in it.
            return FirstFreeSlotToAdd(_ItemToAdd);
        }
        else
        {
            // Foreach ItemIndexIdentifier...
            foreach (ItemIndexIdentifier ItemIdent in ItemsInInventory)
            {
                // If there is no Item, that has less than stacksize...
                if (ItemIdent.IdentSlot.m_Amount < ItemIdent.IdentSlot.m_Item.m_StackSize)
                {
                    // Thats not the needed result, so move on.
                    return ItemIdent.IdentIndex;
                }
                else
                {
                    // Take the first slot that's not full.
                    continue;
                }
            }

            // Returns the first free Slot, to place an Item in it.
            return FirstFreeSlotToAdd(_ItemToAdd);
        }
    }

    /// <summary>
    /// Finds the first free Slot.
    /// </summary>
    /// <param name="_ItemToAdd"></param>
    /// <returns></returns>
    private int FirstFreeSlotToAdd(Item _ItemToAdd)
    {
        for (int slot = 0; slot <= m_GridPanel.transform.childCount - 1; slot++)
        {
            // No items are matching.
            // Take the first slot that is empty.
            if (m_GridPanel.transform.GetChild(slot).GetComponent<Slot>().m_Item.m_IName == "Placeholder")
            {
                // Return the Slot.
                return slot;
            }
            else
            {
                // If the itemName in the Slot matches the ItemName of the Item yout want to add and...
                if (m_GridPanel.transform.GetChild(slot).GetComponent<Slot>().m_Item.m_IName == _ItemToAdd.m_IName && 
                    // If the Amount of the Item is smaller than the Stacksize...
                    m_GridPanel.transform.GetChild(slot).GetComponent<Slot>().m_Amount < m_GridPanel.transform.GetChild(slot).GetComponent<Slot>().m_Item.m_StackSize)
                {
                    // return the Slot.
                    return slot;
                }
            }
        }

        // No Slot found!
        return -1;
    }

    /// <summary>
    /// Returns the next Slot, where you can remove items from.
    /// </summary>
    /// <param name="_Panel"></param>
    /// <param name="_ItemToRemove"></param>
    /// <returns></returns>
    private int ChangeSlotToRemove(GameObject _Panel, Item _ItemToRemove)
    {
        // All the ItemIndexIdentifiers in the Inventory.
        List<ItemIndexIdentifier> SlotsWithItem = FindSlotsWithItems(_Panel, _ItemToRemove);
        // Sets the smallest size to the double oof the stacksize.
        int SmallestSize = _ItemToRemove.m_StackSize * 2;
        // Create an ItemIndexIdentifier, that it can return later.
        ItemIndexIdentifier ToReturn = new ItemIndexIdentifier(0, new Slot());

        // For every ItemIndexIdentifier in the List of ItemIndexIdentifiers.
        foreach (ItemIndexIdentifier ident in SlotsWithItem)
        {
            // If the Amount of the ItemIndexIdentifier is smaller than or equal to the smallest size...
            if (ident.IdentSlot.m_Amount <= SmallestSize)
            {
                // Sets the ItemIndexIdentifier ToReturn to the current ItemIndexIdentifier.
                ToReturn = ident;
                // Changes the smallest size, to the Amount of the current ItemIndexIdentifier.
                SmallestSize = ident.IdentSlot.m_Amount;
            }
        }

        // Gibt den index vom slot zurück, der die kleinste menge des bestimmten objektes hat!
        return ToReturn.IdentIndex;
    }

    /// <summary>
    /// Finds a Slot by the name of the Item in it.
    /// </summary>
    /// <param name="_Panel"></param>
    /// <param name="_Name"></param>
    /// <returns></returns>
    public int FindSlot(GameObject _Panel, string _Name)
    {
        // If there is at least one Slot to look at...
        if (_Panel.transform.childCount > 0)
        {
            // For every Slot in the Inventory.
            for (int i = 0; i < _Panel.transform.childCount; i++)
            {
                // If the ItemName of the Item in the Slot is the ItemName of the Item you are looking for...
                if (_Panel.transform.GetChild(i).GetComponent<Slot>().m_Item.m_IName == _Name)
                {
                    // Print stuff.
                    Debug.Log("SlotIndex = " + i);
                    // Return Index of the Slot.
                    return i;
                }
            }

        }

        // Slot not found.
        return -1;
    }

    /// <summary>
    /// Removes an Item from a specific Slot.
    /// </summary>
    /// <param name="_Panel"></param>
    /// <param name="_SlotIndex"></param>
    /// <param name="_ItemToRemove"></param>
    /// <param name="_RemoveCount"></param>
    public void RemoveItemFromSlot(GameObject _Panel, int _SlotIndex, Item _ItemToRemove, int _RemoveCount)
    {
        // The ItemContainer of the Current Slot.
        ItemContainer ItemCon = _Panel.transform.GetChild(_SlotIndex).GetChild(0).GetComponent<ItemContainer>();

        // If the Slot-Inddex is not -1.
        if (_SlotIndex != -1)
        {
            // While it's not false.
            while (true)
            {
                // Falls im Slot genug items sind...
                if (_Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount >= _RemoveCount)
                {
                    // Reduces the AMount of the Slot by _removeCount.
                    _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount -= _RemoveCount;
                    // Changes the text of the AMount of the Slot.
                    ChangeAmountText(_Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>());
                    // Changes the Amount of the ItemCOntainer to the Slot's Amount.
                    ItemCon.m_Amount = _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount;
                    // Checks if the Slot needs to be destroyed.
                    CheckToDestroy(_Panel, _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount, _SlotIndex);
                    // Breaks the loop.
                    break;
                }
                else
                {
                    // If there are more than one Slot with that Item...
                    if (FindSlotsWithItems(_Panel, _ItemToRemove).Count > 1)
                    {
                        // Split the Data for the Amount of Items that need to be removed.
                        DataPair RemoveDataSplit = new DataPair(1, _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount, _RemoveCount, _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount);
                        // Reduce the amount of Items by the rest of that Slot
                        _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount -= RemoveDataSplit.IValue;
                        // Changes the Text of the Slot Amount.
                        ChangeAmountText(_Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>());
                        // Changes the AMount of the ItemContainer to the Amount of the Slot.
                        ItemCon.m_Amount = _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount;
                        // Checks if the Slot needs to be destroyed.
                        CheckToDestroy(_Panel, _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount, _SlotIndex);
                        // Reduce the amount of items by the matching part of the DataPair.
                        _RemoveCount = RemoveDataSplit.IRest;
                        // Changes the Slot-Index to the next Slot to remove from.
                        _SlotIndex = ChangeSlotToRemove(_Panel, _ItemToRemove);
                        // Continues with the next Slot.
                        continue;
                    }
                    else
                    {
                        // Breaks the loop.
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Checks if the Slot needs to be destroyed.
    /// </summary>
    /// <param name="_Panel"></param>
    /// <param name="_Amount"></param>
    /// <param name="_SlotIndex"></param>
    private void CheckToDestroy(GameObject _Panel, int _Amount, int _SlotIndex)
    {
        // If the Amount of the Slot is 0...
        if (_Amount == 0)
        {
            // Destroy it.
            DestroyIconFromSlot(_Panel, _SlotIndex);
        }
    }

    /// <summary>
    /// Destroys the Item at the Slot at _SlotIndex.
    /// </summary>
    /// <param name="_Panel"></param>
    /// <param name="_SlotIndex"></param>
    private void DestroyIconFromSlot(GameObject _Panel, int _SlotIndex)
    {
        // Grid panel child an der stelle I ... DESTROY
        for (int i = 0; i < _Panel.transform.childCount; i++)
        {
            // If the current Iterator matches the SlotIndex...
            if (i == _SlotIndex)
            {
                // If the Child of the Grid_Panel at that specific location is not empty...
                if (m_GridPanel.transform.GetChild(i) != null)
                {
                    // Set the Item at the targeted Slot to a Placeholder-Item
                    _Panel.transform.GetChild(i).GetComponent<Slot>().Clear();
                    // Finds the Child-Object of that specific Slot we are looking at.
                    GameObject ObjectToEliminate = m_GridPanel.transform.GetChild(i).transform.GetChild(0).gameObject;
                    // Destroy the Image from the Inventory-Grid-Panel
                    Destroy(ObjectToEliminate);
                    // Break the loop.
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Channges the Text of the Amount of that specific Slot _SlotToChangeText.
    /// </summary>
    /// <param name="_SlotToChangeText"></param>
    public void ChangeAmountText(Slot _SlotToChangeText)
    {
        // Change the text to the new Amount.
        if (_SlotToChangeText != null)
        {
            // Change the Text.
            _SlotToChangeText.m_ItemPrefab.transform.GetChild(0).GetComponent<Text>().text = "" + _SlotToChangeText.m_Amount;
        }
    }

    /// <summary>
    /// Finds all the Slots with the Item _ItemToRemove.
    /// </summary>
    /// <param name="_Panel"></param>
    /// <param name="_ItemToRemove"></param>
    /// <returns></returns>
    private List<ItemIndexIdentifier> FindSlotsWithItems(GameObject _Panel, Item _ItemToRemove)
    {
        // List with all the ItemIndexIdentifiers.
        List<ItemIndexIdentifier> ListTargets = new List<ItemIndexIdentifier>();

        // For all the children in the GameObject.
        for (int i = 0; i < _Panel.transform.childCount; i++)
        {
            // If the ItemName of the item in the Slot matches the ItemName of the _ItemToRemove.
            if (_Panel.transform.GetChild(i).GetComponent<Slot>().m_Item.m_IName == _ItemToRemove.m_IName)
            {
                // Add the ItemIndexIdentifier to the List.
                ListTargets.Add(new ItemIndexIdentifier(i, _Panel.transform.GetChild(i).GetComponent<Slot>()));
            }
        }

        // Returns the filled List.
        return ListTargets;
    }

    /// <summary>
    /// Changes the Data of the two Slots. Slot A will be in SLot B and the other way around.
    /// </summary>
    /// <param name="_ChangeNeeded"></param>
    /// <param name="_DraggedSlot"></param>
    /// <param name="_DroppedSlot"></param>
    /// <param name="_SelectedSlotIndex"></param>
    /// <param name="_DroppedSlotIndex"></param>
    public void ChangeSlotData(bool _ChangeNeeded, GameObject _DraggedSlot, GameObject _DroppedSlot, int _SelectedSlotIndex, int _DroppedSlotIndex)
    {
        // If there is a Change needed...
        if (_ChangeNeeded)
        {
            // Gets the Dragged Slot from the Dragged-Object.
            Slot SlotDrag = _DraggedSlot.GetComponent<Slot>();
            // Gets the Dropped Slot from the Dropped-Object.
            Slot SlotDrop = _DroppedSlot.GetComponent<Slot>();

            // If there is at leat one Child-Object in the Dragged-Slot...
            if (_DraggedSlot.transform.childCount > 0)
            {
                // Sets the Dragged Container to the ItemContainer of the Item.
                ItemContainer ContainerDrag = _DraggedSlot.transform.GetChild(0).GetComponent<ItemContainer>();
                // The Slot drags the ItemContainer from the Item.
                SlotDrag.DragItemContainer(ContainerDrag);
            }
            else
            {
                // Clears the Dragged-Slot.
                SlotDrag.Clear();
            }

            if (_DroppedSlot.transform.childCount > 0)
            {
                // Sets the Dropped Container to the ItemContainer of the Item.
                ItemContainer ContainerDrop = _DroppedSlot.transform.GetChild(0).GetComponent<ItemContainer>();
                // The Slot drags the ItemContainer from the Item.
                SlotDrop.DragItemContainer(ContainerDrop);
            }
            else
            {
                // Clears the Dropped-Slot.
                SlotDrop.Clear();
            }
        }
        else
        {
            // Gets the Dropped Slot from the Dropped-Object.
            Slot SlotDrop = _DroppedSlot.GetComponent<Slot>();
            // Sets the Dropped Container to the ItemContainer of the Item.
            ItemContainer ContainerDrop = _DroppedSlot.transform.GetChild(0).GetComponent<ItemContainer>();
            // The Slot drags the ItemContainer from the Item.
            SlotDrop.DragItemContainer(ContainerDrop);
        }
    }

    /// <summary>
    /// Checks if an Inventory Has anough Items of _ItemToCheck.
    /// </summary>
    /// <param name="_Panel"></param>
    /// <param name="_ItemToCheck"></param>
    /// <param name="_ItemCount"></param>
    /// <returns></returns>
    private bool HasEnoughItemsOf(GameObject _Panel, Item _ItemToCheck, int _ItemCount)
    {
        // The total Count of all the Items.
        int TotalItemCount = 0;

        // For every Child-Object of an GameObject.
        for (int slot = 0; slot < _Panel.transform.childCount; slot++)
        {
            // If the name of the Item at the Current Slot matches the ItemName of the _ItemToCheck.
            if (_Panel.transform.GetChild(slot).GetComponent<Slot>().m_Item.m_IName == _ItemToCheck.m_IName)
            {
                // Add the Amount of the Slot to the total Count.
                TotalItemCount += _Panel.transform.GetChild(slot).GetComponent<Slot>().m_Amount;
            }
        }

        // If the total Count if greather than _ItemCount...
        if (TotalItemCount >= _ItemCount)
        {
            // return true.
            return true;
        }

        // Return not true >.<
        return false;
    }

    /// <summary>
    /// Returns a string containing all the data of the Inventory.
    /// </summary>
    /// <param name="_InventoryPanel"></param>
    /// <returns></returns>
    public string MakeSerializible(GameObject _InventoryPanel)
    {
        // An empty string.
        string Seri = "";

        // For every Child-Object in an GameObject.
        for (int i = 0; i < _InventoryPanel.transform.childCount; i++)
        {
            // If the Amount of children is greather than 0...
            if (_InventoryPanel.transform.GetChild(i).childCount > 0)
            {
                // Add the serialized Slot to the string.
                Seri += _InventoryPanel.transform.GetChild(i).GetComponent<Slot>().GetSerializable();
            }
            else
            {
                // Add "NONE" because there is no Item in that Slot.
                Seri += "NONE";
            }

            // Add a Seperator.
            Seri += "|";
        }

        // Return the serialized Inventory-string.
        return Seri;
    }

    /// <summary>
    /// Deserializes the Inventory.
    /// </summary>
    /// <param name="_Serialized"></param>
    /// <param name="_Panel"></param>
    /// <param name="_ItemManager"></param>
    public void Deserialize(string _Serialized, GameObject _Panel, ItemManager _ItemManager)
    {
        // The serialized data as a string-Array.
        string[] SerializedInventory = _Serialized.Split('|');

        // For every Child-Object in the GameObject.
        for (int i = 0; i < _Panel.transform.childCount; i++)
        {
            // If the string at the Current Index is not "NONE"...
            if (SerializedInventory[i] != "NONE")
            {
                // Desrialize the Current Slot.
                _Panel.transform.GetChild(i).GetComponent<Slot>().Deserialize(SerializedInventory[i], _ItemManager);
            }
            else
            {
                // Continiue witch the next Slot.
                continue;
            }
        }
    }

    /// <summary>
    /// Builds the Inventory from the data in the Slot-Components.
    /// </summary>
    /// <param name="_Panel"></param>
    public void BuildInventory(GameObject _Panel)
    {
        // For every Child-Object in the GameObject.
        for (int i = 0; i < _Panel.transform.childCount; i++)
        {
            // If the ItemName in the Current Slot is not "Placeholder"...
            if (_Panel.transform.GetChild(i).GetComponent<Slot>().m_Item.m_IName != "Placeholder")
            {
                // Creates a new gameObject as an ItemPrefab for the Slot.
                GameObject GO = _Panel.transform.GetChild(i).GetComponent<Slot>().CreateItemPrefab(_Panel.transform.GetChild(i).gameObject);
                // Gets the ReectTransform from the GameObject.
                RectTransform RT = GO.GetComponent<RectTransform>();
                // Sets the Anchor-Min of the Object to x|y.
                RT.anchorMin = new Vector2(0.05f, 0.05f);
                // Sets the Anchor-Max of the Object to x|y.
                RT.anchorMax = new Vector2(0.95f, 0.95f);
                // Sets the Size of the Object to zero.
                RT.sizeDelta = Vector2.zero;
                // Resets the Anchored-Positionto zero.
                RT.anchoredPosition = Vector2.zero;
                // Gets the TectTransform of the Text-Object.
                RectTransform RT2 = GO.transform.GetChild(0).GetComponent<RectTransform>();
                // Sets the Anchor-Min of the Object to x|y.
                RT2.anchorMin = new Vector2(0.5f, 0f);
                // Sets the Anchor-Max of the Object to x|y.
                RT2.anchorMax = new Vector2(1f, 0.5f);
                // Sets the Size of the Object to zero.
                RT2.sizeDelta = Vector2.zero;
                // Resets the Anchored-Positionto zero.
                RT2.anchoredPosition = Vector2.zero;
                // Sets the ItemPrefab of the Item to the Data from the Item.
                _Panel.transform.GetChild(i).GetComponent<Slot>().m_ItemPrefab = _Panel.transform.GetChild(i).GetChild(0).gameObject;
                // Sets the Sprite of the Item to the Data from the Item.
                _Panel.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = _Panel.transform.GetChild(i).GetComponent<Slot>().m_Item.m_Icon;
                // Sets the Parent of the Item to the Data from the Item.
                _Panel.transform.GetChild(i).GetComponent<Slot>().m_Parent = _Panel.transform.GetChild(i).gameObject;
                // Sets the Text-Object of the Item to the Data from the Item.
                _Panel.transform.GetChild(i).GetComponent<Slot>().m_textAmount = _Panel.transform.GetChild(i).transform.GetChild(0).GetChild(0).GetComponent<Text>();
                // Sets the Text of the Item-Amount to the Data from the Item.
                _Panel.transform.GetChild(i).GetComponent<Slot>().m_textAmount.text = _Panel.transform.GetChild(i).GetComponent<Slot>().m_Amount.ToString();
                // The ItemContainer of the Slot finally drags the Slotdata from the SLot below it.
                _Panel.transform.GetChild(i).GetComponent<Slot>().transform.GetChild(0).GetComponent<ItemContainer>().DragSlotData(_Panel.transform.GetChild(i).GetComponent<Slot>());
            }
        }
    }

    /// <summary>
    /// Open the Inventory.
    /// </summary>
    /// <param name="_InventoryPanel"></param>
    public void Close(GameObject _InventoryPanel)
    {
        // Sets the Inventory-Panel to inactive.
        _InventoryPanel.SetActive(false);
    }

    /// <summary>
    /// Close the inventory.
    /// </summary>
    /// <param name="_InventoryPanel"></param>
    public void Open(GameObject _InventoryPanel)
    {
        // Sets the Inventory-Panel to active.
        _InventoryPanel.SetActive(true);
    }
}