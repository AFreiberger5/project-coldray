using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// used as information storage for the network manager
/// dontdestroyonload prevents the manager from storing the information on its own
/// </summary>
public class LobbyInfoFeed : MonoBehaviour
{
    public InputField m_GetHostIpInputField;
    public InputField m_GetJoinIpInputField;
}