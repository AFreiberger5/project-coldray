using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//||||||||||||||||||||||||||||||||||||||||||||||||||||\\
//||                                                ||\\
//||            Script by Gregor Hempel             ||\\
//||            23.03.2018                          ||\\
//||            Edits:                              ||\\
//||                                                ||\\
//||||||||||||||||||||||||||||||||||||||||||||||||||||\\

public class CharacterDummy : MonoBehaviour
{
    public string m_DummyName;

    public int[] m_DummyModel = new int[9]
    {
        0,// Gender
        0,// Skin Color
        0,// Head
        0,// Ears
        0,// Eyes
        0,// Accessories
        0,// Hair
        0,// Hair Color
        0//  Eye Color
    };

    public Text m_CharacterInspectorName;
    public Text m_ModeInspectorName;
    public string m_SelectedCharacter = "";

    public Transform m_DummyAnchorHead;
    public Transform m_DummyAnchorBody;
    public Transform m_DummyAnchorLArm1;
    public Transform m_DummyAnchorLArm2;
    public Transform m_DummyAnchorLArm3;
    public Transform m_DummyAnchorRArm1;
    public Transform m_DummyAnchorRArm2;
    public Transform m_DummyAnchorRArm3;
    public Transform m_DummyAnchorLLeg1;
    public Transform m_DummyAnchorLLeg2;
    public Transform m_DummyAnchorLLeg3;
    public Transform m_DummyAnchorRLeg1;
    public Transform m_DummyAnchorRLeg2;
    public Transform m_DummyAnchorRLeg3;

    private GameObject[] m_DummyCustomisationObjects = new GameObject[8];
    private GameObject[] m_DummyBody = new GameObject[13];
    private Animator m_DummyAnimator = null;

    /// <summary>
    /// gets the dummies animator
    /// savety mesures
    /// </summary>
    private void Start()
    {
        m_DummyAnimator = GetComponent<Animator>();

        m_DummyModel = new int[9];// ensures that the inspector doesn't override the array length
    }

    /// <summary>
    /// displays the dummies name in certain input fields
    /// </summary>
    private void Update()
    {
        if (m_SelectedCharacter != "")
        {
            m_CharacterInspectorName.text = m_SelectedCharacter;
            m_ModeInspectorName.text = m_SelectedCharacter;
        }
        else
        {
            m_CharacterInspectorName.text = "";
            m_ModeInspectorName.text = "";
        }
    }

    /// <summary>
    /// called when joining / hosting an online scene to preserve information stored in the dummy
    /// </summary>
    public void DontDestroyDummyOnLoad()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// saves data stored in the dummy
    /// this data is later invoked by the player character
    /// </summary>
    public void SaveDummy()
    {
        CharacterStats cs = new CharacterStats(m_DummyName, m_DummyModel, 100.0f, "NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|NONE|");
        SaveLoadManager.SaveCharacter(cs);
    }

    /// <summary>
    /// loads a player data file by its name
    /// overrides the dummies stats with the loaded data and calls every buidling function
    /// to visually display the dummy afterwards
    /// </summary>
    /// <param name="_selectedCharacter"></param>
    public void LoadCharacterOnDummy(string _selectedCharacter)
    {
        string selectedCharacter = _selectedCharacter;
        CharacterStats cs = SaveLoadManager.LoadCharacter(selectedCharacter);
        m_DummyName = cs.m_StatsName;
        m_DummyModel = cs.m_StatsModel;

        BuildDummyBody();
        BuildEntireDummyCustomisation();
        PaintDummyBody();
        PaintDummyCustomisation();
    }

    /// <summary>
    /// determines the dummies gender and builds the body according to it
    /// </summary>
    public void BuildDummyBody()
    {
        string bodyPath = "Prefabs/";

        if (m_DummyModel[0] == 0)// male
        {
            bodyPath += "M_Humanoid/Body/BodyI0";

            // gets the male animator
            m_DummyAnimator.runtimeAnimatorController =
                Resources.Load("Prefabs/F_Humanoid/Player_Character",
                typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

            m_DummyAnimator.Play("idle");
        }
        else// female
        {
            bodyPath += "F_Humanoid/Body/BodyI0";

            // gets the female animator
            m_DummyAnimator.runtimeAnimatorController =
                Resources.Load("Prefabs/F_Humanoid/Player_Character",
                typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

            m_DummyAnimator.Play("idle");
        }

        // iterates the entire body and instantiates bodyparts according to the gender
        for (int i = 0; i < m_DummyBody.Length; i++)
        {
            // deletes present bodyparts
            if (m_DummyBody[i] != null)
            {
                Destroy(m_DummyBody[i]);
                m_DummyBody[i] = null;
            }

            // adds array index information to the path
            string[] splitter = bodyPath.Split('I');
            splitter[1] = i.ToString();
            bodyPath = splitter[0] + "I" + splitter[1];

            // instantiates the body parts at the right place
            switch (i)
            {
                case 0:// torso
                    InstantiateBodyPartWithMesh(i, bodyPath, m_DummyAnchorBody);
                    break;
                case 1:// left arm anchor 1
                    InstantiateBodyPartWithMesh(i, bodyPath, m_DummyAnchorLArm1);
                    break;
                case 2:// left arm anchor 2
                    InstantiateBodyPartWithMesh(i, bodyPath, m_DummyAnchorLArm2);
                    break;
                case 3:// left arm anchor 3
                    InstantiateBodyPartWithMesh(i, bodyPath, m_DummyAnchorLArm3);
                    break;
                case 4:// right arm anchor 1
                    InstantiateBodyPartWithMesh(i, bodyPath, m_DummyAnchorRArm1);
                    break;
                case 5:// right arm anchor 2
                    InstantiateBodyPartWithMesh(i, bodyPath, m_DummyAnchorRArm2);
                    break;
                case 6:// right arm anchor 3
                    InstantiateBodyPartWithMesh(i, bodyPath, m_DummyAnchorRArm3);
                    break;
                case 7:// left leg anchor 1
                    InstantiateBodyPartWithMesh(i, bodyPath, m_DummyAnchorLLeg1);
                    break;
                case 8:// left leg anchor 2
                    InstantiateBodyPartWithMesh(i, bodyPath, m_DummyAnchorLLeg2);
                    break;
                case 9:// left leg anchor 3
                    InstantiateBodyPartWithMesh(i, bodyPath, m_DummyAnchorLLeg3);
                    break;
                case 10:// right leg anchor 1
                    InstantiateBodyPartWithMesh(i, bodyPath, m_DummyAnchorRLeg1);
                    break;
                case 11:// right leg anchor 2
                    InstantiateBodyPartWithMesh(i, bodyPath, m_DummyAnchorRLeg2);
                    break;
                case 12:// right leg anchor 3
                    InstantiateBodyPartWithMesh(i, bodyPath, m_DummyAnchorRLeg3);
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
    private void InstantiateBodyPartWithMesh(int _i, string _path, Transform _anchor)
    {
        m_DummyBody[_i] = Instantiate(Resources.Load("Prefabs/Meshless", typeof(GameObject)) as GameObject);
        m_DummyBody[_i].GetComponent<MeshFilter>().mesh = Instantiate(Resources.Load(_path, typeof(Mesh)) as Mesh);
        m_DummyBody[_i].name = "BodyI" + _i;
        m_DummyBody[_i].transform.SetParent(_anchor);
        m_DummyBody[_i].transform.localPosition = Vector3.zero;
        m_DummyBody[_i].transform.localRotation = Quaternion.identity;

        if (m_DummyModel[0] == 0)// male
            ScaleFixer(m_DummyBody[_i]);// // male body parts need to be scaled due to a 3dmax error
    }

    /// <summary>
    /// iterates through the entire customisation array and fills it with its default values
    /// also calls the build function to visually display the default parts afterwards
    /// </summary>
    public void BuildDummyDefaultCustomisation()
    {
        for (int i = 1; i < m_DummyModel.Length; i++)// excludes gender
        {
            m_DummyModel[i] = int.Parse(i.ToString() + "0000");// default customisables (male or female)

            // BUILD HERE
            BuildDummyCustomisation(m_DummyModel[i]);
        }
    }

    /// <summary>
    /// calls the build function on every value of the customisation array
    /// </summary>
    public void BuildEntireDummyCustomisation()
    {
        for (int i = 1; i < m_DummyModel.Length; i++)// excludes gender
        {
            BuildDummyCustomisation(m_DummyModel[i]);
        }
    }

    /// <summary>
    /// takes in an id and determines its assignment
    /// deletes pre existing gameobjects and instantiates the new one
    /// builds a folder path based on the gender of the dummy and the accepted id
    /// loads the asset by path and instantiates it on the according position
    /// </summary>
    /// <param asset id="_id"></param>
    public void BuildDummyCustomisation(int _id)
    {
        string path = "Prefabs/";
        if (m_DummyModel[0] == 0)
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
                if (m_DummyCustomisationObjects[bodyPartIndex] != null)
                {
                    Destroy(m_DummyCustomisationObjects[bodyPartIndex]);
                    m_DummyCustomisationObjects[bodyPartIndex] = null;
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
                m_DummyCustomisationObjects[bodyPartIndex] = Instantiate(Resources.Load("Prefabs/Meshless", typeof(GameObject)) as GameObject);
                m_DummyCustomisationObjects[bodyPartIndex].GetComponent<MeshFilter>().mesh = Instantiate(Resources.Load(path, typeof(Mesh)) as Mesh);
                m_DummyCustomisationObjects[bodyPartIndex].name = _id.ToString();
                m_DummyCustomisationObjects[bodyPartIndex].transform.SetParent(m_DummyAnchorHead);
                m_DummyCustomisationObjects[bodyPartIndex].transform.localPosition = Vector3.zero;
                m_DummyCustomisationObjects[bodyPartIndex].transform.localRotation = Quaternion.identity;

                // customisation assets need to be scaled due to a 3dmax error
                ScaleFixer(m_DummyCustomisationObjects[bodyPartIndex]);
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
    /// iterates through the entire dummy body and loads the corresponding materials
    /// </summary>
    public void PaintDummyBody()
    {
        string path = "Prefabs/";
        if (m_DummyModel[0] == 0)
            path += "M_Humanoid/Body/materials/";// male path
        else
            path += "F_Humanoid/Body/materials/";// female path
        path += m_DummyModel[1].ToString();

        for (int i = 0; i < m_DummyBody.Length; i++)
        {
            m_DummyBody[i].GetComponent<MeshRenderer>().material = Resources.Load(path, typeof(Material)) as Material;
        }
    }

    /// <summary>
    /// iterates through the entire dummy customisation and loads the corresponding materials
    /// </summary>
    public void PaintDummyCustomisation()
    {
        string path = "Prefabs/";
        if (m_DummyModel[0] == 0)
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
                    tmpPath += m_DummyModel[2].ToString() + m_DummyModel[1].ToString();
                    break;
                case 3:// ears
                    tmpPath += "Ears/materials/";
                    tmpPath += m_DummyModel[3].ToString() + m_DummyModel[1].ToString();
                    break;
                case 4:// eyes
                    tmpPath += "Eyes/materials/";
                    tmpPath += m_DummyModel[4].ToString() + m_DummyModel[8].ToString();
                    break;
                case 5:// accessories
                    tmpPath += "Accessories/materials/";

                    if (m_DummyModel[0] == 0)// male
                    {
                        tmpPath += m_DummyModel[5].ToString() + m_DummyModel[7].ToString();// male accessories use the hair color
                    }
                    else// female
                    {
                        tmpPath += m_DummyModel[5].ToString();// female accessories have a fixed color
                    }
                    break;
                case 6:// hair
                    tmpPath += "Hair/materials/";
                    tmpPath += m_DummyModel[6].ToString() + m_DummyModel[7].ToString();
                    break;
                default:
                    break;
            }

            // under certain conditions the painting process gets skipped
            if (m_DummyModel[i] == 50000 || m_DummyModel[i] == 60000)// the default accessory and hair has no material
                continue;

            m_DummyCustomisationObjects[i].GetComponent<MeshRenderer>().material = Resources.Load(tmpPath, typeof(Material)) as Material;
        }
    }

    /// <summary>
    /// deletes every information and gameobject stored in the currently active dummy
    /// </summary>
    public void EmptyOutTheDummy()
    {
        m_DummyName = "";
        m_CharacterInspectorName.text = "";
        m_ModeInspectorName.text = "";

        for (int i = 0; i < m_DummyBody.Length; i++)
        {
            Destroy(m_DummyBody[i]);
            m_DummyBody[i] = null;
        }
        for (int i = 0; i < m_DummyCustomisationObjects.Length; i++)
        {
            Destroy(m_DummyCustomisationObjects[i]);
            m_DummyCustomisationObjects[i] = null;
        }
    }
}