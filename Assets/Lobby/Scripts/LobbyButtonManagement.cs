using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text.RegularExpressions;

//||||||||||||||||||||||||||||||||||||||||||||||||||||\\
//||                                                ||\\
//||            Script by Gregor Hempel             ||\\
//||            23.03.2018                          ||\\
//||            Edits:                              ||\\
//||                                                ||\\
//||||||||||||||||||||||||||||||||||||||||||||||||||||\\

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
    private Regex m_rgx = new Regex(@"^[a-zA-Z0-9]+ ?[a-zA-Z0-9]*$");
    private int m_characterMin = 3;
    private int m_characterMax = 10;

    /// <summary>
    /// multiple int arrays to store the ids used to customise the player character
    /// these arrays are displayed as buttons inside the character creation
    /// every mentioned id needs a counterpart in the resources folder
    /// </summary>
    #region private slot ids

    private int[] m_SkinIdsAsSlots = new int[2]// skin color
    {
        10000,
        10001
    };
    private int[] m_FaceIdsAsSlots = new int[2]// heads
    {
        20000,
        20001
    };
    private int[] m_EarsIdsAsSlots = new int[2]// ears
    {
        30000,
        30001
    };
    private int[] m_EyesIdsAsSlots = new int[1]// eyes
    {
        40000
    };
    private int[] m_AccessoriesIdsAsSlots = new int[4]// accessories
    {
        50000,
        50001,
        50002,
        50003
    };
    private int[] m_HairIdsAsSlots = new int[3]// hair
    {
        60000,
        60001,
        60002
    };
    private int[] m_HairColorIdsAsSlots = new int[3]// hair colors
    {
        70000,
        70001,
        70002
    };
    private int[] m_EyeColorIdsAsSlots = new int[5]// eye colors
    {
        80000,
        80001,
        80002,
        80003,
        80004
    };

    #endregion

    /// <summary>
    /// gets private variables
    /// savety mesures
    /// </summary>
    private void Awake()
    {
        m_cd = FindObjectOfType<CharacterDummy>();

        m_cm = FindObjectOfType<CharacterManager>();

        m_cnm = FindObjectOfType<CustomNetworkManager>();

        m_colorfulText = m_NameInputField.GetComponentsInChildren<Text>().Where(o => o.gameObject.name == "Colorful Text").SingleOrDefault();

        m_NameInputField.characterLimit = m_characterMax;
        m_NameInputField.onValueChanged.RemoveAllListeners();// savety mesure
        m_NameInputField.onValueChanged.AddListener(DisplayColorfulString);

        m_SelectedLast = null;

        m_awake = true;
    }

    /// <summary>
    /// registers clicked buttons and calls up the corresponding functions
    /// click spam protection
    /// </summary>
    private void Update()
    {
        GameObject csg = EventSystem.current.currentSelectedGameObject;

        if (m_awake
            &&
            csg != null
            &&
            csg != m_SelectedLast)// spam prevention
        {
            m_SelectedLast = csg;// spam prevention

            // executes functions when a button is pressed
            switch (csg.name)
            {
                // GENERAL BUTTONS -----------------------------------------------------------------------------------------------------------------------------
                case "Delete Button":
                    DeleteSelectedCharacter();
                    break;
                case "Reload Back Button":
                    ReloadTheScene();
                    break;
                case "Quit Button":
                    QuitTheGame();
                    break;
                case "New Character Button":
                    m_cd.EmptyOutTheDummy();
                    m_cd.m_DummyModel[0] = 0;// character is now male
                    m_cd.BuildDummyBody();
                    m_cd.BuildDummyDefaultCustomisation();
                    m_cd.PaintDummyBody();
                    m_cd.PaintDummyCustomisation();
                    break;
                // CHARACTER CREATION BUTTONS ------------------------------------------------------------------------------------------------------------------
                case "Male Button":
                    ClearCurrentSlots();
                    m_cd.m_DummyModel[0] = 0;// character is now male
                    m_cd.BuildDummyBody();
                    m_cd.BuildDummyDefaultCustomisation();
                    m_cd.PaintDummyBody();
                    m_cd.PaintDummyCustomisation();
                    break;
                case "Female Button":
                    ClearCurrentSlots();
                    m_cd.m_DummyModel[0] = 1;// character is now female
                    m_cd.BuildDummyBody();
                    m_cd.BuildDummyDefaultCustomisation();
                    m_cd.PaintDummyBody();
                    m_cd.PaintDummyCustomisation();
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
                case "Hair Color Button":
                    ClearCurrentSlots();
                    CreateSlotsOfType(m_HairColorIdsAsSlots);
                    break;
                case "Eye Color Button":
                    ClearCurrentSlots();
                    CreateSlotsOfType(m_EyeColorIdsAsSlots);
                    break;
                case "Create Button":
                    CreateCharacter();
                    break;
                // GAME MODE BUTTONS ---------------------------------------------------------------------------------------------------------------------------
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

            // changes ids at the proper position
            if (m_SlotsAlive.Contains(csg))// checks if any customisation button was clicked
            {
                int slotIndex = System.Array.IndexOf(m_SlotsAlive, csg);// returns the index of the clicked button

                int selectedId = int.Parse(m_SlotsAlive[slotIndex].name);// every button has his id stored as its name
                int customisationIndex = int.Parse(selectedId.ToString().Substring(0, 1));// the index is the first digit of the id

                if (customisationIndex == 1)// skin color
                {
                    m_cd.m_DummyModel[customisationIndex] = selectedId;

                    m_cd.PaintDummyBody();
                    m_cd.PaintDummyCustomisation();
                }
                else if (customisationIndex >= 2 && customisationIndex <= 6)// customisation: head, ears, eyes, accessories, hair
                {
                    m_cd.m_DummyModel[customisationIndex] = selectedId;

                    m_cd.BuildDummyCustomisation(selectedId);
                    m_cd.PaintDummyCustomisation();
                }
                else if (customisationIndex == 7)// hair color
                {
                    m_cd.m_DummyModel[customisationIndex] = selectedId;

                    m_cd.PaintDummyCustomisation();
                }
                else if (customisationIndex == 8)// eye color
                {
                    m_cd.m_DummyModel[customisationIndex] = selectedId;

                    m_cd.PaintDummyCustomisation();
                }
                else
                {
                    Debug.LogError("LobbyButtonManagement Error:\nSlot Error");
                }
            }
        }
    }

    // GENERAL FUNCTIONS ---------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// deletes the file corresponding to the dummy name
    /// </summary>
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

    /// <summary>
    /// gets the current build index and loads the scene again
    /// </summary>
    private void ReloadTheScene()
    {
        ClearCurrentSlots();// savety mesure

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    /// <summary>
    /// quits the game
    /// </summary>
    private void QuitTheGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // CHARACTER CREATION FUNCTIONS ----------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// creates a button for every id in the transferred int array
    /// stores the created buttons in an array, when an other category gets selected every present button gets deleted
    /// buttons store the corresponding id as name
    /// </summary>
    /// <param int array with ids="_idArray"></param>
    private void CreateSlotsOfType(int[] _idArray)
    {
        m_SlotsAlive = new GameObject[_idArray.Length];

        for (int i = 0; i < _idArray.Length; i++)
        {
            m_SlotsAlive[i] = Instantiate(m_ContentSlotPrefab);
            m_SlotsAlive[i].transform.SetParent(m_ContentToFill.transform);
            m_SlotsAlive[i].name = _idArray[i].ToString();// the slot name stores the id for later use
            m_SlotsAlive[i].GetComponentInChildren<Text>().text = _idArray[i].ToString();// USE ENUM HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            m_SlotsAlive[i].transform.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// deletes every pre existing buttons
    /// </summary>
    private void ClearCurrentSlots()
    {
        m_ContentScrollbar.value = 1;// resets the scrollbar

        if (m_SlotsAlive.Length != 0)
        {
            m_SlotsAlive.ToList().ForEach(o => Destroy(o));// destroys every button in the array
            m_SlotsAlive = new GameObject[0];
        }
    }

    /// <summary>
    /// checks for name violations and displays them in the name input field
    /// saves information stored in the current dummy as a binary file
    /// </summary>
    private void CreateCharacter()
    {
        if (!m_cm.m_Files.Select(o => o.ToUpper()).Contains(m_NameInputField.text.ToUpper() + ".SAV")// the name doesn't already exists
            &&
            m_rgx.IsMatch(m_NameInputField.text)// the name matches the regex
            &&
            m_NameInputField.text.Length >= m_characterMin)// the name contains enough characters
        {
            m_cd.m_DummyName = m_NameInputField.text;// gets the name

            m_cd.SaveDummy();// saves all informations

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);// reloads the scene
        }
        else
        {
            m_NameInputField.characterLimit = 42;// extends the allowed character limit

            if (m_cm.m_Files.Select(o => o.ToUpper()).Contains(m_NameInputField.text.ToUpper() + ".SAV"))// the name already exists
            {
                m_NameInputField.text = "Name already exists";
                DisplayStringInRed(m_NameInputField.text);
            }
            else if (!m_rgx.IsMatch(m_NameInputField.text))// the name doesn't match the regex
            {
                m_NameInputField.text = FilterThisString(m_NameInputField.text);
                DisplayColorfulString(m_NameInputField.text);
            }
            else if (m_NameInputField.text.Length < m_characterMin)// the name doesn't contain enough characters
            {
                m_NameInputField.text = "Use more letters";
                DisplayStringInRed(m_NameInputField.text);
            }

            m_NameInputField.characterLimit = m_characterMax;
        }
    }

    /// <summary>
    /// returns every char that isn't allowed by the regex
    /// </summary>
    /// <param string to be checked="_string"></param>
    /// <returns></returns>
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

    /// <summary>
    /// displays a string as rich text
    /// regex violations are displayed in red
    /// addresses a text beside the input field, because input fields don't support rich text
    /// </summary>
    /// <param string to be displayed="_string"></param>
    private void DisplayColorfulString(string _string)
    {
        m_colorfulText.text = "";

        List<char> stringList = _string.ToList();

        foreach (char c in stringList)
        {
            if (m_rgx.IsMatch(c.ToString()))
            {
                m_colorfulText.text += "<color=black>" + c.ToString() + "</color>";
            }
            else
            {
                m_colorfulText.text += "<i><color=red>" + c.ToString() + "</color></i>";
            }
        }
    }

    /// <summary>
    /// displays a string as red rich text
    /// </summary>
    /// <param string to be displayed in red="_string"></param>
    private void DisplayStringInRed(string _string)
    {
        m_colorfulText.text = "<i><color=red>" + _string + "</color></i>";
    }
}