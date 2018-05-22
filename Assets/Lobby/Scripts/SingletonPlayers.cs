using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//||||||||||||||||||||||||||||||||||||||||||||||||||||\\
//||                                                ||\\
//||            Script by Gregor Hempel             ||\\
//||            23.03.2018                          ||\\
//||            Edits:                              ||\\
//||                                                ||\\
//||||||||||||||||||||||||||||||||||||||||||||||||||||\\

public class SingletonPlayers : MonoBehaviour
{
    static SingletonPlayers INSTANCE;
    public string[] m_allPlayers = new string[4];
    public static SingletonPlayers Instance
    {
        get
        {
            if (INSTANCE == null)
            {
                INSTANCE = FindObjectOfType<SingletonPlayers>();
                if (INSTANCE == null)
                {
                    INSTANCE = new GameObject().AddComponent<SingletonPlayers>();
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

    public void RegPlayer(int _id, string _playerName)
    {
        m_allPlayers[_id] = _playerName;
    }
}