using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    static GameStatus INSTANCE;
    private Vector3 m_WorldPosition = new Vector3 (575,0,734);
    public bool m_OverworldBuilt = false;
    public bool m_building = false;
    private Transform m_PortalA;
    private Transform m_PortalB;
    private bool m_newPortalB = false;

    // Use this for initialization


    void Start()
    {
        if (INSTANCE != null)
        {
            Destroy(this.gameObject);
            return;
        }
        INSTANCE = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    public static GameStatus GetInstance()
    {
        return INSTANCE;
    }

    public void BuildWorld(World _world)
    {
        if (!m_OverworldBuilt == !m_building)
        {
            _world.StartBuild();

        }
    }

    public Vector3 GetWorldPos()
    {
        return m_WorldPosition;
    }
    public Transform GetPortalA()
    {
        return m_PortalA;
    }

    public void SetPortalA(Transform _trf)
    {
        m_PortalA = _trf;
    }

    public bool GetNewPortalB()
    {
        return m_newPortalB;
    }

    public void SetNewPortalB(bool _b)
    {
        m_newPortalB = _b;
    }

    public void SetPortalB(Transform _trf)
    {
        m_PortalB = _trf;
    }

    public Transform GetPortalB()
    {
        return m_PortalB;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
