using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class WorldManager : NetworkBehaviour
{
    static WorldManager INSTANCE;
    

    public static WorldManager GetInstance()
    {
        return INSTANCE;        
    }

    [SyncVar(hook = "OnChangeWorld")]
    public bool m_OverworldBuilt = false;

    [SyncVar(hook = "OnDungeonADone")]
    public bool m_DungeonADone = false;
    
    private Transform m_PortalA;
    private Transform m_PortalB;
    private bool m_newPortalB = false;
    private Vector3 m_WorldPosition = new Vector3(336, 0, 127);

    void Awake()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
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
        m_PortalA.GetComponent<PortalTeleporterA>().m_selfRegistered = true;
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
        m_PortalB.GetComponent<PortalTeleporterB>().m_selfRegistered = true;
    }

    public Transform GetPortalB()
    {
        return m_PortalB;
    }


    public override void OnStartClient()
    {
        base.OnStartClient();
        OnChangeWorld(m_OverworldBuilt);
        OnDungeonADone(m_DungeonADone);
    }

    public void ReportWorldBuilt(bool _b)
    {
        CmdSetWorldBool(_b);
    }

    [Command]
    void CmdSetWorldBool(bool _b)
    {
        m_OverworldBuilt = _b;
    }

    void OnChangeWorld(bool _b)
    {
        if (!isServer)
        {
            if (_b == true)
            {
                GameObject.FindObjectOfType<World>().StartBuild();
            }
        }
    }

    [Command]
    public void CmdBuildDungeonA()
    {
        GameObject.FindObjectOfType<DunGen>().PreGen();
    }

    void OnDungeonADone(bool _b)
    {
        if (!isServer)
        {
            if (_b)
            {
                GameObject.FindObjectOfType<DunGen>().Build();
            }
        }

    }
}
