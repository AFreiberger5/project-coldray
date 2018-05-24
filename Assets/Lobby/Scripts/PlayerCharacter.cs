using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

//||||||||||||||||||||||||||||||||||||||||||||||||||||\\
//||                                                ||\\
//||            Script by Gregor Hempel             ||\\
//||            23.03.2018                          ||\\
//||            Edits:                              ||\\
//||                                                ||\\
//||||||||||||||||||||||||||||||||||||||||||||||||||||\\

public class PlayerCharacter : NetworkBehaviour
{
    #region Player Anchors

    [Header("Body Part Anchors")]
    public Transform m_PlayerAnchorHead;
    public Transform m_PlayerAnchorBody;
    public Transform m_PlayerAnchorLArm1;
    public Transform m_PlayerAnchorLArm2;
    public Transform m_PlayerAnchorLArm3;
    public Transform m_PlayerAnchorRArm1;
    public Transform m_PlayerAnchorRArm2;
    public Transform m_PlayerAnchorRArm3;
    public Transform m_PlayerAnchorLLeg1;
    public Transform m_PlayerAnchorLLeg2;
    public Transform m_PlayerAnchorLLeg3;
    public Transform m_PlayerAnchorRLeg1;
    public Transform m_PlayerAnchorRLeg2;
    public Transform m_PlayerAnchorRLeg3;

    #endregion

    [Header("Player Sync Vars")]
    [SyncVar(hook = "OnChangeName")]
    public string m_PlayerName = "";
    [SyncVar]
    public int m_PlayerId = 42;// default, regular ids would be 0,1 ...

    public SyncListInt m_SyncModel = new SyncListInt();

    #region Player Hooks

    public override void OnStartClient()
    {
        base.OnStartClient();
        OnChangeName(m_PlayerName);

        m_SyncModel.Callback -= OnIntChanged;
        m_SyncModel.Callback += OnIntChanged;
    }

    private void OnChangeName(string _s)
    {
        m_PlayerName = _s;
        SingletonPlayers.Instance.RegPlayer(m_PlayerId, m_PlayerName);
    }

    private void OnIntChanged(SyncListInt.Operation op, int index)
    {
        //print("id: " + m_PlayerId + ", sync model count: " + m_SyncModel.Count);

        if (m_SyncModel.Count == 9)
        {
            BuildEntirePlayer(m_SyncModel);
        }
    }

    #endregion

    #region Player Properties

    public bool Gender// index: 0
    {
        get
        {
            return m_SyncModel[0] == 1;
        }
        set
        {
            m_SyncModel[0] = value ? 0 : 1;
        }
    }
    public int SkinColor// index: 1
    {
        get
        {
            return m_SyncModel[1];
        }
    }
    public int Face// index: 2
    {
        get
        {
            return m_SyncModel[2];
        }
    }
    public int Ears// index: 3
    {
        get
        {
            return m_SyncModel[3];
        }
    }
    public int Eyes// index: 4
    {
        get
        {
            return m_SyncModel[4];
        }
    }
    public int Accessories// index: 5
    {
        get
        {
            return m_SyncModel[5];
        }
    }
    public int Hair// index: 6
    {
        get
        {
            return m_SyncModel[6];
        }
    }
    public int HairColor// index 7
    {
        get
        {
            return m_SyncModel[7];
        }
    }
    public int EyeColor// index 8
    {
        get
        {
            return m_SyncModel[8];
        }
    }

    #endregion

    private GameObject[] m_PlayerCustomisationObjects = new GameObject[8];
    private GameObject[] m_PlayerBody = new GameObject[13];
#pragma warning disable 0414
    private Animator m_PlayerAnimator = null;
#pragma warning restore 0414

    private Inventory m_playerInventoryScript;

    /// <summary>
    /// converts an int array into an int sync list
    /// </summary>
    /// <param name="_intA"></param>
    private void MakeItASyncListInt(int[] _intA)
    {
        if (m_SyncModel.Count < 9)
            _intA.ToList().ForEach(o => m_SyncModel.Add(o));
    }

    private void Start()
    {
        m_PlayerAnimator = GetComponent<Animator>();

        m_playerInventoryScript = GetComponent<Inventory>();

        if (m_SyncModel.Count >= 9)
        {
            BuildEntirePlayer(m_SyncModel);
        }
    }

    /// <summary>
    /// builds the player as soon as he got his name from the dummy
    /// </summary>
    private void Update()
    {
        if (isLocalPlayer)
        {
            if (m_PlayerName == "")// gets called when the player spawns for the first time
            {
                CharacterDummy dummy = FindObjectOfType<CharacterDummy>();
                CmdNetworkInitialize(dummy.m_DummyName);
                m_PlayerName = dummy.m_DummyName;

                Destroy(dummy.gameObject);

                //print("try to load: " + m_PlayerName);

                LoadCharacter(m_PlayerName);

                CmdSendArrayData(m_PlayerId, m_SyncModel.ToArray());

                BuildEntirePlayer(m_SyncModel);
            }
        }
    }

    /// <summary>
    /// extracts the players name from the dummy
    /// </summary>
    /// <param name="_string"></param>
    [Command]
    public void CmdNetworkInitialize(string _string)
    {
        Debug.developerConsoleVisible = true;
        m_PlayerName = _string;
    }

    /// <summary>
    /// loads the player file by name and adjusts the players stats according to it
    /// </summary>
    /// <param name="_characterName"></param>
    public void LoadCharacter(string _characterName)
    {
        if (isLocalPlayer)
        {
            CharacterStats cs = SaveLoadManager.LoadCharacter(_characterName);
            m_PlayerName = cs.m_StatsName;
            MakeItASyncListInt(cs.m_StatsModel);

            //---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW
            GetComponent<PlayerController>().m_PlayerCurrentHP = cs.m_StatsCurrentHP;
            GetComponent<PlayerController>().m_PlayerInventory = cs.m_StatsInventory;

            m_playerInventoryScript.Deserialize(GetComponent<PlayerController>().m_PlayerInventory, m_playerInventoryScript.m_GridPanel, GameObject.Find("ItemManager").GetComponent<ItemManager>());
            m_playerInventoryScript.BuildInventory(m_playerInventoryScript.m_GridPanel);
            
            Debug.Log(m_playerInventoryScript.MakeSerializible(m_playerInventoryScript.m_GridPanel));
            //---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW---NEW

            gameObject.name = cs.m_StatsName;
        }
    }

    public void SavePlayerCharacter()
    {
        CharacterStats cs = new CharacterStats(m_PlayerName, m_SyncModel.ToArray(), GetComponent<PlayerController>().m_PlayerCurrentHP, m_playerInventoryScript.MakeSerializible(m_playerInventoryScript.m_GridPanel));
        Debug.Log(m_playerInventoryScript.MakeSerializible(m_playerInventoryScript.m_GridPanel));

        SaveLoadManager.SaveCharacter(cs);

        Debug.Log("saved inventory");
    }

    /// <summary>
    /// gives the server information about the player
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_model"></param>
    [Command]
    private void CmdSendArrayData(int _id, int[] _model)
    {
        //print("player name: " + m_PlayerName + ", player id: " + m_PlayerId + ", sync model count: " + _model.Length);

        if (_id == m_PlayerId)
        {
            MakeItASyncListInt(_model);
        }
    }

    /// <summary>
    /// calls every build function to build the entire player
    /// </summary>
    /// <param name="_syncModel"></param>
    private void BuildEntirePlayer(SyncListInt _syncModel)
    {
        try
        {
            BuildPlayerBody(_syncModel);
            BuildEntirePlayerCustomisation(_syncModel);
            PaintPlayerBody(_syncModel);
            PaintPlayerCustomisation(_syncModel);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// determines the players gender and builds the body according to it
    /// </summary>
    public void BuildPlayerBody(SyncListInt _syncModel)
    {
        string bodyPath = "Prefabs/";

        if (_syncModel[0] == 0)// male
        {
            bodyPath += "M_Humanoid/Body/BodyI0";

            //--------- CHANGE ANIMATOR HERE ---------
            // gets the male animator
            //m_PlayerAnimator.runtimeAnimatorController =
            //    Resources.Load("Prefabs/M_Humanoid/Female_Human",
            //    typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
            //
            //m_PlayerAnimator.Play("Idle");
            //--------- CHANGE ANIMATOR HERE ---------
        }
        else// female
        {
            bodyPath += "F_Humanoid/Body/BodyI0";

            //--------- CHANGE ANIMATOR HERE ---------
            // gets the female animator
            //m_PlayerAnimator.runtimeAnimatorController =
            //    Resources.Load("Prefabs/F_Humanoid/Female_Human",
            //    typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
            //
            //m_PlayerAnimator.Play("Idle");
            //--------- CHANGE ANIMATOR HERE ---------
        }

        // iterates the entire body and instantiates bodyparts according to the gender
        for (int i = 0; i < m_PlayerBody.Length; i++)
        {
            // deletes present bodyparts
            if (m_PlayerBody[i] != null)
            {
                Destroy(m_PlayerBody[i]);
                m_PlayerBody[i] = null;
            }

            // adds array index information to the path
            string[] splitter = bodyPath.Split('I');
            splitter[1] = i.ToString();
            bodyPath = splitter[0] + "I" + splitter[1];

            // instantiates the body parts at the right place
            switch (i)
            {
                case 0:// torso
                    InstantiatePlayerBodyPartWithMesh(i, bodyPath, m_PlayerAnchorBody);
                    break;
                case 1:// left arm anchor 1
                    InstantiatePlayerBodyPartWithMesh(i, bodyPath, m_PlayerAnchorLArm1);
                    break;
                case 2:// left arm anchor 2
                    InstantiatePlayerBodyPartWithMesh(i, bodyPath, m_PlayerAnchorLArm2);
                    break;
                case 3:// left arm anchor 3
                    InstantiatePlayerBodyPartWithMesh(i, bodyPath, m_PlayerAnchorLArm3);
                    break;
                case 4:// right arm anchor 1
                    InstantiatePlayerBodyPartWithMesh(i, bodyPath, m_PlayerAnchorRArm1);
                    break;
                case 5:// right arm anchor 2
                    InstantiatePlayerBodyPartWithMesh(i, bodyPath, m_PlayerAnchorRArm2);
                    break;
                case 6:// right arm anchor 3
                    InstantiatePlayerBodyPartWithMesh(i, bodyPath, m_PlayerAnchorRArm3);
                    break;
                case 7:// left leg anchor 1
                    InstantiatePlayerBodyPartWithMesh(i, bodyPath, m_PlayerAnchorLLeg1);
                    break;
                case 8:// left leg anchor 2
                    InstantiatePlayerBodyPartWithMesh(i, bodyPath, m_PlayerAnchorLLeg2);
                    break;
                case 9:// left leg anchor 3
                    InstantiatePlayerBodyPartWithMesh(i, bodyPath, m_PlayerAnchorLLeg3);
                    break;
                case 10:// right leg anchor 1
                    InstantiatePlayerBodyPartWithMesh(i, bodyPath, m_PlayerAnchorRLeg1);
                    break;
                case 11:// right leg anchor 2
                    InstantiatePlayerBodyPartWithMesh(i, bodyPath, m_PlayerAnchorRLeg2);
                    break;
                case 12:// right leg anchor 3
                    InstantiatePlayerBodyPartWithMesh(i, bodyPath, m_PlayerAnchorRLeg3);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// instantiates a gameobject with no mesh
    /// loads a mesh by its path and transferres it into the gameobject
    /// </summary>
    /// <param iterator="_i"></param>
    /// <param folder path="_path"></param>
    /// <param position anchor="_anchor"></param>
    private void InstantiatePlayerBodyPartWithMesh(int _i, string _path, Transform _anchor)
    {
        m_PlayerBody[_i] = Instantiate(Resources.Load("Prefabs/Meshless", typeof(GameObject)) as GameObject);
        m_PlayerBody[_i].GetComponent<MeshFilter>().mesh = Instantiate(Resources.Load(_path, typeof(Mesh)) as Mesh);
        m_PlayerBody[_i].name = "BodyI" + _i;
        m_PlayerBody[_i].transform.SetParent(_anchor);
        m_PlayerBody[_i].transform.localPosition = Vector3.zero;
        m_PlayerBody[_i].transform.localRotation = Quaternion.identity;

        if (m_SyncModel[0] == 0)// male
            ScaleFixer(m_PlayerBody[_i]);// // male body parts need to be scaled due to a 3dmax error
    }

    /// <summary>
    /// calls the build function on every value of the customisation array
    /// </summary>
    public void BuildEntirePlayerCustomisation(SyncListInt _syncModel)
    {
        for (int i = 1; i < m_SyncModel.ToArray().Length; i++)// excludes gender
        {
            BuildPlayerCustomisation(_syncModel[i], _syncModel);
        }
    }

    /// <summary>
    /// takes in an id and determines its assignment
    /// deletes pre existing gameobjects and instantiates the new one
    /// builds a folder path based on the gender of the player and the accepted id
    /// loads the asset by path and instantiates it on the according position
    /// </summary>
    /// <param asset id="_id"></param>
    public void BuildPlayerCustomisation(int _id, SyncListInt _syncModel)
    {
        string path = "Prefabs/";
        if (_syncModel[0] == 0)
            path += "M_Humanoid/";// male path
        else
            path += "F_Humanoid/";// female path

        int bodyPartIndex = int.Parse(_id.ToString().Substring(0, 1));
        int identifier = int.Parse(_id.ToString().Substring(1, 4));

        if (identifier < 1000)// >= 1000 would be gear instead of a customisation asset
        {
            if (bodyPartIndex == 0 || bodyPartIndex == 1 || bodyPartIndex == 7 || bodyPartIndex == 8)// excludes gender and colors
                return;

            if (bodyPartIndex >= 2 && bodyPartIndex <= 6)// head, ears, eyes, accessories, hair
            {
                // deletes pre existing customisation
                if (m_PlayerCustomisationObjects[bodyPartIndex] != null)
                {
                    Destroy(m_PlayerCustomisationObjects[bodyPartIndex]);
                    m_PlayerCustomisationObjects[bodyPartIndex] = null;
                }

                // under certain conditions the initialisation gets skipped
                if ((bodyPartIndex == 5 || bodyPartIndex == 6)
                    &&
                    (identifier == 0))// accessory and hair default is null
                    return;

                switch (bodyPartIndex)
                {
                    case 2:
                        path += "Head/";
                        break;
                    case 3:
                        path += "Ears/";
                        break;
                    case 4:
                        path += "Eyes/";
                        break;
                    case 5:
                        path += "Accessories/";
                        break;
                    case 6:
                        path += "Hair/";
                        break;
                    default:
                        break;
                }
                path += _id.ToString();

                // instantiation and repositioning
                m_PlayerCustomisationObjects[bodyPartIndex] = Instantiate(Resources.Load("Prefabs/Meshless", typeof(GameObject)) as GameObject);
                m_PlayerCustomisationObjects[bodyPartIndex].GetComponent<MeshFilter>().mesh = Instantiate(Resources.Load(path, typeof(Mesh)) as Mesh);
                m_PlayerCustomisationObjects[bodyPartIndex].name = _id.ToString();
                m_PlayerCustomisationObjects[bodyPartIndex].transform.SetParent(m_PlayerAnchorHead);
                m_PlayerCustomisationObjects[bodyPartIndex].transform.localPosition = Vector3.zero;
                m_PlayerCustomisationObjects[bodyPartIndex].transform.localRotation = Quaternion.identity;

                // customisation assets need to be scaled due to a 3dmax error
                ScaleFixer(m_PlayerCustomisationObjects[bodyPartIndex]);
            }
            else
                Debug.LogError("Invalid bodyPartIndex: " + bodyPartIndex);
        }
        else
            Debug.LogError("Invalid identifier: " + identifier);
    }

    /// <summary>
    /// fixes the scaling of some gameobjects
    /// </summary>
    /// <param target gameobject="_g"></param>
    public void ScaleFixer(GameObject _g)
    {
        _g.transform.localScale = new Vector3(2, 2, 2);
    }

    /// <summary>
    /// iterates through the entire players body and loads the corresponding materials
    /// </summary>
    public void PaintPlayerBody(SyncListInt _syncModel)
    {
        string path = "Prefabs/";
        if (_syncModel[0] == 0)
            path += "M_Humanoid/Body/materials/";// male path
        else
            path += "F_Humanoid/Body/materials/";// female path
        path += _syncModel[1].ToString();

        for (int i = 0; i < m_PlayerBody.Length; i++)
        {
            m_PlayerBody[i].GetComponent<MeshRenderer>().material = Resources.Load(path, typeof(Material)) as Material;
        }
    }

    /// <summary>
    /// iterates through the entire players customisation and loads the corresponding materials
    /// </summary>
    public void PaintPlayerCustomisation(SyncListInt _syncModel)
    {
        string path = "Prefabs/";
        if (_syncModel[0] == 0)
            path += "M_Humanoid/";// male path
        else
            path += "F_Humanoid/";// female path

        for (int i = 2; i <= 6; i++)// excludes gender and colors
        {
            string tmpPath = path;

            switch (i)
            {
                case 2:// head
                    tmpPath += "Head/materials/";
                    tmpPath += _syncModel[2].ToString() + _syncModel[1].ToString();
                    break;
                case 3:// ears
                    tmpPath += "Ears/materials/";
                    tmpPath += _syncModel[3].ToString() + _syncModel[1].ToString();
                    break;
                case 4:// eyes
                    tmpPath += "Eyes/materials/";
                    tmpPath += _syncModel[4].ToString() + _syncModel[8].ToString();
                    break;
                case 5:// accessories
                    tmpPath += "Accessories/materials/";

                    if (_syncModel[0] == 0)// male
                    {
                        tmpPath += _syncModel[5].ToString() + _syncModel[7].ToString();// male accessories use the hair color
                    }
                    else// female
                    {
                        tmpPath += _syncModel[5].ToString();// female accessories have a fixed color
                    }
                    break;
                case 6:// hair
                    tmpPath += "Hair/materials/";
                    tmpPath += _syncModel[6].ToString() + _syncModel[7].ToString();
                    break;
                default:
                    break;
            }

            // under certain conditions the painting process gets skipped
            if (_syncModel[i] == 50000 || _syncModel[i] == 60000)// the default accessory and hair has no material
                continue;

            m_PlayerCustomisationObjects[i].GetComponent<MeshRenderer>().material = Resources.Load(tmpPath, typeof(Material)) as Material;
        }
    }
}