using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeOnClick : MonoBehaviour
{
    public bool m_Solo;
    public bool m_Host;
    public bool m_Join;

    private void Start()
    {
        if (m_Solo
            &&
            !m_Host
            &&
            !m_Join)
        {
            gameObject.GetComponent<Button>().onClick.AddListener(FindObjectOfType<CustomNetworkManager>().SoloOnClick);
            //gameObject.GetComponent<Button>().onClick.AddListener(FindObjectOfType<CustomLobbyManager>().SoloLobbyAndGoOnClick);
        }
        else if (m_Host
            &&
            !m_Solo
            &&
            !m_Join)
        {
            gameObject.GetComponent<Button>().onClick.AddListener(FindObjectOfType<CustomNetworkManager>().HostOnClick);
            //gameObject.GetComponent<Button>().onClick.AddListener(FindObjectOfType<CustomLobbyManager>().OpenLobbyOnClick);
        }
        else if(m_Join
            &&
            !m_Solo
            &&
            !m_Host)
        {
            gameObject.GetComponent<Button>().onClick.AddListener(FindObjectOfType<CustomNetworkManager>().JoinOnClick);
            //gameObject.GetComponent<Button>().onClick.AddListener(FindObjectOfType<CustomLobbyManager>().JoinLobbyOnClick);
        }
    }
}