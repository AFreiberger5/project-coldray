using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCharacter : NetworkBehaviour
{
    [SyncVar(hook = "OnChangeName")]
    public string m_PlayerName = "";
    [SyncVar]
    public int m_PlayerId = 42;

    private void OnChangeName(string _s)
    {
        m_PlayerName = _s;
        SINGLETOOOOON.Instance.RegPlayer(m_PlayerId, m_PlayerName);
    }




    //[SyncVar]
    //public string m_PlayerName = "";
    //[SyncVar]
    //public int m_PlayerId = 42;

    public int[] m_PlayerModel = new int[7]
    {
        0,
        0,
        0,
        0,
        0,
        0,
        0
    };

    public bool Gender// Index: 0
    {
        get
        {
            return m_PlayerModel[0] == 1;
        }
        set
        {
            m_PlayerModel[0] = value ? 0 : 1;
        }
    }
    public int SkinColor// Index: 1
    {
        get
        {
            return m_PlayerModel[1];
        }
    }
    public int Face// Index: 2
    {
        get
        {
            return m_PlayerModel[2];
        }
    }
    public int Ears// Index: 3
    {
        get
        {
            return m_PlayerModel[3];
        }
    }
    public int Eyes// Index: 4
    {
        get
        {
            return m_PlayerModel[4];
        }
    }
    public int Accessories// Index: 5
    {
        get
        {
            return m_PlayerModel[5];
        }
    }
    public int Hair// Index: 6
    {
        get
        {
            return m_PlayerModel[6];
        }
    }

    //[ClientCallback]
    private void Update()
    {
        if (isLocalPlayer)
        {
            if (m_PlayerName == "")
            {
                CharacterDummy dummy = FindObjectOfType<CharacterDummy>();
                CmdNetworkInitialize(dummy.m_DummyName);

                print("try to load: " + m_PlayerName);

                //SINGLETOOOOON.Instance.RegisterPlayerInArray(m_PlayerId, m_PlayerName);
            }
        }
    }

    [Command]
    public void CmdNetworkInitialize(string _string)
    {
        Debug.developerConsoleVisible = true;
        m_PlayerName = _string;
    }

    // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- NEU

    //public void LoadCharacter(string _characterName)
    //{
    //    string selectedCharacter = _characterName;
    //    CharacterStats cs = SaveLoadManager.LoadCharacter(selectedCharacter);
    //    m_PlayerName = cs.m_StatsName;
    //    //m_PlayerModel = cs.m_Model;
    //
    //    // BUILD CHARACTER HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //}

    //public string GetPrefabPath(int _id)
    //{
    //    string path = "Prefabs/";
    //
    //    if (m_PlayerModel[0] == 0)// Male
    //        path += "m";
    //    else// Female
    //        path += "f";
    //
    //    int index;
    //    int.TryParse(_id.ToString().Substring(1, 4), out index);
    //
    //    int type;
    //    int.TryParse(_id.ToString().Substring(0, 1), out type);
    //
    //    if (index < 1000)
    //    {
    //        // Body
    //        switch (type)
    //        {
    //            case 1:
    //                m_TEST_TYPE = "Color";
    //                Debug.Log("Color");
    //                break;
    //            case 2:
    //                m_TEST_TYPE = "Face";
    //                Debug.Log("Face");
    //                break;
    //            case 3:
    //                m_TEST_TYPE = "Ears";
    //                Debug.Log("Ears");
    //                break;
    //            case 4:
    //                m_TEST_TYPE = "Eyes";
    //                Debug.Log("Eyes");
    //                break;
    //            case 5:
    //                m_TEST_TYPE = "Accessories";
    //                Debug.Log("Accessories");
    //                break;
    //            case 6:
    //                m_TEST_TYPE = "Hair";
    //                Debug.Log("Hair");
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    else if (index >= 1000)
    //    {
    //        // Gear
    //        switch (type)
    //        {
    //            case 0:
    //                m_TEST_TYPE = "Head";
    //                Debug.Log("Head");
    //                break;
    //            case 1:
    //                m_TEST_TYPE = "Chest";
    //                Debug.Log("Chest");
    //                break;
    //            case 2:
    //                m_TEST_TYPE = "Legs";
    //                Debug.Log("Legs");
    //                break;
    //            case 3:
    //                m_TEST_TYPE = "Hands";
    //                Debug.Log("Hands");
    //                break;
    //            case 4:
    //                m_TEST_TYPE = "Feet";
    //                Debug.Log("Feet");
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    // TEST
    //    m_TEST_ITEM_INDEX = index;
    //
    //
    //    path += _id.ToString();
    //    //path += ".prefab";
    //
    //    Debug.Log(path);
    //
    //    return path;
    //}
}