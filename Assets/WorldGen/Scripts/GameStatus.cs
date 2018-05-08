using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameStatus : NetworkBehaviour
{
    static GameStatus INSTANCE;
    public GameObject m_EntryRoom;
    public Dictionary<Vector3, int> m_TopTilesA = new Dictionary<Vector3, int>();
    public Dictionary<Vector3, int> m_BotTilesA = new Dictionary<Vector3, int>();
    public Dictionary<Vector3, int> m_LeftTilesA = new Dictionary<Vector3, int>();
    public Dictionary<Vector3, int> m_RightTilesA = new Dictionary<Vector3, int>();
    public List<Vector3> m_DeadendsA = new List<Vector3>();
    public bool m_buildDungeon = true;

    [SyncVar(hook = "OnChangeWorld")]
    public bool m_OverworldBuilt = false;

    [SyncVar(hook = "OnDungeonAChange")]
    public bool m_DungeonADone = false;

    public bool m_building = false;
    private RoomTemplates m_templates;
    private Transform m_PortalA;
    private Transform m_PortalB;
    private bool m_newPortalB = false;
    private Vector3 m_WorldPosition = new Vector3(575, 0, 734);
    private World m_overworld;


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

    public static GameStatus GetInstance()
    {
        if (INSTANCE == null)
        {
            INSTANCE = GameObject.FindObjectOfType<GameStatus>();
        }
        return INSTANCE;
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

    public void SetTemplates(RoomTemplates _rt)
    {
        m_templates = _rt;
        m_templates.m_SelfRegistered = true;
    }

    public RoomTemplates GetTemplates()
    {
        return m_templates;
    }
    // Update is called once per frame
    void Update()
    {

        if (m_buildDungeon == true && m_OverworldBuilt == true)
            if (GameObject.FindObjectOfType<Destroyer>() == null)
            {
                m_buildDungeon = false;
                m_DungeonADone = true;
            }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        OnChangeWorld(m_OverworldBuilt);
        OnDungeonAChange(m_DungeonADone);
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

    void OnDungeonAChange(bool _b)
    {

        if (!isServer)
        {
            if (_b == true)
            {
                BuildDungeonA();
            }
        }

    }

    [Command]
    public void CmdBuildDungeonA()
    {
        Instantiate(m_EntryRoom, new Vector3(0, 100, 0), m_EntryRoom.transform.rotation);

    }


    private void BuildDungeonA()
    {
        Instantiate(m_EntryRoom, new Vector3(0, 100, 0), m_EntryRoom.transform.rotation);

        foreach (KeyValuePair<Vector3, int> c in m_TopTilesA)
        {
            Instantiate(m_templates.m_TopRooms[c.Value], c.Key, m_templates.m_TopRooms[c.Value].transform.rotation);
        }
        foreach (KeyValuePair<Vector3, int> c in m_BotTilesA)
        {
            Instantiate(m_templates.m_BottomRooms[c.Value], c.Key, m_templates.m_BottomRooms[c.Value].transform.rotation);
        }
        foreach (KeyValuePair<Vector3, int> c in m_LeftTilesA)
        {
            Instantiate(m_templates.m_LeftRooms[c.Value], c.Key, m_templates.m_LeftRooms[c.Value].transform.rotation);
        }
        foreach (KeyValuePair<Vector3, int> c in m_RightTilesA)
        {
            Instantiate(m_templates.m_RightRooms[c.Value], c.Key, m_templates.m_RightRooms[c.Value].transform.rotation);
        }
        foreach (Vector3 t in m_DeadendsA)
        {
            Instantiate(m_templates.m_ClosedRoom, t, m_templates.m_ClosedRoom.transform.rotation);
        }
    }
}
