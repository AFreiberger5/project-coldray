using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class CharacterManager : MonoBehaviour
{
    public GameObject m_ScrollContent;
    public GameObject m_CharacterSlot;
    // Test
    public Text m_Text;
    public GameObject m_CharacterViewer;
    public Button m_ContinueButton;
    public Button m_DeleteButton;
    // Test

    public List<string> m_Files = new List<string>();

    private void Start()
    {
        FindAllCharacters();
    }

    private void Update()
    {
        if (m_Text.text.Length >= 3)
        {
            m_CharacterViewer.SetActive(true);
            m_ContinueButton.interactable = true;
            m_DeleteButton.interactable = true;
        }
        else
        {
            m_CharacterViewer.SetActive(false);
            m_ContinueButton.interactable = false;
            m_DeleteButton.interactable = false;
        }
    }

    public void FindAllCharacters()
    {
        // Searches for character files and ignores the meta data
        DirectoryInfo info;

        if (!Directory.Exists(Application.dataPath + "/Characters"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Characters/");
        }

        info = new DirectoryInfo(Application.dataPath + "/Characters/");

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
            }
        }

        // Sort here
        m_Files = m_Files.OrderBy(o => o.ToString()).ToList();

        foreach (string file in m_Files)
        {
            string characterName = file;
            string[] splitter = characterName.Split('.');
            characterName = splitter[0];

            GameObject slot = Instantiate(m_CharacterSlot, m_ScrollContent.transform.position, Quaternion.identity);
            slot.name = "Slot: " + characterName;
            slot.transform.SetParent(m_ScrollContent.transform);
            slot.GetComponentInChildren<Text>().text = characterName;
        }
    }
}