using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCharacterOnClick : MonoBehaviour
{
    public void LoadThisCharacter()
    {
        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        cd.LoadCharacterOnDummy(gameObject.GetComponentInChildren<Text>().text);
        // TEST
        cd.m_SelectedCharacter = gameObject.GetComponentInChildren<Text>().text;
        // TEST
    }
}