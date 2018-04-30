using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCharacter : NetworkBehaviour
{
    [SyncVar(hook = "OnChangeName")]
    public string m_PlayerName = "";

    private void OnChangeName(string _s)
    {
        m_PlayerName = _s;
        SINGLETOOOOON.Instance.RegPlayer(m_PlayerId, m_PlayerName);
    }

    [SyncVar]
    public int m_PlayerId = 42;// default, regular ids would be 0,1 ...

    public int[] m_PlayerModel = new int[9]
    {
        0,// gender
        0,// skin color
        0,// head
        0,// ears
        0,// eyes
        0,// accessories
        0,// hair
        0,// hair color
        0//  eye color
    };

    public bool Gender// index: 0
    {
        get
        {
            return m_PlayerModel[0] == 1;
        }
        set
        {
            m_PlayerModel[0] = value ? 0 : 1;
        }
    }
    public int SkinColor// index: 1
    {
        get
        {
            return m_PlayerModel[1];
        }
    }
    public int Face// index: 2
    {
        get
        {
            return m_PlayerModel[2];
        }
    }
    public int Ears// index: 3
    {
        get
        {
            return m_PlayerModel[3];
        }
    }
    public int Eyes// index: 4
    {
        get
        {
            return m_PlayerModel[4];
        }
    }
    public int Accessories// index: 5
    {
        get
        {
            return m_PlayerModel[5];
        }
    }
    public int Hair// index: 6
    {
        get
        {
            return m_PlayerModel[6];
        }
    }
    public int HairColor// index 7
    {
        get
        {
            return m_PlayerModel[7];
        }
    }
    public int EyeColor// index 8
    {
        get
        {
            return m_PlayerModel[8];
        }
    }

    // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- NEU
    //public Rigidbody m_rigidbody;
    private Vector2 m_movement;// !!!!!!!!!!!!!!!
    private Vector3 m_movement2;// !!!!!!!!!!!!!!
    private float m_speed = 10f;
    private float m_maxVelocityChange = 10f;

    //private Transform m_cam;


    private void Start()
    {
        if (isLocalPlayer)
        {
            //transform.Rotate(0, 45, 0);// RISKY BUSINESS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //m_rigidbody = GetComponent<Rigidbody>();

            //Transform camAnchor = transform.Find("CamAnchor");
            //m_cam = Camera.main.transform;
            //m_cam.SetParent(camAnchor);
            //m_cam.position = camAnchor.position;
            //m_cam.rotation = camAnchor.rotation;
        }
    }

    // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- NEU

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            if (m_PlayerName == "")
            {
                CharacterDummy dummy = FindObjectOfType<CharacterDummy>();
                CmdNetworkInitialize(dummy.m_DummyName);

                print("try to load: " + m_PlayerName);

                LoadCharacter(m_PlayerName);
            }

            // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- NEU

            //CmdPlayerMove(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
            //CmdPlayerMove2(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- NEU
        }

        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- NEU

        if (isServer)
        {
            //Vector3 targetVelocity = new Vector3(m_movement.x, 0, m_movement.y);
            //targetVelocity = m_cam.transform.TransformDirection(targetVelocity);// CAM TRANSFORM RISKY OR NOT ?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!
            //targetVelocity *= m_speed;
            //
            //Vector3 velocity = m_rigidbody.velocity;
            //Vector3 velocityChange = (targetVelocity - velocity);
            //velocityChange.x = Mathf.Clamp(velocityChange.x, -m_maxVelocityChange, m_maxVelocityChange);
            //velocityChange.z = Mathf.Clamp(velocityChange.z, -m_maxVelocityChange, m_maxVelocityChange);
            //velocityChange.y = 0;
            //m_rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);


            //m_rigidbody.MovePosition(transform.position + m_movement2);
        }

        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- NEU
    }

    [Command]
    public void CmdNetworkInitialize(string _string)
    {
        Debug.developerConsoleVisible = true;
        m_PlayerName = _string;
    }

    // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- NEU

    [Command]
    private void CmdPlayerMove(Vector2 _move)
    {
        m_movement = _move;
    }

    [Command]
    private void CmdPlayerMove2(Vector3 _move)
    {
        m_movement2 = _move.normalized * m_speed * Time.deltaTime;
    }

    // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- NEU

    public void LoadCharacter(string _characterName)
    {
        string selectedCharacter = _characterName;
        CharacterStats cs = SaveLoadManager.LoadCharacter(selectedCharacter);
        m_PlayerName = cs.m_StatsName;
        m_PlayerModel = cs.m_Model;
    
        // BUILD CHARACTER HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }

    //public string GetPrefabPath(int _id)
    //{
    //    string path = "Prefabs/";
    //
    //    if (m_PlayerModel[0] == 0)// Male
    //        path += "m";
    //    else// Female
    //        path += "f";
    //
    //    int index;
    //    int.TryParse(_id.ToString().Substring(1, 4), out index);
    //
    //    int type;
    //    int.TryParse(_id.ToString().Substring(0, 1), out type);
    //
    //    if (index < 1000)
    //    {
    //        // Body
    //        switch (type)
    //        {
    //            case 1:
    //                Debug.Log("Color");
    //                break;
    //            case 2:
    //                Debug.Log("Face");
    //                break;
    //            case 3:
    //                Debug.Log("Ears");
    //                break;
    //            case 4:
    //                Debug.Log("Eyes");
    //                break;
    //            case 5:
    //                Debug.Log("Accessories");
    //                break;
    //            case 6:
    //                Debug.Log("Hair");
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    else if (index >= 1000)
    //    {
    //        // Gear
    //        switch (type)
    //        {
    //            case 0:
    //                Debug.Log("Head");
    //                break;
    //            case 1:
    //                Debug.Log("Chest");
    //                break;
    //            case 2:
    //                Debug.Log("Legs");
    //                break;
    //            case 3:
    //                Debug.Log("Hands");
    //                break;
    //            case 4:
    //                Debug.Log("Feet");
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //
    //    path += _id.ToString();
    //
    //    Debug.Log(path);
    //
    //    return path;
    //}
}