using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropBlock
{
    //	#########################################
    //	O			IDropBlock 				    O
    //	O---------------------------------------O
    //	O	Author: Martin Lohse				O
    //	O	Date: 19.05.2018					O
    //	O	Edited: X							O
    //	O	Description: This Interface is for  O
    //	O	             every Slot that should O
    //	O	             block input.           O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    // Returns the GameObject, that blocks the Item.
    GameObject gameObject { get; }

    // If this script is atached to a Slot, the Slot won't accept Items.
}
