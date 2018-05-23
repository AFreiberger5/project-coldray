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

    [SyncVar(hook = "OnBuildWorldNow")]
    public bool m_BuildWorldNow = false;

    [SyncVar(hook = "OnWorldDone")]
    public bool m_OverworldBuilt = false;

    [SyncVar(hook = "OnDungeonADone")]
    public bool m_DungeonADone = false;

    [SyncVar(hook = "OnPropsDone")]
    public bool m_PropsDone = false;

    [SyncVar(hook = "OnPropsListDone")]
    public bool m_PropsListDone = false;


    private Transform m_PortalA;
    private Transform m_PortalB;
    private PortalTeleporterA m_overworldTeleporter;
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
        PortalTeleporterA tmp = m_PortalA.GetComponent<PortalTeleporterA>();
        m_overworldTeleporter = tmp;
        tmp.m_selfRegistered = true;
        tmp.gameObject.SetActive(false);
    }

    public void ActivatePortalA()
    {
        m_overworldTeleporter.gameObject.SetActive(true);
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
        OnDungeonADone(m_DungeonADone);
    }

    public void ReportBuildWorldNow(bool _b)
    {
        CmdSetBuildWorldNow(_b);
    }

    public void ReportWorldBuilt(bool _b)
    {
        CmdSetWorldDone(_b);
    }

    public void ReportPropsDone(bool _b)
    {
        CmdSetPropsDone(_b);
    }

    [Command]
    public void CmdSetPropsDone(bool _b)
    {
        m_PropsDone = _b;
    }

    public void OnPropsDone(bool _b)
    {
        if (isServer)
        {
            GameObject.FindObjectOfType<World>().CmdPopulateSyncList();
        }
    }

    public void ReportPropsListDone(bool _b)
    {
        CmdSetPropsListDone(_b);

    }

    [Command]
    void CmdSetPropsListDone(bool _b)
    {
        m_PropsListDone = _b;
    }

    public void OnPropsListDone(bool _b)
    {
        if (_b == true)
        {
            StartCoroutine(GameObject.FindObjectOfType<World>().InstantiateProps());
        }
    }

    [Command]
    public void CmdSetBuildWorldNow(bool _b)
    {
        m_BuildWorldNow = _b;
    }

    [Command]
    void CmdSetWorldDone(bool _b)
    {
        m_OverworldBuilt = _b;
    }

    void OnWorldDone(bool _b)
    {
        if (isServer)
        {
            if (_b == true)
            {
                GameObject.FindObjectOfType<World>().PropSeedController();
            }
        }
    }

    [Command]
    public void CmdBuildDungeonA()
    {
        GameObject.FindObjectOfType<DunGen>().PreGen();
    }

    void OnBuildWorldNow(bool _b)
    {
        if (!isServer)
        {
            if (_b)
            {
                GameObject.FindObjectOfType<World>().StartBuild();
            }
        }
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
