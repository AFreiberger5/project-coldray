using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class CharacterManager : MonoBehaviour
{
    public GameObject m_CharacterSelectionScroller;
    public GameObject m_CharacterSlot;
    public GameObject m_CharacterInspector;
    public Button m_ContinueButton;
    public Button m_DeleteButton;

    public List<string> m_Files = new List<string>();

    // TEST
    private CharacterDummy m_dummy;
    // TEST

    private void Start()
    {
        // TEST
        m_dummy = FindObjectOfType<CharacterDummy>();
        // TEST

        FindAllCharacters();
    }

    private void Update()
    {
        if (m_Files.Count != 0
            &&
            m_dummy.m_SelectedCharacter != "")
        {
            //m_CharacterInspector.SetActive(true);
            m_ContinueButton.interactable = true;
            m_DeleteButton.interactable = true;
        }
        else
        {
            //m_CharacterInspector.SetActive(false);
            m_ContinueButton.interactable = false;
            m_DeleteButton.interactable = false;
        }
    }

    public void FindAllCharacters()
    {
        // Searches for character files and ignores the meta data
        DirectoryInfo info;

        if (!Directory.Exists(Application.persistentDataPath + "/Characters"))//
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Characters/");//
        }

        info = new DirectoryInfo(Application.persistentDataPath + "/Characters/");//

        FileInfo[] fileInfo = info.GetFiles();
        for (int i = 0; i < fileInfo.GetLength(0); i++)
        {
            if (fileInfo[i].Name.Contains(".meta")
                ||
                fileInfo[i].Name.Contains(".sav") == false)
            {
                continue;
            }
            else
            {
                m_Files.Add(fileInfo[i].Name);
                Debug.Log(fileInfo[i].Name + " found." + "\n");
            }
        }

        if (m_Files.Count == 0)
        {
            try
            {
                //FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Characters/");
                //FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Characters/" + "Characters.meta");
            }
            catch (System.Exception)
            { }
        }

        // Checks for empty buttons and deletes them
        m_Files.RemoveAll(o => o.ToString() == "");

        // Sort here
        m_Files = m_Files.OrderBy(o => o.ToString()).ToList();

        foreach (string file in m_Files)
        {
            string characterName = file;
            string[] splitter = characterName.Split('.');
            characterName = splitter[0];

            GameObject slot = Instantiate(m_CharacterSlot, m_CharacterSelectionScroller.transform.position, Quaternion.identity);
            slot.name = "Slot: " + characterName;
            slot.transform.SetParent(m_CharacterSelectionScroller.transform);
            slot.GetComponentInChildren<Text>().text = characterName;
        }

        if (m_Files.Count > 0)
        {
            Debug.Log("Characters in List: " + m_Files.Count.ToString() + "\n");
        }
    }
}