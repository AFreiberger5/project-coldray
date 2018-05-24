using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkbenchFind : MonoBehaviour
{
    //	#########################################
    //	O			WorkbenchFind				O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date: 18.05.2018					O
    //	O	Edited: X							O
    //	O	Description: The Script for the 	O
    //	O	             Find-Button of the     O
    //	O	             Workbench.             O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public List<string> m_PossibleOutputList;
    private ItemManager m_itemManager;
    public List<Slot> m_WorkbenchSlots;
    private Workbench m_workbenchScript;
    private Button_Find m_buttonFindScript;
    private Dropdown m_WorkbenchDropdownScript;
    public Recipe m_currentRecipe;
    public Martin_Utility.RecipeData m_RecipeData;

    private void Awake()
    {
        m_itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        m_buttonFindScript = GameObject.Find("Button_Find").GetComponent<Button_Find>();
        m_WorkbenchDropdownScript = GameObject.Find("Workbench_Dropdown").GetComponent<Dropdown>();
        m_RecipeData = new Martin_Utility.RecipeData(true);
    }

    private void Start()
    {
        m_workbenchScript = GetComponent<Workbench>();
        m_WorkbenchSlots = InitializeSlots(m_workbenchScript.m_VirtualSlot_1,
                                           m_workbenchScript.m_VirtualSlot_2,
                                           m_workbenchScript.m_VirtualSlot_3,
                                           m_workbenchScript.m_VirtualSlot_4);
    }

    private void Update()
    {
        UpdateList(m_WorkbenchSlots);

        if (m_buttonFindScript.m_IsPressed)
        {
            m_PossibleOutputList = FindCraftableItems(m_WorkbenchSlots);
            m_WorkbenchDropdownScript.ClearOptions();
            m_WorkbenchDropdownScript.AddOptions(m_PossibleOutputList);
            m_buttonFindScript.m_IsPressed = false;
        }
    }

    public List<string> FindCraftableItems(List<Slot> _SlotList)
    {
        List<string> tmp = new List<string>();
        bool HasEnough;

        while (m_RecipeData.Recipes.Count > 0)
        {
            m_RecipeData.Recipes.Remove(m_RecipeData.Recipes[0]);
        }

        foreach (List<Item> list in m_itemManager.ItemLists)
        {
            foreach (Item item in list)
            {
                foreach (Recipe R in item.m_Recipes)
                {
                    if (item.m_Recipes.Count > 0)
                    {
                        bool LastBool = false;

                        foreach (Recipe.Ingredient ing in R.m_Ingredients)
                        {

                            Inventory.HasEnoughItems HEI = new Inventory.HasEnoughItems(_SlotList, ing.ItemName, ing.ItemCount);

                            if (HEI.Result &&
                                ing.ItemCount > 0)
                            {
                                // ENough Items of that ingredient.
                                LastBool = true;
                            }
                            else
                            {
                                // Not enough items of that ingredient.
                                LastBool = false;
                            }

                            if (LastBool)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }

                        }

                        if (!LastBool)
                        {
                            break;
                        }
                        else
                        {
                            if (!tmp.Contains(item.m_IName))
                            {
                                tmp.Add(item.m_IName);
                                m_RecipeData.AddRecipe(R);
                            }
                        }

                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        return tmp;
    }

    private List<Slot> InitializeSlots(params Slot[] _SlotsToInitialize)
    {
        List<Slot> tmp = new List<Slot>();

        foreach (Slot slot in _SlotsToInitialize)
        {
            tmp.Add(slot);
        }

        return tmp;
    }

    private void UpdateList(List<Slot> _SlotsToUpdate)
    {
        _SlotsToUpdate[0] = m_workbenchScript.m_VirtualSlot_1;
        _SlotsToUpdate[1] = m_workbenchScript.m_VirtualSlot_2;
        _SlotsToUpdate[2] = m_workbenchScript.m_VirtualSlot_3;
        _SlotsToUpdate[3] = m_workbenchScript.m_VirtualSlot_4;
    }
}
