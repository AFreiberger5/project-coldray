using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SINGLETOOOOON : MonoBehaviour
{
    //static SINGLETOOOOON INSTANCE;
    //
    //public string[] m_PlayerArray = new string[4];
    //
    //private void Start()
    //{
    //    if (INSTANCE != null)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    INSTANCE = this;
    //    DontDestroyOnLoad(gameObject);
    //}
    //
    //public static SINGLETOOOOON GetInstance()
    //{
    //    return INSTANCE;
    //}
    //
    //public string[] GetPlayerArray()
    //{
    //    return m_PlayerArray;
    //}
    //
    //public void RegisterPlayerInArray(int _index, string _playerName)
    //{
    //    m_PlayerArray[_index] = _playerName;
    //}

    static SINGLETOOOOON INSTANCE;
    public string[] m_allPlayers = new string[4];
    public static SINGLETOOOOON Instance
    {
        get
        {
            if (INSTANCE == null)
            {
                INSTANCE = FindObjectOfType<SINGLETOOOOON>();
                if (INSTANCE == null)
                {
                    INSTANCE = new GameObject().AddComponent<SINGLETOOOOON>();
                }
            }
            return INSTANCE;
        }
    }

    private void Awake()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string[] GetAllPlayers()
    {
        return m_allPlayers;
    }

    public void RegPlayer(int _id, string _playerName)
    {
        m_allPlayers[_id] = _playerName;
    }
}