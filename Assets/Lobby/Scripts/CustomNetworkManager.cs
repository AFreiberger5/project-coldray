using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;

public class CustomNetworkManager : NetworkManager
{
    public Text m_HostIP;
    public Text m_JoinIP;
    public Button m_JoinButton;

    private void Start()
    {
        m_HostIP.text = Network.player.ipAddress;// ??????????????????????????

        //m_HostIP.text = "127.0.0.1";
    }

    private void Update()
    {
        if (m_JoinIP.text.Length > 3)
        {
            m_JoinButton.interactable = true;
        }
        else
        {
            m_JoinButton.interactable = false;
        }
    }

    public void SoloOnClick()
    {
        networkAddress = "localhost";

        maxConnections = 1;

        StartHost();
    }

    public void HostOnClick()
    {
        networkAddress = m_HostIP.text;

        StartHost();
    }

    public void JoinOnClick()
    {
        string iP;
        if (CheckIP(m_JoinIP.text,out iP))
        {
            networkAddress = iP;
            StartClient();
        }
        else
        {
            Debug.Log("Nö");
        }
    }

    public override void OnServerConnect(NetworkConnection _conn)
    {
        base.OnServerConnect(_conn);

        print("Player ID: " + _conn.connectionId + "\n");
    }

    private bool CheckIP(string _ip, out string _ckecked)
    {
        System.Net.IPAddress iP;
        if (System.Net.IPAddress.TryParse(_ip, out iP))
        {
            //return iP.ToString();
            _ckecked = iP.ToString();
            return true;
        }
        else
        {
            //return "localhost";
            _ckecked = "localhost";
            return false;
        }
    }
}