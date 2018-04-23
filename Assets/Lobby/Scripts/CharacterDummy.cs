using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDummy : MonoBehaviour
{
    // TEST
    public Text m_CharacterInspectorName;
    public Text m_ModeInspectorName;
    public string m_SelectedCharacter = "";
    // TEST

    public string m_DummyName;

    public int[] m_DummyModel = new int[7]
    {
        0,
        0,
        0,
        0,
        0,
        0,
        0
    };

    private void Update()
    {
        if (m_SelectedCharacter != "")
        {
            m_CharacterInspectorName.text = m_SelectedCharacter;
            m_ModeInspectorName.text = m_SelectedCharacter;
        }
        else
        {
            m_CharacterInspectorName.text = "";
            m_ModeInspectorName.text = "";
        }
    }

    public void DontDestroyDummyOnLoad()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SaveDummy()
    {
        CharacterStats cs = new CharacterStats(m_DummyName, m_DummyModel);
        SaveLoadManager.SaveCharacter(cs);
    }

    public void LoadCharacterOnDummy(string _selectedCharacter)
    {
        string selectedCharacter = _selectedCharacter;
        CharacterStats cs = SaveLoadManager.LoadCharacter(selectedCharacter);
        m_DummyName = cs.m_StatsName;
        m_DummyModel = cs.m_Model;

        // BUILD DUMMY HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }

    public void BuildDummy()
    {
        // ARBEIT, ARBEIT.

    }

    // ????????????????????????????????????????????????????????
    public void DestroyDummy()
    {
        Destroy(gameObject);
    }
    // ????????????????????????????????????????????????????????
}