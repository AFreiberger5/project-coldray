using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleFunctionOnClick : MonoBehaviour
{
    public void LoadThisCharacter()
    {
        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        cd.LoadCharacterOnDummy(gameObject.GetComponentInChildren<Text>().text);
        // TEST
        cd.m_SelectedCharacter = gameObject.GetComponentInChildren<Text>().text;
        // TEST
    }

    public void DeleteThisCharacter()
    {
        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        try
        {
            File.Delete(Application.persistentDataPath + "/Characters/" + cd.m_DummyName + ".sav");//
            File.Delete(Application.persistentDataPath + "/Characters/" + cd.m_DummyName + ".sav.meta");//
        }
        catch (System.Exception)
        {
            Debug.LogError("DeleteCharacterOnClick Error:\nCharacter could not be deleted.");
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
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