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

    [SyncVar]
    public Vector3 m_WorldPosition = new Vector3(336, 0, 127);

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


    public bool m_IsDestroyingWorld = false;
    public bool m_IsDestroyingDungeonA = false;
    private Transform m_PortalA;
    private Transform m_PortalB;
    private PortalTeleporterA m_overworldTeleporter;
    private bool m_newPortalB = false;

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

    public override void OnStartClient()
    {
        base.OnStartClient();
        OnBuildWorldNow(m_BuildWorldNow);
        OnPropsListDone(m_PropsListDone);
        OnDungeonADone(m_DungeonADone);
    }


    [ClientRpc]
    void RpcDestroyWorld()
    {
        if (!isServer)
        {
            StartCoroutine(FindObjectOfType<DunGen>().DestroyDungeon());
            StartCoroutine(FindObjectOfType<World>().DestroyWorld());
        }
    }

    #region Getter

    public Vector3 GetWorldPos()
    {
        return m_WorldPosition;
    }

    public Transform GetPortalA()
    {
        return m_PortalA;

    }

    public bool GetNewPortalB()
    {
        return m_newPortalB;
    }

    public Transform GetPortalB()
    {
        return m_PortalB;
    }
    #endregion

    #region Setter

    public void SetPortalA(Transform _trf)
    {
        m_PortalA = _trf;
        PortalTeleporterA tmp = m_PortalA.GetComponent<PortalTeleporterA>();
        m_overworldTeleporter = tmp;
        tmp.m_selfRegistered = true;
        tmp.gameObject.SetActive(false);
    }

    public void SetPortalAActive(bool _b)
    {
        m_overworldTeleporter.gameObject.SetActive(_b);
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

    #endregion

    #region Command-Wrapper for Monobehaviours


    public void CallBuildDungeon()
    {
        CmdBuildDungeonA();
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

    public void ReportDungeonADone(bool _b)
    {
        CmdSetDungeonADone(_b);
    }

    public void ReportPropsListDone(bool _b)
    {
        CmdSetPropsListDone(_b);

    }
    #endregion

    #region Commands
    [Command]
    void CmdSetWorldPos()
    {
        int x = Random.Range(-3000, 3000);
        if (x > -100 && x < 100)
        {
            x += 200;
        }
        int z = Random.Range(-3000, 3000);
        if (z > -100 && z < 100)
        {
            z -= 200;
        }
        int y = 0;
        m_WorldPosition = new Vector3(x,y,z);
    }

    [Command]
    public void CmdStartNewWorld()
    {
        if(m_IsDestroyingDungeonA || m_IsDestroyingWorld)
        {
            return;
        }
        CmdSetWorldPos();
        FindObjectOfType<World>().StartBuild();
    }

    [Command]
    public void CmdSetPropsDone(bool _b)
    {
        m_PropsDone = _b;
    }

    [Command]
    void CmdSetDungeonADone(bool _b)
    {
        m_DungeonADone = _b;
    }

    [Command]
    void CmdSetPropsListDone(bool _b)
    {
        m_PropsListDone = _b;
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

    [Command]
    public void CmdBuildDungeonA()
    {
        GameObject.FindObjectOfType<DunGen>().PreGen();
    }

    [Command]
    public void CmdDestroyWorld()
    {
        m_IsDestroyingDungeonA = true;
        m_IsDestroyingWorld = true;
        RpcDestroyWorld();
        StartCoroutine(FindObjectOfType<DunGen>().DestroyDungeon());
        StartCoroutine(FindObjectOfType<World>().DestroyWorld());
    }

    #endregion

    #region Hooks

    public void OnPropsDone(bool _b)
    {
        if (_b == true)
        {
            if (isServer)
            {
                GameObject.FindObjectOfType<World>().CmdPopulateSyncList();
            }
        }
    }

    public void OnPropsListDone(bool _b)
    {
        if (_b == true)
        {
            StartCoroutine(GameObject.FindObjectOfType<World>().InstantiateProps());
        }
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

    void OnBuildWorldNow(bool _b)
    {
        if (!isServer)
        {
            if (_b == true)
            {
                GameObject.FindObjectOfType<World>().StartBuild();
            }
        }
    }

    void OnDungeonADone(bool _b)
    {
        if (!isServer)
        {
            if (_b == true)
            {
                GameObject.FindObjectOfType<DunGen>().Build();
            }
        }

    }

    #endregion

}
