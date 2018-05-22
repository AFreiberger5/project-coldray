using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;

//||||||||||||||||||||||||||||||||||||||||||||||||||||\\
//||                                                ||\\
//||            Script by Gregor Hempel             ||\\
//||            23.03.2018                          ||\\
//||            Edits:                              ||\\
//||                                                ||\\
//||||||||||||||||||||||||||||||||||||||||||||||||||||\\

public class CharacterManager : MonoBehaviour
{
    public GameObject m_CharacterSelectionScroller;
    public GameObject m_CharacterSlot;
    public GameObject m_CharacterInspector;
    public Button m_ContinueButton;
    public Button m_DeleteButton;
    public List<string> m_Files = new List<string>();

    private CharacterDummy m_dummy;

    /// <summary>
    /// procures the current dummy and executes the findallcharacters function
    /// </summary>
    private void Start()
    {
        m_dummy = FindObjectOfType<CharacterDummy>();

        FindAllCharacters();
    }

    /// <summary>
    /// locks the continue and delete button as long as there is no player character selected
    /// </summary>
    private void Update()
    {
        if (m_Files.Count != 0
            &&
            m_dummy.m_SelectedCharacter != "")
        {
            m_ContinueButton.interactable = true;
            m_DeleteButton.interactable = true;
        }
        else
        {
            m_ContinueButton.interactable = false;
            m_DeleteButton.interactable = false;
        }
    }

    /// <summary>
    /// searches for character stat files and ignores the meta data in the process
    /// </summary>
    public void FindAllCharacters()
    {
        DirectoryInfo info;

        if (Directory.Exists(Application.persistentDataPath + "/Characters") == false)// player character folder doesn't exist
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Characters/");// creates the folder
        }

        info = new DirectoryInfo(Application.persistentDataPath + "/Characters/");// used to procure all player character files

        FileInfo[] fileInfo = info.GetFiles();
        for (int i = 0; i < fileInfo.GetLength(0); i++)
        {
            if (fileInfo[i].Name.Contains(".meta")// is a meta data
                ||
                fileInfo[i].Name.Contains(".sav") == false)// contains the name but isn't a .sav file
            {
                continue;// ignores the data
            }
            else
            {
                m_Files.Add(fileInfo[i].Name);// adds the file to a list
            }
        }

        // checks for empty buttons and deletes them
        m_Files.RemoveAll(o => o.ToString() == "");

        // sorts the files by alphabet
        m_Files = m_Files.OrderBy(o => o.ToString()).ToList();

        // instantiates a button for every player data found in the corresponding folder
        foreach (string file in m_Files)
        {
            string characterName = file;
            string[] splitter = characterName.Split('.');
            characterName = splitter[0];// seperates the character name from the file name

            GameObject slot = Instantiate(m_CharacterSlot, m_CharacterSelectionScroller.transform.position, Quaternion.identity);
            slot.name = "Slot: " + characterName;
            slot.transform.SetParent(m_CharacterSelectionScroller.transform);
            slot.GetComponentInChildren<Text>().text = characterName;
        }
    }
}