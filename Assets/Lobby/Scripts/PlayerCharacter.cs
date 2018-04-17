using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCharacter : NetworkBehaviour
{
    public string m_PlayerName = "";
    public int m_PlayerId = 42;
    //public string m_PlayerNetId = "42";
    //public byte[] m_PlayerModel = new byte[7];

    [ClientCallback]
    private void Start()
    {
        if (isLocalPlayer)
        {
            while (m_PlayerName == "")
            {
                CharacterDummy dummy = FindObjectOfType<CharacterDummy>();
                CmdNetworkInitialize(dummy.m_DummyName);
                // Destroy / make dummy invisible
                //Destroy(dummy.gameObject);
            }
        }
    }

    [Command]
    public void CmdNetworkInitialize(string _string)
    {
        Debug.developerConsoleVisible = true;
        //print("NetId: " + netId + ", Name: " + _string);
        m_PlayerName = _string;
        //m_PlayerNetId = netId.ToString();
        gameObject.name = _string;
        //CmdLoadCharacter(_string);
    }

    [Command]
    public void CmdLoadCharacter(string _characterName)
    {
        string selectedCharacter = _characterName;
        CharacterStats cs = SaveLoadManager.LoadCharacter(selectedCharacter);
        m_PlayerName = cs.m_Name;
        //m_PlayerModel = cs.m_Model;

        // BUILD DUMMY HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }
}