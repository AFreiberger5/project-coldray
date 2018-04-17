using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Linq;
using System;

public class CreateCharacterOnClick : MonoBehaviour
{
    // Public
    public InputField m_NameInputField;

    // Private
    private Regex m_rgx = new Regex(@"^[a-zA-Z0-9]* ?[a-zA-Z0-9]*$");

    private int m_characterMin = 3;
    private int m_characterMax = 10;

    private Text m_ColorfulText;

    private CharacterManager m_manager;

    private CharacterDummy m_dummy;

    private void Start()
    {
        m_ColorfulText = m_NameInputField.GetComponentsInChildren<Text>().Where(o => o.gameObject.name == "Colorful Text").SingleOrDefault();
        m_manager = FindObjectOfType<CharacterManager>();
        m_dummy = FindObjectOfType<CharacterDummy>();

        m_NameInputField.characterLimit = m_characterMax;

        m_NameInputField.onValueChanged.AddListener(DisplayColorfulString);

        //m_NameInputField.contentType = InputField.ContentType.
    }

    public void CreateCharacter()
    {
        if (!m_manager.m_Files.Select(o => o.ToUpper()).Contains(m_NameInputField.text.ToUpper() + ".SAV")
            &&
            m_rgx.IsMatch(m_NameInputField.text)
            &&
            m_NameInputField.text.Length >= m_characterMin)
        {
            m_dummy.m_DummyName = m_NameInputField.text;

            m_dummy.SaveDummy();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
        else
        {
            m_NameInputField.characterLimit = 42;

            if (m_manager.m_Files.Select(o => o.ToUpper()).Contains(m_NameInputField.text.ToUpper() + ".SAV"))
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

    public string FilterThisString(string _string)
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

    public void DisplayColorfulString(string _string)
    {
        m_ColorfulText.text = "";

        List<char> stringList = _string.ToList();

        foreach (char c in stringList)
        {
            if (m_rgx.IsMatch(c.ToString()))
            {
                m_ColorfulText.text += "<color=white>" + c.ToString() + "</color>";
            }
            else
            {
                m_ColorfulText.text += "<i><color=red>" + c.ToString() + "</color></i>";
            }
        }
    }

    public void DisplayStringInRed(string _string)
    {
        m_ColorfulText.text = "<b><color=red>" + _string + "</color></b>";
    }
}