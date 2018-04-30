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

    // these variables are only used in the offline scene
    private InputField m_HostIPInputField;
    private InputField m_JoinIPInputField;
    private bool m_gotTheInformation = false;

    /// <summary>
    /// procures much needed informations
    /// </summary>
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0// the offline scene is active
            &&
            (m_HostIPInputField == null// one of the input fields or buttons is missing
            ||
            m_JoinIPInputField == null))// ""
        {
            m_gotTheInformation = false;

            LobbyInfoFeed info = FindObjectOfType<LobbyInfoFeed>();// the lobby managers dontdestroyonload prevents it from keeping the information

            info = FindObjectOfType<LobbyInfoFeed>();// procures the information carrier
            m_HostIPInputField = info.m_GetHostIpInputField;// procures the required information
            m_JoinIPInputField = info.m_GetJoinIpInputField;// ""

            m_HostIPInputField.text = Network.player.ipAddress;// ??????????????????????????????????????????????? RIGHT IP ADRESS?????????????????????????

            m_dummy = FindObjectOfType<CharacterDummy>();// procures the current dummy

            m_gotTheInformation = true;
        }
    }

    /// <summary>
    /// hosts a game but limits the maximum player amount to one
    /// </summary>
    public void SoloOnClick()
    {
        m_dummy.DontDestroyDummyOnLoad();// allows the dummy to travel to the online scene

        networkAddress = "localhost";

        maxConnections = 1;

        StartHost();
    }

    /// <summary>
    /// hosts a game for a maximum of 4 active players
    /// </summary>
    public void HostOnClick()
    {
        m_dummy.DontDestroyDummyOnLoad();// allows the dummy to travel to the online scene

        networkAddress = "localhost";
        //networkAddress = m_HostIP.text;// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        StartHost();
    }

    /// <summary>
    /// lets the player join an active game
    /// </summary>
    public void JoinOnClick()
    {
        string ip;
        if (CheckIP(m_JoinIPInputField.text, out ip))
        {
            m_dummy.DontDestroyDummyOnLoad();// allows the dummy to travel to the online scene

            //networkAddress = ip;// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            networkAddress = "127.0.0.1";
            StartClient();
        }
        else
        {
            m_JoinIPInputField.text = "Invalid Ip";
        }
    }

    /// <summary>
    /// passes the network connection on to the player character
    /// </summary>
    /// <param the players network connection="_conn"></param>
    public override void OnServerConnect(NetworkConnection _conn)
    {
        base.OnServerConnect(_conn);

        print("Player ID: " + _conn.connectionId + "\n");
        playerPrefab.GetComponent<PlayerCharacter>().m_PlayerId = _conn.connectionId;// passes the network connection on to the player character
    }

    /// <summary>
    /// checks the ip address for possible violations
    /// </summary>
    /// <param ip adress="_ip"></param>
    /// <param checked ip address="_ckecked"></param>
    /// <returns></returns>
    private bool CheckIP(string _ip, out string _ckecked)
    {
        System.Net.IPAddress iP;
        if (System.Net.IPAddress.TryParse(_ip, out iP)
            &&
            _ip.Length >= 1)
        {
            _ckecked = iP.ToString();

            return true;
        }
        else
        {
            _ckecked = "Invalid IP";

            return false;
        }
    }
}