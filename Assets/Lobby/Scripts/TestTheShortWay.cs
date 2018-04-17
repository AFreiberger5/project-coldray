using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TestTheShortWay : MonoBehaviour
{
    public string m_StringOne = "CASE: ONE";
    public string m_StringTwo = "CASE: TWO";
    public InputField m_Field;

#pragma warning disable 0414
    private Text m_TextOne;
    private Text m_TextTwo;

    public bool m_WayOne;
    public bool m_WayTwo;
#pragma warning restore 0414

    private void Start()
    {
        //Debug.Log("<color=green>TEST</color> ");// BEISPIEL !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }

    private void Update()
    {
        if (m_WayOne)
        {
            m_WayOne = false;

            m_TextOne = TestFunctionOne();
            m_TextOne.color = new Color(0, 0, 1, 1);
            m_Field.text = m_StringOne;
        }
        else if (m_WayTwo)
        {
            m_WayTwo = false;

            m_TextTwo = TestFunctionTwo();
            m_TextOne.color = new Color(1, 0, 0, 1);
            m_Field.text = m_StringTwo;
        }
    }


    // WAY ONE
    public Text TestFunctionOne()
    {
        return m_Field.GetComponentsInChildren<Text>().Where(o => o.gameObject.name == "Text").SingleOrDefault();
    }


    // WAY TWO
    public Text TestFunctionTwo()
    {
        List<Text> texts = m_Field.GetComponentsInChildren<Text>().ToList();
        foreach (Text t in texts)
        {
            return (t.name == "Text" ? t : NewText());
        }
        return NewText();
    }


    private Text NewText()
    {
        //bk dance afu ra state of the arts
        GameObject newGO = new GameObject("ERROR");
        newGO.transform.SetParent(this.transform);

        Text myText = newGO.AddComponent<Text>();
        myText.text = "ERROR";
        return myText;
    }
}