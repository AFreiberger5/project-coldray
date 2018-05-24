using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Martin_Utility
{
    //	#########################################
    //	O			Martin_Utility				O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date: 19.05.2018					O
    //	O	Edited: X	    					O
    //	O	Description: Utility Script.	    O
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

    public struct ItemDataPackage
    {
        public Item Item;
        public int ItemCount;

        public ItemDataPackage(Item _Item, int _ItemCount)
        {
            Item = _Item;
            ItemCount = _ItemCount;
        }
    }

    public static Item FindItemByName(List<List<Item>> _Lists, string _ItemName)
    {
        foreach (List<Item> itemList in _Lists)
        {
            foreach (Item item in itemList)
            {
                if (item.m_IName == _ItemName)
                {
                    return item;
                }
            }
        }

        // If there is no item found...
        return null;
    }

    public struct RecipeData
    {
        public List<Recipe> Recipes;

        public RecipeData(bool b)
        {
            if (b)
            {
                Recipes = new List<Recipe>();
            }
            else
            {
                Recipes = null;
            }
        }

        public void AddRecipe(Recipe _RecipeToAdd)
        {
            Recipes.Add(_RecipeToAdd);
        }

        // For the Bachelor-Stuff
        public void AddData(Recipe _RecipeToAdd, string _NameToAdd)
        {
            Recipes.Add(_RecipeToAdd);
        }

        public void RemoveRecipe(Recipe _RecipeToRemove)
        {
            Recipes.Remove(_RecipeToRemove);
        }

        // For the Bachelor-Stuff
        public void RemveData(Recipe _RecipeToRemove, string _NameToRemove)
        {
            Recipes.Remove(_RecipeToRemove);
        }
    }

    public static void FixIcon(GameObject _Target, Vector2 _AnchorMin, Vector2 _AnchorMax)
    {
        RectTransform RE = _Target.GetComponent<RectTransform>();
        RE.anchorMin = _AnchorMin;
        RE.anchorMax = _AnchorMax;
        RE.anchoredPosition = Vector2.zero;
        RE.sizeDelta = Vector2.zero;
    }

    public static int GetItemID(string _ItemName, List<List<Item>> _ItemLists)
    {
        foreach (List<Item> list in _ItemLists)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].m_IName == _ItemName)
                {
                    return i;
                }
            }

        }

        // Not found!
        return -1;
    }

    public static int GetItemListID(string _ItemName, List<List<Item>> _ItemLists)
    {
        for (int i = 0; i < _ItemLists.Count; i++)
        {
            for (int j = 0; j < _ItemLists[i].Count; j++)
            {
                if (_ItemLists[i][j].m_IName == _ItemName)
                {
                    return i;
                }
            }
        }

        // Not found!
        return -1;
    }

    public static List<Item> GetItemList(string _ItemName, List<List<Item>> _ItemLists)
    {
        foreach (List<Item> list in _ItemLists)
        {
            foreach (Item item in list)
            {
                if (item.m_IName == _ItemName)
                {
                    return list;
                }
            }
        }

        // Not found!
        return null;
    }

    public static void RemoveItemFromSlot(GameObject _Panel, int _SlotIndex, Item _ItemToRemove, int _RemoveCount)
    {
        ItemContainer ItemCon = _Panel.transform.GetChild(_SlotIndex).GetChild(0).GetComponent<ItemContainer>();

        if (_SlotIndex != -1)
        {
            while (true)
            {
                // Falls im Slot genug items sind...
                if (_Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount >= _RemoveCount)
                {
                    _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount -= _RemoveCount;

                    ChangeAmountText(_Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>());

                    ItemCon.m_Amount = _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount;

                    CheckToDestroy(_Panel, _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount, _SlotIndex);

                    break;
                }
                else
                {
                    if (FindSlotsWithItems(_Panel, _ItemToRemove).Count > 1)
                    {
                        // Split the Data for the Amount of Items that need to be removed.
                        DataPair RemoveDataSplit = new DataPair(1, _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount, _RemoveCount, _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount);

                        //Reduce the amount of Items by the rest of that Slot
                        _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount -= RemoveDataSplit.IValue;

                        ChangeAmountText(_Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>());

                        ItemCon.m_Amount = _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount;

                        CheckToDestroy(_Panel, _Panel.transform.GetChild(_SlotIndex).GetComponent<Slot>().m_Amount, _SlotIndex);

                        // Reduce the amount of items by the matching part of the DataPair.
                        _RemoveCount = RemoveDataSplit.IRest;

                        _SlotIndex = ChangeSlotToRemove(_Panel, _ItemToRemove);

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

    public static void RemoveItem(List<Item> _ListOfItems, ItemManager _ItemManager, int _ItemID, int _AmountOfItemsToRemove, GameObject _Panel)
    {
        Debug.Log("Item-ID: " + _ItemID + " | " + "Amount: " + _AmountOfItemsToRemove);

        if (_ListOfItems == _ItemManager.ItemsFood)
        {
            foreach (Item item in _ListOfItems)
            {
                ItemFood tmp = (ItemFood)item;

                if (tmp.m_FoodID == _ItemID)
                {

                    if (HasEnoughItemsOf(_Panel, tmp, _AmountOfItemsToRemove))
                    {
                        RemoveItemFromSlot(_Panel, ChangeSlotToRemove(_Panel, tmp), tmp, _AmountOfItemsToRemove);
                        break;
                    }
                }
            }
        }

        if (_ListOfItems == _ItemManager.ItemsArmor)
        {
            foreach (Item item in _ListOfItems)
            {
                ItemArmor tmp = (ItemArmor)item;

                if (tmp.m_ArmorID == _ItemID)
                {
                    if (HasEnoughItemsOf(_Panel, tmp, _AmountOfItemsToRemove))
                    {
                        RemoveItemFromSlot(_Panel, ChangeSlotToRemove(_Panel, tmp), tmp, _AmountOfItemsToRemove);
                        break;
                    }
                }
            }
        }

        if (_ListOfItems == _ItemManager.ItemsWeapon)
        {
            foreach (Item item in _ListOfItems)
            {
                ItemWeapon tmp = (ItemWeapon)item;

                if (tmp.m_WeaponID == _ItemID)
                {
                    if (HasEnoughItemsOf(_Panel, tmp, _AmountOfItemsToRemove))
                    {
                        RemoveItemFromSlot(_Panel, ChangeSlotToRemove(_Panel, tmp), tmp, _AmountOfItemsToRemove);
                        break;
                    }
                }
            }
        }

        if (_ListOfItems == _ItemManager.ItemsMaterial)
        {
            foreach (Item item in _ListOfItems)
            {
                ItemMaterial tmp = (ItemMaterial)item;

                if (tmp.m_MaterialID == _ItemID)
                {
                    if (HasEnoughItemsOf(_Panel, tmp, _AmountOfItemsToRemove))
                    {
                        RemoveItemFromSlot(_Panel, ChangeSlotToRemove(_Panel, tmp), tmp, _AmountOfItemsToRemove);
                        break;
                    }
                }
            }
        }
    }

    public static int FindSlot(GameObject _Panel, string _Name)
    {
        // If there is at least one Slot to look at...
        if (_Panel.transform.childCount > 0)
        {
            for (int i = 0; i < _Panel.transform.childCount; i++)
            {
                if (_Panel.transform.GetChild(i).GetComponent<Slot>().m_Item.m_IName == _Name)
                {
                    Debug.Log("SlotIndex = " + i);
                    return i;
                }
            }

        }
        return -1;
    }

    private static bool HasEnoughItemsOf(GameObject _Panel, Item _ItemToCheck, int _ItemCount)
    {
        int TotalItemCount = 0;

        for (int slot = 0; slot < _Panel.transform.childCount; slot++)
        {
            if (_Panel.transform.GetChild(slot).GetComponent<Slot>().m_Item.m_IName == _ItemToCheck.m_IName)
            {
                TotalItemCount += _Panel.transform.GetChild(slot).GetComponent<Slot>().m_Amount;
            }
        }

        if (TotalItemCount >= _ItemCount)
        {
            return true;
        }
        return false;
    }

    public static void ChangeAmountText(Slot _SlotToChangeText)
    {
        // Change the text to the new Amount.
        if (_SlotToChangeText != null)
        {
            _SlotToChangeText.m_ItemPrefab.transform.GetChild(0).GetComponent<Text>().text = "" + _SlotToChangeText.m_Amount;
        }
    }

    private static void CheckToDestroy(GameObject _Panel, int _Amount, int _SlotIndex)
    {
        if (_Amount == 0)
        {
            DestroyIconFromSlot(_Panel, _SlotIndex);
        }
    }

    private static void DestroyIconFromSlot(GameObject _Panel, int _SlotIndex)
    {
        // Grid panel child an der stelle I ... DESTROY
        for (int i = 0; i < _Panel.transform.childCount; i++)
        {
            // If the current Iterator matches the SlotIndex...
            if (i == _SlotIndex)
            {
                // If the Child of the Grid_Panel at that specific location is not empty...
                if (_Panel.transform.GetChild(i) != null)
                {
                    // Set the Item at the targeted Slot to a Placeholder-Item
                    _Panel.transform.GetChild(i).GetComponent<Slot>().Clear();

                    // Finds the Child-Object of that specific Slot we are looking at.
                    GameObject ObjectToEliminate = _Panel.transform.GetChild(i).transform.GetChild(0).gameObject;

                    // Destroy the Image from the Inventory-Grid-Panel
                    GameObject.Destroy(ObjectToEliminate);

                    break;
                }
            }
        }
    }

    private static int ChangeSlotToRemove(GameObject _Panel, Item _ItemToRemove)
    {
        List<ItemIndexIdentifier> SlotsWithItem = FindSlotsWithItems(_Panel, _ItemToRemove);

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

    private static List<ItemIndexIdentifier> FindSlotsWithItems(GameObject _Panel, Item _ItemToRemove)
    {
        List<ItemIndexIdentifier> ListTargets = new List<ItemIndexIdentifier>();

        for (int i = 0; i < _Panel.transform.childCount; i++)
        {
            if (_Panel.transform.GetChild(i).GetComponent<Slot>().m_Item.m_IName == _ItemToRemove.m_IName)
            {
                ListTargets.Add(new ItemIndexIdentifier(i, _Panel.transform.GetChild(i).GetComponent<Slot>()));
            }
        }

        return ListTargets;
    }
}
