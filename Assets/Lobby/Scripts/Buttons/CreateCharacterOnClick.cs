using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Linq;

public class CreateCharacterOnClick : MonoBehaviour
{
    public Text m_NameInputField;
    public Text m_ErrorText;

    public void CreateCharacter()
    {
        CharacterManager manager = FindObjectOfType<CharacterManager>();
        CharacterDummy dummy = FindObjectOfType<CharacterDummy>();

        Regex rgx = new Regex(@"^[a-zA-Z0-9]* ?[a-zA-Z0-9]*$");

        if (!manager.m_Files.Select(o => o.ToUpper()).Contains(m_NameInputField.text.ToUpper() + ".SAV")
            &&
            rgx.IsMatch(m_NameInputField.text)
            &&
            m_NameInputField.text.Length >= 4
            &&
            m_NameInputField.text.Length <= 15)
        {
            dummy.m_DummyName = m_NameInputField.text;

            dummy.SaveDummy();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
        else
        {
            if (manager.m_Files.Select(o => o.ToUpper()).Contains(m_NameInputField.text.ToUpper() + ".SAV"))
            {
                m_ErrorText.text = "This name already exists!";
            }
            else if (!rgx.IsMatch(m_NameInputField.text))
            {
                m_ErrorText.text = "Invalid name!\nPlease only use: a-Z, 0-9, _";
            }
            else if (m_NameInputField.text.Length < 3)
            {
                m_ErrorText.text = "Please use 3 or more letters!";
            }
            else if (m_NameInputField.text.Length > 15)
            {
                m_ErrorText.text = "15 Letters max.";
            }
        }
    }
}