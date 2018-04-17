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

    public byte[] m_DummyModel = new byte[7];
    
    public bool Gender// Index: 0
    {
        get
        {
            return m_DummyModel[0] == 1;
        }
        set
        {
            m_DummyModel[0] = value ? (byte)0 : (byte)1;
        }
    }
    public byte SkinColor// Index: 1
    {
        get
        {
            return m_DummyModel[1];
        }
    }
    public byte Face// Index: 2
    {
        get
        {
            return m_DummyModel[2];
        }
    }
    public byte Ears// Index: 3
    {
        get
        {
            return m_DummyModel[3];
        }
    }
    public byte Eyes// Index: 4
    {
        get
        {
            return m_DummyModel[4];
        }
    }
    public byte Accessories// Index: 5
    {
        get
        {
            return m_DummyModel[5];
        }
    }
    public byte Hair// Index: 6
    {
        get
        {
            return m_DummyModel[6];
        }
    }

    private void Update()
    {
        return;
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
        m_DummyName = cs.m_Name;
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