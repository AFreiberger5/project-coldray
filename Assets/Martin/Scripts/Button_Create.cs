using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Create : MonoBehaviour
{
    //	#########################################
    //	O			Button_Create				O
    //	O---------------------------------------O
    //	O	Author:	Martin Lohse				O
    //	O	Date: 18.05.2018					O
    //	O	Edited: X							O
    //	O	Description: The Script for the 	O
    //	O                Item-Creation.         O
    //	O---------------------------------------O
    //	O	Name: 								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    private WorkbenchFind m_workbenchFind;
    private Dropdown m_WorkbenchDropdown;
    private ItemManager m_itemManager;
    private Workbench m_workbench;

    /// <summary>
    /// The Click-Event for the Create-Button of the Workbench.
    /// </summary>
    public void Button_Create_Click()
    {
        m_itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        m_WorkbenchDropdown = GameObject.Find("Workbench_Dropdown").GetComponent<Dropdown>();
        m_workbenchFind = GameObject.Find("Workbench").GetComponent<WorkbenchFind>();
        m_workbenchFind.m_PossibleOutputList = m_workbenchFind.FindCraftableItems(m_workbenchFind.m_WorkbenchSlots);
        m_workbench = GameObject.Find("Workbench").GetComponent<Workbench>();

        // If there is no Item in the Output-Slot or the Item is the same, that should be created...
        if (m_workbench.m_VirtualSlot_Output.m_Item.m_Name == "Placeholder" ||
            m_WorkbenchDropdown.options[m_WorkbenchDropdown.value].text == m_workbench.m_VirtualSlot_Output.m_Item.m_Name)
        {
            // if the Script for the WorkbenchFind is not null and the Count of the list with the names of the Items that can be created,
            // is greather than 0 and the Options of the Dropdown are also more than 0...
            if (m_workbenchFind != null &&
                m_workbenchFind.m_PossibleOutputList.Count > 0 &&
                m_WorkbenchDropdown.options.Count > 0)
            {
                // If the Text of the Options at the desired value is not nothing and the name of the Item, that we wwant to create,
                // is part of the list witwh the possibe Items that can be created...
                if (m_WorkbenchDropdown.options[m_WorkbenchDropdown.value].text != "" &&
                   m_workbenchFind.m_PossibleOutputList.Contains(m_WorkbenchDropdown.options[m_WorkbenchDropdown.value].text))
                {
                    // Reducing the Mats
                    foreach (Recipe.Ingredient ing in m_workbenchFind.m_RecipeData.Recipes[m_WorkbenchDropdown.value].m_Ingredients)
                    {
                        // If the Slot, where we want to reduce the Item is not at Position -1, witch means it's not found...
                        if (Martin_Utility.FindSlot(m_workbench.m_Panel, ing.ItemName) != -1)
                        {
                            // Remove the Item at that Slot by the Amount of the specific Ingredient.
                            Martin_Utility.RemoveItem(Martin_Utility.GetItemList(ing.ItemName, m_itemManager.ItemLists), m_itemManager, Martin_Utility.GetItemID(ing.ItemName, m_itemManager.ItemLists) + 1, ing.ItemCount, m_workbench.m_Panel);
                        }
                        else
                        {
                            // Reset the values and return.
                            Debug.Log("Nicht genug Items!");
                            m_WorkbenchDropdown.value = 0;
                            return;
                        }
                    }

                    // If the Itemname of the Outputslot matches the name of the current selected Option in the Dropdown and the Amount is high
                    // enough to add another Item...
                    if (m_workbench.m_Slot_Output.GetComponent<Slot>().m_Item.m_Name == m_WorkbenchDropdown.options[m_WorkbenchDropdown.value].text &&
                        m_workbench.m_Slot_Output.GetComponent<Slot>().m_Amount + 1 < m_workbench.m_Slot_Output.GetComponent<Slot>().m_Item.m_StackSize)
                    {
                        // Enough Space to Merge the new Item, but there is no need to create a new item.
                        m_workbench.m_Slot_Output.GetComponent<Slot>().m_Amount += 1;
                        // Change the Text to the new Amount.
                        Martin_Utility.ChangeAmountText(m_workbench.m_Slot_Output.GetComponent<Slot>());
                        // Drag the Data of the ParentSlot.
                        m_workbench.m_Slot_Output.transform.GetChild(0).GetComponent<ItemContainer>().DragSlotData(m_workbench.m_Slot_Output.GetComponent<Slot>());
                    }
                    else
                    {
                        // Create a new Item from Scratch and fill in the needed Data.
                        GameObject GO = m_workbench.m_Slot_Output.GetComponent<Slot>().CreateItemPrefab(m_workbench.m_Slot_Output);
                        // Ensures that the Icon is perfectly fitting.
                        Martin_Utility.FixIcon(GO, new Vector2(0.05f, 0.05f), new Vector2(0.95f, 0.95f));
                        // Gets the ItemContainer-Component.
                        ItemContainer IC = GO.GetComponent<ItemContainer>();
                        // Sets the Amount of the new Item to 1.
                        IC.m_Amount = 1;
                        // Finds the Item based on the List, that holds all the ItemTypeLists and the name of the Item.
                        IC.m_Item = Martin_Utility.FindItemByName(m_itemManager.ItemLists, m_WorkbenchDropdown.options[m_WorkbenchDropdown.value].text);
                        // Sets the Parent of the new Item.
                        IC.m_Parent = m_workbench.m_Slot_Output;
                        // Changes the name of the new Item.
                        IC.m_ContainedName = IC.m_Item.m_Name;
                        // Changes the Icon-Sprite to the Icon that comes along with the specific Item.
                        GO.GetComponent<Image>().sprite = IC.m_Item.m_Icon;
                        // Fixes the Icons Position and makes it fitting.
                        Martin_Utility.FixIcon(GO.transform.GetChild(0).gameObject, new Vector2(0.5f, 0f), new Vector2(1f, 0.5f));
                        // Changes the Text of the Amount.
                        GO.transform.GetChild(0).GetComponent<Text>().text = IC.m_Amount.ToString();
                        // drags the Data of the ItemContainer.
                        GO.transform.parent.GetComponent<Slot>().DragItemContainer(IC);
                    }

                    // Clear the data, so that everything is smoothed out.
                    m_WorkbenchDropdown.value = 0;
                    m_WorkbenchDropdown.ClearOptions();
                    m_workbenchFind.m_PossibleOutputList.Clear();
                }
            }
        }
        else
        {
            Debug.Log("Es kann kein Item gecraftet werden!");
        }
    }
}
