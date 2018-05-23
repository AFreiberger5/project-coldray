using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The spelling is on purpose!
public interface IDragable
{
    //	#########################################
    //	O			IDragable				    O
    //	O---------------------------------------O
    //	O	Author:	Martin Lohse				O
    //	O	Date: 18.04.2018					O
    //	O	Edited: X							O
    //	O	Description: This is an Interface	O
    //	O	             for the dragable Items	O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    // Returns the Draggable GameObject
    GameObject gameObject { get; }
}
