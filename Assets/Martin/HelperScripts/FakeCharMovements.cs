using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FakeCharMovements : MonoBehaviour 
{
    //	#########################################
    //	O			FakeCharMovements			O
    //	O---------------------------------------O
    //	O	Author:	Ich							O
    //	O	Date: Letztens  					O
    //	O	Edited:	XXX							O
    //	O	Description: Booobies! \o/			O
    //	O---------------------------------------O
    //	O	Name:								O
    //	O	Date:								O
    //  O 	Chanes:								O
    //	O---------------------------------------O
    //	O										O
    //	#########################################

    public CharacterController m_Charcon;

	// Use this for initialization
	void Awake () 
	{
        // Gets the Charcon from teh Char.
        m_Charcon = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () 
	{
        ProcessMove();
	}

    private void ProcessMove()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        move = move.normalized;

        move = move * Time.deltaTime * 5;

        move = transform.TransformDirection(move);

        m_Charcon.Move(move);

    }
}
