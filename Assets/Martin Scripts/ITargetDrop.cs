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
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    bool Accept(IDragable _Dragged);
    bool NeedToChange();
    void OnDragFinished(GameObject _DraggedObject);
}
