using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Find : MonoBehaviour 
{
    //	#########################################
    //	O			Button_Find				    O
    //	O---------------------------------------O
    //	O	Author:	Martin Lohse				O
    //	O	Date: 18.05.2018					O
    //	O	Edited: X							O
    //	O	Description: Script for the Button	O
    //	O	             to find all craftable  O
    //	O	             Items.                 O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Changes:							O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    // Is the Button pressed or not? This bool will tell ya.
    public bool m_IsPressed;

    /// <summary>
    /// Triggers the bool, that starts the Find-Event of the Workbench.
    /// </summary>
    public void Button_Find_Trigger()
    {
        if (!m_IsPressed)
            m_IsPressed = true;
    }
    
}
