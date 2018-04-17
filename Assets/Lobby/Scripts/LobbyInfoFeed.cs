using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Used as information storage for the network manager
// Dontdestroyonload prevents the manager from storing the information on its own
public class LobbyInfoFeed : MonoBehaviour
{
    public InputField m_GetHostIp;
    public InputField m_GetJoinIp;
    public Button m_GetJoinButton;
}