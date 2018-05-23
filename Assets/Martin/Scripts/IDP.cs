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

    /// <summary>
    /// Is the Slot-Object accepting the Draggable?
    /// </summary>
    /// <param name="_Draggable"></param>
    /// <returns></returns>
    public bool Accept(IDragable _Draggable)
    {
            // It always does :D
            return true;
    }

    /// <summary>
    /// Is there a need to change the Items?
    /// </summary>
    /// <returns></returns>
    public bool NeedToChange()
    {
        // returns, if the Item has a IDragable somewhere.
        return GetComponentInChildren<IDragable>() != null;
    }

    /// <summary>
    /// When the Process of Dragging is finished.
    /// </summary>
    /// <param name="_DraggedObject"></param>
    public void OnDragFinished(GameObject _DraggedObject)
    {
        // Reset the Anchor-Position of the Dragged-Object.
        _DraggedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
