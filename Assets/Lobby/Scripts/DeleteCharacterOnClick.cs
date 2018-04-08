using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class DeleteCharacterOnClick : MonoBehaviour
{
    public Text m_Text;

    public void DeleteThisCharacter()
    {
        try
        {
            File.Delete(Application.dataPath + "/Characters/" + m_Text.text + ".sav");
            File.Delete(Application.dataPath + "/Characters/" + m_Text.text + ".sav.meta");
        }
        catch (System.Exception)
        {

        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}