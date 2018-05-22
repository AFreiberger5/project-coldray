using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//||||||||||||||||||||||||||||||||||||||||||||||||||||\\
//||                                                ||\\
//||            Script by Gregor Hempel             ||\\
//||            23.03.2018                          ||\\
//||            Edits:                              ||\\
//||                                                ||\\
//||||||||||||||||||||||||||||||||||||||||||||||||||||\\

public class LoadCharacterOnClick : MonoBehaviour
{
    /// <summary>
    /// player character buttons use this to load the corresponding character stats
    /// </summary>
    public void LoadThisCharacter()
    {
        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        cd.LoadCharacterOnDummy(gameObject.GetComponentInChildren<Text>().text);// the text stores the characters name

        cd.m_SelectedCharacter = gameObject.GetComponentInChildren<Text>().text;// this information is essential when a game mode gets selected
    }
}