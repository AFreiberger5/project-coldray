using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetDrop
{
    //	#########################################
    //	O			ITargetDrop				    O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date: 18.04.2018					O
    //	O	Edited: X							O
    //	O	Description: This is an Interface   O
    //	O	             for the Slots where    O
    //	O	             you can drop the Slot  O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    // Does the Slot accept the Dragged-Object?
    bool Accept(IDragable _Dragged);

    // Need the Items to be changed?
    bool NeedToChange();

    // Do stuff once the Dragging-Process is done.
    void OnDragFinished(GameObject _DraggedObject);
}
