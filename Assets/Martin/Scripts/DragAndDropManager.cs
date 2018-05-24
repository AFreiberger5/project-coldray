using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropManager : MonoBehaviour
{
    //	#########################################
    //	O			DragAndDropManager			O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date: 18.04.2018					O
    //	O	Edited: X							O
    //	O	Description: A Manager Class for	O
    //	O	             all the operations of	O
    //	O                the Drag and Drop.  	O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    private GameObject m_currentDragged;
    private Transform m_originalParent;
    private Vector3 m_originalPosition;
    private Vector2 m_lastMousePosition;

    public GameObject m_PlayerInventoryPanel;
    private Inventory m_inventoryscript;
    private Slot m_selectedSlot;

    private bool m_changeNeeded;

    private int m_draggedSlotIndex;
    private int m_droppedSlotIndex;
    private GameObject m_DraggedSlot;
    private GameObject m_splitObject;

    private Item m_draggedItem;
    private Item m_droppedItem;

    private void Start()
    {
        // Gets the Inventoryscript.
        m_inventoryscript = GetComponent<Inventory>();
        m_PlayerInventoryPanel = m_inventoryscript.m_GridPanel;
    }

    /// <summary>
    /// Resets the Current DraggedObject.
    /// </summary>
    private void Reset()
    {
        // Resets the Parent of the CurrentDragged-Object to the original one.
        m_currentDragged.transform.parent = m_originalParent;
        // Resets it's Position.
        m_currentDragged.transform.position = m_originalPosition;
        // And finally drags all the Data back to the Slot.
        m_originalParent.GetComponent<Slot>().DragItemContainer(m_currentDragged.GetComponent<ItemContainer>());
    }

    /// <summary>
    /// Resets the Current SplitObject.
    /// </summary>
    /// <param name="_OriginalParent"></param>
    /// <param name="_Dragged"></param>
    private void ResetSplit(GameObject _OriginalParent, GameObject _Dragged)
    {
        // Merges the Data of the Current SplitObject with it's original Parent.
        _OriginalParent.GetComponent<Slot>().Merge(_Dragged.GetComponent<ItemContainer>());
        // The ItemContainer Drags the Data from the Slot.
        _OriginalParent.transform.GetChild(0).GetComponent<ItemContainer>().DragSlotData(_OriginalParent.GetComponent<Slot>());
        // Destroys the Dragged-Object, witch is or will be the Current SplitObject.
        Destroy(_Dragged);
    }

    /// <summary>
    /// Drop the Base Whoop whoop!
    /// </summary>
    private void Drop()
    {
        // Finds all GameObjects at the MousePosition.
        GameObject[] RaycastHits = RaycastAtMousePosition();

        // If there are more than, or 2 Objects hit...
        if (RaycastHits.Length >= 2)
        {
            // Get the first hit!
            GameObject Container = RaycastHits[1];
            // determined the Index of the Dropped Slot.
            m_droppedSlotIndex = GetSlotIndex(m_PlayerInventoryPanel, Container);

            // If the Container has a Idragable on it...
            if (Container.GetComponent<IDragable>() != null)
            {
                // Take another Gameobject instead.
                Container = RaycastHits[2];
                // determined the Index of the Dropped Slot.
                m_droppedSlotIndex = GetSlotIndex(m_PlayerInventoryPanel, Container);
            }

            // Gets the ITargetDrop from the Container.
            ITargetDrop Target = Container.GetComponent<ITargetDrop>();

            // Checks witch Condition prevails on the DroppedSlot.
            switch(CheckIfOutput(m_currentDragged, Container, m_originalParent.gameObject, m_splitObject))
            {
                // Just go ahead.
                case 0:
                    break;
                case 1:
                    // In this case, Reset the Split-Process.
                    ResetSplit(m_originalParent.gameObject, m_currentDragged);
                    // And finish the Dragging-Process.
                    Target.OnDragFinished(m_currentDragged);
                    // Sets the Current Dragged to null, so that there is no Object attached to the Mouse.
                    m_currentDragged = null;
                    return;
                case 2:
                    // Just Resets the Dragged Object.
                    Reset();
                    // Finishes the Dragging-Process.
                    Target.OnDragFinished(m_currentDragged);
                    // Sets the Current Dragged to null.
                    m_currentDragged = null;
                    return;
            }
            
            // If a Merge is possible.
            if (CheckForMerge(m_currentDragged, Container))
            {
                // Set the Parent of the Current Dragged Object to the DroppedSlot.
                m_currentDragged.transform.parent = Container.transform;
                // Merge both Data, so that the Slots are both in one Slot.
                Merge(m_currentDragged, Container, m_originalParent.gameObject);
                // Jumps back.
                return;
            }

            // If the Currentdragged goes to hell...
            if ((RaycastHits[0].name == "Panel_Inventory" || RaycastHits[1].name == "Panel_Inventory" && 
                m_splitObject != null))
            {
                // Bringt it back, for grace and redemption!
                ResetSplit(m_originalParent.gameObject, m_currentDragged);
                // And peacefully return!
                return;
            }

            // If the Target is not null and the draggable Item as been accepted...
            if (Target != null && Target.Accept(m_currentDragged.GetComponent<IDragable>()))
            {

                // If the Object has to replace another Item...
                if (Target.NeedToChange())
                {
                    // Take the originalTarget.
                    ITargetDrop OriginalTarget = m_originalParent.GetComponent<ITargetDrop>();

                    // And take also the Object to Swap
                    IDragable ItemToSwap = Container.GetComponentInChildren<IDragable>();

                    // if the SplitItem is not null...
                    if (m_splitObject != null)
                    {
                        // Reset the Split-Process.
                        ResetSplit(m_originalParent.gameObject, m_currentDragged);
                        // Jumps back.
                        return;
                    }

                    // If the original target does not accept the Item you want to swap...
                    if (!OriginalTarget.Accept(ItemToSwap) ||
                        Container.GetComponent<IDropBlock>() != null)
                    {
                        // Reset the current Dragged.
                        Reset();
                        // Jumps back.
                        return;
                    }

                    // Sets the Parent of the current Dragged to the transform of the Container.
                    ItemToSwap.gameObject.transform.parent = m_originalParent;
                    // Finishes the Dragging-Process
                    OriginalTarget.OnDragFinished(ItemToSwap.gameObject);
                    // Fixes the Icon of the Item-To-Swap and makes it perfectly fitting.
                    FixIcon(ItemToSwap.gameObject, new Vector2(0.05f, 0.05f), new Vector2(0.95f, 0.95f));
                }

                // Sets the Parent of the current dragged Object to the Container.
                m_currentDragged.transform.parent = Container.transform;
                // Changes the Data of the Changed Slots, to ensure that every SLot, has their items Data.
                m_inventoryscript.ChangeSlotData(m_changeNeeded, m_originalParent.gameObject, Container, m_draggedSlotIndex, m_droppedSlotIndex);

                // Finishes the current dragging-Process.
                Target.OnDragFinished(m_currentDragged);
                // Fixes the Icon of the Current Dragged, so that it's  fitting it like it should.
                FixIcon(m_currentDragged.gameObject, new Vector2(0.05f, 0.05f), new Vector2(0.95f, 0.95f));

                // Changes the bool, that proves if the're is a Change needed.
                m_changeNeeded = false;
            }
            else
            {
                // Resets the Dragging-Process.
                Reset();
            }
        }
        else
        {
            // Resets the Process of Dragging.
            Reset();
        }

        // Stops the Dragging-Process
        m_currentDragged = null;
        // Sets the Split-Object to null, because at this Point it might be placed.
        m_splitObject = null;
    }

    /// <summary>
    /// Gets all the Object in Ui, that are under the Mouse-Position.
    /// </summary>
    /// <returns></returns>
    private GameObject[] RaycastAtMousePosition()
    {
        // A new PointerEventData.
        PointerEventData pointereventdata = new PointerEventData(EventSystem.current);

        // Sets the Data-Position to the Mouse-Position.
        pointereventdata.position = Input.mousePosition;

        // A new List of Objects that got hit by a Rayhast.
        List<RaycastResult> hitresults = new List<RaycastResult>();

        // Takes all the data from the hitresults.
        EventSystem.current.RaycastAll(pointereventdata, hitresults);

        // returns all the GameObjects, that got hit
        return hitresults.Select(hit => hit.gameObject).ToArray();
    }

    void Update()
    {
        // If there is something dragged...
        if (m_currentDragged != null)
        {
            // If the MouseButton was released...
            if (Input.GetMouseButtonUp(0))
            {
                // Than drop it!
                Drop();
                // Jumps back.
                return;
            }
            else
            {
                // Calculate the Offset.
                Vector2 Offset = (Vector2)Input.mousePosition - m_lastMousePosition;

                // Sets the last Mouse-Position to the current Mouse-Position.
                m_lastMousePosition = Input.mousePosition;

                // Gets the Recttransform of the current Dragged.
                RectTransform rect = m_currentDragged.GetComponent<RectTransform>();

                // Translates the Object by X,Y
                rect.position += (Vector3)Offset;
            }
        }
        else
        {
            // If the Mouse was pressed...
            if (Input.GetMouseButtonDown(0))
            {

                // Gets all the GameObjects that got hit again.
                GameObject[] hits = RaycastAtMousePosition();

                // If there are no Objects hit...
                if (hits.Length == 0)
                    return;

                // Gets the Object on the Top.
                GameObject UpperObject = hits[0];

                // Sets the selected-Slot to null.
                GameObject SelectedSlot = null;

                // If there more than One Object below the Mouse.
                if (hits.Length > 1)
                {
                    // Gets the Selected Slot
                    if (hits[1].GetComponent<IDP>() != null)
                    {
                        // Sets the Selected-Slot to the second Object under the Mouse.
                        SelectedSlot = hits[1];
                    }
                    else
                    {
                        // Sets the selected-Slot to null.
                        SelectedSlot = null;
                    }

                }

                // If the UpperObject has Idragable on it...
                if (UpperObject.GetComponent<IDragable>() != null)
                {
                    // Save the Original Parent
                    m_originalParent = UpperObject.transform.parent;

                    // Save the Original Position
                    m_originalPosition = UpperObject.transform.position;

                    // Sets the UpperObjects Parent to the transform.
                    UpperObject.transform.parent = FindObjectOfType<Canvas>().transform;

                    // Sets the current Dragged to the Object on the Top.
                    m_currentDragged = UpperObject;

                    // Sets the LastMousePosition to the current Mouse Pos.
                    m_lastMousePosition = Input.mousePosition;

                    // Get the Slot from the dragged Object. (virtuell)
                    m_originalParent.GetComponent<Slot>().DragItemContainer(m_currentDragged.GetComponent<ItemContainer>());

                    // Clears the Slot at the OriginalParent.
                    m_originalParent.GetComponent<Slot>().Clear();
                }

                // Sets the dragged Slot Index to the correct number.
                m_draggedSlotIndex = GetSlotIndex(m_PlayerInventoryPanel, SelectedSlot);

                // At this Point, a Change will be needed.
                m_changeNeeded = true;

                // If the Current-Dragged is null
                if (m_currentDragged != null)
                // Sets the dragged-Slot to the Current-Dragged.
                    m_DraggedSlot = m_currentDragged.gameObject;

            }

            // If the Left Mouse-Button is pressed down.
            if (Input.GetMouseButtonDown(1))
            {
                // SPlit the Current-Slot.
                Split();
            }
        }
    }

    /// <summary>
    /// Gets the Slot-Index of _Slot.
    /// </summary>
    /// <param name="_InventoryGameObject"></param>
    /// <param name="_Slot"></param>
    /// <returns></returns>
    private int GetSlotIndex(GameObject _InventoryGameObject, GameObject _Slot)
    {
        // For every Child in the Inventory-Panel.
        for (int i = 0; i < _InventoryGameObject.transform.childCount; i++)
        {
            // If the child at position i has an IDP on it and _Slot is nit null...
            if (_InventoryGameObject.transform.GetChild(i).GetComponent<IDP>() != null && _Slot != null)
            {
                // If the GameObject of that Child matches _Slot...
                if (_InventoryGameObject.transform.GetChild(i).gameObject == _Slot)
                {
                    // Return the Slot-Index.
                    return i;
                }
            }
        }

        // Slot not found!
        return -1;
    }

    /// <summary>
    /// Splits The Slot at the Current Mouse-Postion.
    /// </summary>
    private void Split()
    {
        // All the Hit-Objects at the Mouse-Position.
        GameObject[] Hits = RaycastAtMousePosition();

        // If the Count of the Hits is 0...
        if (Hits.Length == 0)
            // Jump Back.
            return;

        // If the COunt of the Hit-Objects is greather than 2...
        if (Hits.Length > 2)
        {
            // If the first Object has an ItemContainer on it...
            if (Hits[0].GetComponent<ItemContainer>() != null)
            {
                // Sets the Split-Object to the Object on the top.
                GameObject SplitObject = Hits[0];
                // Sets the Split-Slot to the Object below the Split-Object.
                GameObject SplitSlot = Hits[1];
                // Finds the Canvas in the Scene.
                GameObject Canvas = GameObject.Find("Canvas");

                // If the Amount of the Split-Object is greather than 1...
                if (SplitObject.GetComponent<ItemContainer>().m_Amount > 1)
                {
                    // Prints something in the Console. (Debug-Window)
                    Debug.Log("SPLIT");

                    // Creates a new Item and saves it as a new GameObject.
                    GameObject GO = SplitSlot.GetComponent<Slot>().CreateItemPrefab(Canvas);
                    // The new Item drags the Data from it's origin.
                    GO.GetComponent<ItemContainer>().DragSlotData(SplitSlot.GetComponent<Slot>());

                    // If the Amount is an odd Number, (ungerade Zahl) then calculate it to the Main-Stack.
                    if (SplitSlot.GetComponent<Slot>().m_Amount % 2 == 1)
                    {
                        // Sets tmp to the whole Amount of the Original-Slot.
                        int tmp = SplitSlot.GetComponent<Slot>().m_Amount;
                        // Sets the Amount of the Split-Slot to the new Amount.
                        SplitSlot.GetComponent<Slot>().m_Amount = (tmp - 1) / 2;
                        // Sets the Amount of the ItemContainer of the new Item to it's new AMount.
                        GO.GetComponent<ItemContainer>().m_Amount = (tmp - 1) / 2 + 1;
                    }
                    else
                    {
                        // Sets tmp to the whole Amount of the Original-Slot.
                        int tmp = SplitSlot.GetComponent<Slot>().m_Amount;
                        // Sets the Amount of the Split-Object to 50%.
                        SplitSlot.GetComponent<Slot>().m_Amount = tmp / 2;
                        // Sets the Amount of the new item also to 50%. 
                        GO.GetComponent<ItemContainer>().m_Amount = tmp / 2;
                    }

                    // Changes the Amount-Text of the Split-Slot.
                    m_inventoryscript.ChangeAmountText(SplitSlot.GetComponent<Slot>());
                    // Sets the Text of the new Item-Amount to its Amount. 
                    GO.transform.GetChild(0).GetComponent<Text>().text = GO.GetComponent<ItemContainer>().m_Amount.ToString();
                    // The Item drags the Data from the Slot. 
                    SplitSlot.transform.GetChild(0).GetComponent<ItemContainer>().DragSlotData(SplitSlot.GetComponent<Slot>());

                    // The Item gets it's Icon, based on the Item's Icon.
                    GO.GetComponent<Image>().sprite = GO.GetComponent<ItemContainer>().m_Item.m_Icon;
                    // Gets the RectTransform of the Object.
                    RectTransform RE = GO.GetComponent<RectTransform>();
                    // Binds if to the Mouse-Position.
                    RE.transform.position = m_lastMousePosition;
                    // Gets the RectTransform of the Text-Object of that Item. 
                    RectTransform RE2 = GO.transform.GetChild(0).GetComponent<RectTransform>();
                    // Sets it's Min-Anchor to x|y.
                    RE2.anchorMin = new Vector2(0.5f, 0.0f);
                    // Sets it's Max-Anchor to x|y.
                    RE2.anchorMax = new Vector2(1.0f, 0.5f);
                    // Resets the Size of the Icon.
                    RE2.sizeDelta = Vector2.zero;
                    
                    // Sets the Parent of the OriginalParent to the Split-Object.  
                    m_originalParent = SplitSlot.gameObject.transform;
                    // Sets Current-Dragged to the Item.
                    m_currentDragged = GO;
                    // And Split-Objectt to the Current-Dragged.
                    m_splitObject = m_currentDragged;
                }
            }
        }
    }

    /// <summary>
    /// Checks if the Items can be merged.
    /// </summary>
    /// <param name="_Dragged"></param>
    /// <param name="_Dropped"></param>
    /// <returns></returns>
    private bool CheckForMerge(GameObject _Dragged, GameObject _Dropped)
    {
        // If the Dragged-Object is not null...
        if (_Dragged != null &&
        // If the Dropped-Object is not null...
            _Dropped != null &&
            // If the Dropped-Slot-Object has an Slot-Component on it...
            _Dropped.GetComponent<Slot>() != null &&
            // If the Dragged-Slot-Object has an ItemContainer-Component on it...
            _Dragged.GetComponent<ItemContainer>() != null &&
            // If the name of the Item in the Container equals to the name of the Item in the Slot...
            _Dragged.GetComponent<ItemContainer>().m_Item.m_Name == _Dropped.GetComponent<Slot>().m_Item.m_Name &&
            // If the Dropped-Slot is not the OriginalParent
            _Dropped != m_originalParent &&
            // If the Split-Object is not the Original-Parent.
            m_splitObject != m_originalParent)
        {
            // They can be merged.
            return true;
        }
        // They can't be merged.
        return false;
    }

    /// <summary>
    /// Merges two Items.
    /// </summary>
    /// <param name="_Dragged"></param>
    /// <param name="_Dropped"></param>
    /// <param name="_OriginalParent"></param>
    private void Merge(GameObject _Dragged, GameObject _Dropped, GameObject _OriginalParent)
    {
        // Prints the victory.
        Debug.Log("Merge!");

        // Gets the Slot from the Dropped-Slot.
        Slot SlotDropped = _Dropped.GetComponent<Slot>();
        // Gets the ItemContainer from the Dragged-Object.
        ItemContainer DragContainer = _Dragged.GetComponent<ItemContainer>();

        // If the Amount of the Dropped-Slot is smaller than the item-Stacksize in that Slot...
        if (SlotDropped.m_Amount < SlotDropped.m_Item.m_StackSize)
        {
            // If the Amount of the Dropped-Slot plus the Item-Count is also smaller than the item-Stacksize in that Slot...  
            if ((SlotDropped.m_Amount + DragContainer.m_Amount) <= SlotDropped.m_Item.m_StackSize)
            {
                // Add the Item-Count.
                SlotDropped.m_Amount += DragContainer.m_Amount;
            }
            else
            {
                // Split the Data obf the Item. 
                Inventory.DataPair Data = new Inventory.DataPair(0, SlotDropped.m_Amount, DragContainer.m_Amount, SlotDropped.m_Item.m_StackSize);

                // Add the Data-Value.
                SlotDropped.m_Amount += Data.IValue;
                // Set the Item-Count to the Rest. 
                DragContainer.m_Amount = Data.IRest;
            }

            // The ItemContainer drags the data of the Slot. 
            _Dropped.transform.GetChild(0).GetComponent<ItemContainer>().DragSlotData(SlotDropped);
            // Sets the Item-Prefab of the Dropped-Slot.
            _Dropped.GetComponent<Slot>().m_ItemPrefab = _Dropped.transform.GetChild(0).gameObject;
            // Changes the Text of the Amount of the Dropped-Slot. 
            m_inventoryscript.ChangeAmountText(SlotDropped);
            // Destroy the Dragged-Object.
            Destroy(_Dragged);
        }

        // And jump back.
        return;
    }

    /// <summary>
    /// determines witch Condition prevails on the Targeted Slot.
    /// </summary>
    /// <param name="_DraggedObject"></param>
    /// <param name="_DroppedSlotObject"></param>
    /// <param name="_OriginalParent"></param>
    /// <param name="_SplitObject"></param>
    /// <returns></returns>
    private int CheckIfOutput(GameObject _DraggedObject, GameObject _DroppedSlotObject, GameObject _OriginalParent, GameObject _SplitObject)
    {
        // If Dropped Slot does Block any Item or the SplitObject is not null...
        if (_DroppedSlotObject.GetComponent<IDropBlock>() != null && _SplitObject != null)
            return 1;
        // If the Dropped Slot does just block any Item...
        else if (_DroppedSlotObject.GetComponent<IDropBlock>() != null)
            return 2;
        else
            // Everything is fine.
            return 0;
    }

    /// <summary>
    /// Fixes the Icon and makes it fitting in its Parent.
    /// </summary>
    /// <param name="_Target"></param>
    /// <param name="_AnchorMin"></param>
    /// <param name="_AnchorMax"></param>
    private void FixIcon(GameObject _Target, Vector2 _AnchorMin, Vector2 _AnchorMax)
    {
        // Gets the RectTransform of the Target.
        RectTransform RE = _Target.GetComponent<RectTransform>();
        // Changes the Anchor-Min.
        RE.anchorMin = _AnchorMin;
        // Changes the Anchor-Max.
        RE.anchorMax = _AnchorMax;
        // Resets the Size of the Icon.
        RE.sizeDelta = Vector2.zero;
    }
}