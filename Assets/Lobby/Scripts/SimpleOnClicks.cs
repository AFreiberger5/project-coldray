using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimpleOnClicks : MonoBehaviour
{
    public void LoadThisCharacter()
    {
        Character m_Character = FindObjectOfType<Character>();
        CharacterViewerName m_CVName = FindObjectOfType<CharacterViewerName>();

        m_Character.Load(GetComponentInChildren<Text>().text);

        m_CVName.GetComponent<Text>().text = GetComponentInChildren<Text>().text;

        // Test
        m_Character.BuildCharacter();
    }

    public void ReloadThisScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}