using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    private CharacterDummy m_dummy;

    // Lobby scene use only
    private InputField m_HostIP;
    private InputField m_JoinIP;
    private Button m_JoinButton;

    private bool m_gotIt = false;

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0
            &&
            (m_HostIP == null
            ||
            m_JoinIP == null
            ||
            m_JoinButton == null))
        {
            m_gotIt = false;

            LobbyInfoFeed info = FindObjectOfType<LobbyInfoFeed>();

            info = FindObjectOfType<LobbyInfoFeed>();
            m_HostIP = info.m_GetHostIp;
            m_JoinIP = info.m_GetJoinIp;
            m_JoinButton = info.m_GetJoinButton;

            m_HostIP.text = Network.player.ipAddress;// ??????????????????????????

            m_dummy = FindObjectOfType<CharacterDummy>();

            m_gotIt = true;
        }

        if (m_gotIt)
        {
            if (m_JoinIP.text.Length >= 1)
            {
                m_JoinButton.interactable = true;
            }
            else
            {
                m_JoinButton.interactable = false;
            }
        }
    }

    public void SoloOnClick()
    {
        m_dummy.DontDestroyDummyOnLoad();

        networkAddress = "localhost";

        maxConnections = 1;

        StartHost();
    }

    public void HostOnClick()
    {
        m_dummy.DontDestroyDummyOnLoad();

        networkAddress = "localhost";
        //networkAddress = m_HostIP.text;

        StartHost();
    }

    public void JoinOnClick()
    {
        string iP;
        if (CheckIP(m_JoinIP.text, out iP))
        {
            m_dummy.DontDestroyDummyOnLoad();

            //networkAddress = iP;// !!!!!!!!!!!!!!!!!!!!!!!!!!!
            networkAddress = "127.0.0.1";
            StartClient();
        }
        else
        {
            m_JoinIP.text = "Invalid Ip";
        }
    }

    public override void OnServerConnect(NetworkConnection _conn)
    {
        base.OnServerConnect(_conn);

        print("Player ID: " + _conn.connectionId + "\n");
        playerPrefab.GetComponent<PlayerCharacter>().m_PlayerId = _conn.connectionId;// Player Prefab gets its connectionId
    }

    private bool CheckIP(string _ip, out string _ckecked)
    {
        System.Net.IPAddress iP;
        if (System.Net.IPAddress.TryParse(_ip, out iP))
        {
            _ckecked = iP.ToString();
            return true;
        }
        else
        {
            _ckecked = "ERROR";
            return false;
        }
    }
}