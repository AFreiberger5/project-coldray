using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStatsOnServer : NetworkBehaviour
{
    public string[] m_ServerPlayerNames = new string[4];
}