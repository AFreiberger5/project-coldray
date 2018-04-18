using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCharacter : NetworkBehaviour
{
    [SyncVar]
    public string m_PlayerName = "";
    [SyncVar]
    public int m_PlayerId = 42;

    public byte[] m_PlayerModel = new byte[7];

    [ClientCallback]
    private void Update()
    {
        if (isLocalPlayer)
        {
            if (m_PlayerName == "")
            {
                print("1: " + m_PlayerName);
                CharacterDummy dummy = FindObjectOfType<CharacterDummy>();
                print("2: " + m_PlayerName);
                CmdNetworkInitialize(dummy.m_DummyName);

                print("try to load: " + m_PlayerName);

                //FindObjectOfType<PlayerStatsOnServer>().m_ServerPlayerNames[m_PlayerId] = dummy.m_DummyName;
                //NetworkServer.FindLocalObject(new NetworkInstanceId(1)).GetComponent<PlayerStatsOnServer>().m_ServerPlayerNames[m_PlayerId] = m_PlayerName;
            }
        }
    }

    [Command]
    public void CmdNetworkInitialize(string _string)
    {
        Debug.developerConsoleVisible = true;
        m_PlayerName = _string;

        RpcSetGameObjectName();
    }

    [ClientRpc]
    public void RpcSetGameObjectName()
    {
        gameObject.name = m_PlayerName;
    }

    public void LoadCharacter(string _characterName)
    {
        string selectedCharacter = _characterName;
        CharacterStats cs = SaveLoadManager.LoadCharacter(selectedCharacter);
        m_PlayerName = cs.m_Name;
        //m_PlayerModel = cs.m_Model;

        // BUILD DUMMY HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }
}