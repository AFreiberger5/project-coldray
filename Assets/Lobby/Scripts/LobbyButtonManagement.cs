using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text.RegularExpressions;

public class LobbyButtonManagement : MonoBehaviour
{
    [Header("Requirements")]
    public GameObject m_ContentSlotPrefab;
    public GameObject m_ContentToFill;
    public Scrollbar m_ContentScrollbar;
    public InputField m_NameInputField;

    private CharacterDummy m_cd;

    private CharacterManager m_cm;

    private CustomNetworkManager m_cnm;
    
    private Text m_colorfulText;

    private GameObject[] m_SlotsAlive = new GameObject[0];
    private GameObject m_SelectedLast;

    private bool m_awake = false;

    // character name restrictions
    private Regex m_rgx = new Regex(@"^[a-zA-Z0-9]* ?[a-zA-Z0-9]*$");
    private int m_characterMin = 3;
    private int m_characterMax = 10;

    #region private slot ids

    private int[] m_SkinIdsAsSlots = new int[10]
    {
        10000,
        10001,
        10002,
        10003,
        10004,
        10005,
        10006,
        10007,
        10008,
        10009
    };
    private int[] m_FaceIdsAsSlots = new int[10]
    {
        20000,
        20001,
        20002,
        20003,
        20004,
        20005,
        20006,
        20007,
        20008,
        20009
    };
    private int[] m_EarsIdsAsSlots = new int[10]
    {
        30000,
        30001,
        30002,
        30003,
        30004,
        30005,
        30006,
        30007,
        30008,
        30009
    };
    private int[] m_EyesIdsAsSlots = new int[10]
    {
        40000,
        40001,
        40002,
        40003,
        40004,
        40005,
        40006,
        40007,
        40008,
        40009
    };
    private int[] m_AccessoriesIdsAsSlots = new int[10]
    {
        50000,
        50001,
        50002,
        50003,
        50004,
        50005,
        50006,
        50007,
        50008,
        50009
    };
    private int[] m_HairIdsAsSlots = new int[10]
    {
        60000,
        60001,
        60002,
        60003,
        60004,
        60005,
        60006,
        60007,
        60008,
        60009
    };

    #endregion

    private void Awake()
    {
        m_cd = FindObjectOfType<CharacterDummy>();

        m_cm = FindObjectOfType<CharacterManager>();

        m_cnm = FindObjectOfType<CustomNetworkManager>();

        m_colorfulText = m_NameInputField.GetComponentsInChildren<Text>().Where(o => o.gameObject.name == "Colorful Text").SingleOrDefault();

        m_NameInputField.characterLimit = m_characterMax;
        m_NameInputField.onValueChanged.AddListener(DisplayColorfulString);

        m_SelectedLast = null;

        m_awake = true;
    }

    private void Update()
    {
        if (m_awake
            &&
            EventSystem.current.currentSelectedGameObject != null
            &&
            EventSystem.current.currentSelectedGameObject != m_SelectedLast)// spam prevention
        {
            m_SelectedLast = EventSystem.current.currentSelectedGameObject;// spam prevention

            // executes functions when a button is pressed
            switch (EventSystem.current.currentSelectedGameObject.name)
            {
                // general buttons -----------------------------------------------------------------------------------------------------------------------------
                case "Delete Button":
                    DeleteSelectedCharacter();
                    break;
                case "Reload Back Button":
                    ReloadTheScene();
                    break;
                case "Quit Button":
                    QuitTheGame();
                    break;
                // character creation buttons ------------------------------------------------------------------------------------------------------------------
                case "Male Button":
                    ClearCurrentSlots();
                    DefaultTheCharacter();// character is now male
                    break;
                case "Female Button":
                    ClearCurrentSlots();
                    DefaultTheCharacter();
                    m_cd.m_DummyModel[0] = 1;// character is now female
                    break;
                case "Skin Color Button":
                    ClearCurrentSlots();
                    CreateSlotsOfType(m_SkinIdsAsSlots);
                    break;
                case "Face Button":
                    ClearCurrentSlots();
                    CreateSlotsOfType(m_FaceIdsAsSlots);
                    break;
                case "Ears Button":
                    ClearCurrentSlots();
                    CreateSlotsOfType(m_EarsIdsAsSlots);
                    break;
                case "Eyes Button":
                    ClearCurrentSlots();
                    CreateSlotsOfType(m_EyesIdsAsSlots);
                    break;
                case "Accessories Button":
                    ClearCurrentSlots();
                    CreateSlotsOfType(m_AccessoriesIdsAsSlots);
                    break;
                case "Hair Button":
                    ClearCurrentSlots();
                    CreateSlotsOfType(m_HairIdsAsSlots);
                    break;
                case "Create Button":
                    CreateCharacter();
                    break;
                // game mode buttons ---------------------------------------------------------------------------------------------------------------------------
                case "Solo Button":
                    m_cnm.SoloOnClick();
                    break;
                case "Host Button":
                    m_cnm.HostOnClick();
                    break;
                case "Join Button":
                    m_cnm.JoinOnClick();
                    break;
                default:
                    
                    break;
            }

            // changes the dummy model id on the proper index
            if (m_SlotsAlive.Contains(EventSystem.current.currentSelectedGameObject))
            {
                int slotIndex = System.Array.IndexOf(m_SlotsAlive, EventSystem.current.currentSelectedGameObject);

                int selectedId = 0;
                int.TryParse(m_SlotsAlive[slotIndex].name, out selectedId);

                int bodyPartIndex;
                int.TryParse(selectedId.ToString().Substring(0, 1), out bodyPartIndex);

                if (bodyPartIndex >= 1
                    &&
                    bodyPartIndex <= 7)
                {
                    m_cd.m_DummyModel[bodyPartIndex] = selectedId;

                    // BUILD DUMMY HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                }
                else
                {
                    Debug.LogError("LobbyButtonManagement Error:\nSlot Error");
                }
            }
        }
    }
    
    // general functions ---------------------------------------------------------------------------------------------------------------------------

    private void DeleteSelectedCharacter()
    {
        try
        {
            File.Delete(Application.persistentDataPath + "/Characters/" + m_cd.m_DummyName + ".sav");//
            File.Delete(Application.persistentDataPath + "/Characters/" + m_cd.m_DummyName + ".sav.meta");//
        }
        catch (System.Exception)
        {
            Debug.LogError("LobbyButtonManagement Error:\nCharacter could not be deleted.");
        }

        ReloadTheScene();
    }

    private void ReloadTheScene()
    {
        ClearCurrentSlots();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    private void QuitTheGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    // character creation functions ----------------------------------------------------------------------------------------------------------------

    private void CreateSlotsOfType(int[] _idArray)
    {
        m_SlotsAlive = new GameObject[_idArray.Length];

        for (int i = 0; i < _idArray.Length; i++)
        {
            m_SlotsAlive[i] = Instantiate(m_ContentSlotPrefab);
            m_SlotsAlive[i].transform.SetParent(m_ContentToFill.transform);
            m_SlotsAlive[i].name = _idArray[i].ToString();// SLOT NAME HAS TO BE THE ID!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            m_SlotsAlive[i].GetComponentInChildren<Text>().text = _idArray[i].ToString();// USE ENUM HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
    }

    private void ClearCurrentSlots()
    {
        m_ContentScrollbar.value = 1;// resets the scrollbar

        if (m_SlotsAlive.Length != 0)
        {
            m_SlotsAlive.ToList().ForEach(o => Destroy(o));
            m_SlotsAlive = new GameObject[0];
        }
    }

    private void DefaultTheCharacter()
    {
        for (int i = 0; i < m_cd.m_DummyModel.Length; i++)
        {
            m_cd.m_DummyModel[i] = 0;// default character
        }
    }

    private void CreateCharacter()
    {
        if (!m_cm.m_Files.Select(o => o.ToUpper()).Contains(m_NameInputField.text.ToUpper() + ".SAV")
            &&
            m_rgx.IsMatch(m_NameInputField.text)
            &&
            m_NameInputField.text.Length >= m_characterMin)
        {
            m_cd.m_DummyName = m_NameInputField.text;

            m_cd.SaveDummy();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
        else
        {
            m_NameInputField.characterLimit = 42;

            if (m_cm.m_Files.Select(o => o.ToUpper()).Contains(m_NameInputField.text.ToUpper() + ".SAV"))
            {
                m_NameInputField.text = "Name already exists";
                DisplayStringInRed(m_NameInputField.text);
            }
            else if (!m_rgx.IsMatch(m_NameInputField.text))
            {
                m_NameInputField.text = FilterThisString(m_NameInputField.text);
                DisplayColorfulString(m_NameInputField.text);
            }
            else if (m_NameInputField.text.Length < m_characterMin)
            {
                m_NameInputField.text = "Use more letters";
                DisplayStringInRed(m_NameInputField.text);
            }

            m_NameInputField.characterLimit = m_characterMax;
        }
    }

    private string FilterThisString(string _string)
    {
        string s = "";
        foreach (char c in _string)
        {
            if (!m_rgx.IsMatch(c.ToString()))
            {
                s += c.ToString();
            }
        }
        return s;
    }

    private void DisplayColorfulString(string _string)
    {
        m_colorfulText.text = "";

        List<char> stringList = _string.ToList();

        foreach (char c in stringList)
        {
            if (m_rgx.IsMatch(c.ToString()))
            {
                m_colorfulText.text += "<color=white>" + c.ToString() + "</color>";
            }
            else
            {
                m_colorfulText.text += "<i><color=red>" + c.ToString() + "</color></i>";
            }
        }
    }

    private void DisplayStringInRed(string _string)
    {
        m_colorfulText.text = "<b><color=red>" + _string + "</color></b>";
    }
}