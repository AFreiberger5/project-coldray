using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    private GameObject m_Character;

    public void HostOnClick()
    {
        //m_Character = FindObjectOfType<Character>().gameObject;
        //NetworkIdentity ni = m_Character.AddComponent<NetworkIdentity>();
        //ni.localPlayerAuthority = true;
        //playerPrefab = CharReady();

        StartHost();

        //StartCoroutine(CharReady());
    }

    public void JoinOnClick()
    {
        m_Character = FindObjectOfType<Character>().gameObject;
        NetworkIdentity ni = m_Character.AddComponent<NetworkIdentity>();
        ni.localPlayerAuthority = true;
        //playerPrefab = CharReady();

        //StartClient();
    }

    private IEnumerator CharReady()
    {
        Debug.Log("lkusagfkuzw");
        GameObject g = FindObjectOfType<Character>().gameObject;
        NetworkIdentity ni = g.AddComponent<NetworkIdentity>();
        ni.localPlayerAuthority = true;
        spawnPrefabs.Add(g);
        playerPrefab = g;

        //yield return new WaitUntil(() => playerPrefab != null);
        yield return new WaitForSeconds(2f);

        StartHost();
    }
}