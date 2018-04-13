using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Linq;

public class CreateOnClick : MonoBehaviour
{
    public GameObject m_CharacterInScene;
    public Text m_NameText;
    public Text m_ErrorText;

    private CharacterManager m_finder;

    public void CreateCharacter()
    {
        m_finder = FindObjectOfType<CharacterManager>();

        Regex rgx = new Regex(@"^[a-zA-Z0-9]* ?[a-zA-Z0-9]*$");

        if (!m_finder.m_Files.Select(o => o.ToUpper()).Contains(m_NameText.text.ToUpper() + ".SAV")
            &&
            rgx.IsMatch(m_NameText.text)
            &&
            m_NameText.text.Length >= 4
            &&
            m_NameText.text.Length <= 15)
        {
            //Character char = FindObjectOfType<Character>();
            m_CharacterInScene.SetActive(true);
            m_CharacterInScene.GetComponent<Character>().CreateCharacter(m_NameText.text, 0, 0, 0, 0);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
        else
        {
            if (m_finder.m_Files.Select(o => o.ToUpper()).Contains(m_NameText.text.ToUpper() + ".SAV"))
            {
                m_ErrorText.text = "This name already exists!";
            }
            else if (!rgx.IsMatch(m_NameText.text))
            {
                m_ErrorText.text = "Invalid name!\nPlease only use: a-Z, 0-9, _";
            }
            else if (m_NameText.text.Length < 3)
            {
                m_ErrorText.text = "Please use 3 or more letters!";
            }
            else if (m_NameText.text.Length > 15)
            {
                m_ErrorText.text = "15 Letters max.";
            }
        }
    }
}