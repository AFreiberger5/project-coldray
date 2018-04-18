using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDP : MonoBehaviour, ITargetDrop
{
    //	#########################################
    //	O			IDP				            O
    //	O---------------------------------------O
    //	O	Author:	Martin Lohse				O
    //	O	Date: 18.04.2018					O
    //	O	Edited:	X							O
    //	O	Description: This Script ensures    O
    //	O	             that the Slot is set   O
    //	O	             to a Target for        O
    //	O	             draggable Item.        O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public bool Accept(IDragable _Draggable)
    {
        // Here are all the Conditions, that define the circumstances under witch the 
        // Drag is accepted.
        return true;
    }

    public bool NeedToChange()
    {
        return GetComponentInChildren<IDragable>() != null;
    }

    public void OnDragFinished(GameObject _DraggedObject)
    {
        _DraggedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
