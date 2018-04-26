using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MatSync : NetworkBehaviour
{

    // HexColors for testing - green: 04BF3404 , red: 9F121204 , blue: 221E9004
    string m_textbox;

    [SyncVar(hook = "OnColorChange")] 
    public string m_PlayerColor = "#ffffff";


    // Textfeld auf GUI um den String mit einer Hexa-Farbe zu befüllen
    private void OnGUI()
    {
        if (isLocalPlayer)
        {
            m_textbox = GUI.TextField(new Rect(25, 15, 100, 25), m_textbox);
            if(GUI.Button(new Rect(130,15,135,25), "Set"))
            {
                CmdChangeColor(m_textbox); //Aufruf der Commandfunktion
            }
        }
    }


    [Command] // Command an den Server damit die SyncVar Serverseits ein update bekommt und an die Clients zurück gesynct wird
    public void CmdChangeColor(string _newColor)
    {
        m_PlayerColor = _newColor;
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            r.material.SetColor("_Color", HexToColor(m_PlayerColor));
        }
    }

    void OnColorChange(string _newColor) // wird auf dem client aufgerufen wenn die syncvar sich verändert hat 
    {
        m_PlayerColor = _newColor;
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach ( Renderer r in rends)
        {
            r.material.SetColor("_Color", HexToColor(m_PlayerColor));
        }
    }

    public override void OnStartClient() // on colorchange muss bei client start aufgerufen werden, damit werte von anderen spielern beim start übernommen werden 
    {
        base.OnStartClient();
        OnColorChange(m_PlayerColor);
    }

    /// <summary>
    /// https://answers.unity.com/questions/812240/convert-hex-int-to-colorcolor32.html
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
}
