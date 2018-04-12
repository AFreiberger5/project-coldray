using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerSetup : NetworkBehaviour
{

    public Text m_namePrefab;
    public Text m_nameLabel;
    public Transform m_namePos;
    string m_textboxname = "";

    [SyncVar(hook = "OnChangeName")]
    public string m_pName = "Player";

    public override void OnStartClient() // nach einem Delay nach Clienverbindungen zwingen die SyncVars auch anzuzeigen
    {
        base.OnStartClient();
        Invoke("UpdateStates", 1f);

    }


    private void Start()
    {
        GameObject canvas = GameObject.FindWithTag("MainCanvas");
        m_nameLabel = Instantiate(m_namePrefab, Vector3.zero, Quaternion.identity) as Text;
        m_nameLabel.transform.SetParent(canvas.transform);
    }

    private void Update()
    {
        Vector3 nameLabelPos = Camera.main.WorldToScreenPoint(m_namePos.position); //Positions des Labels von WorldSpace in Screenspace
        m_nameLabel.transform.position = nameLabelPos;
    }

    [Command]
    public void CmdChangeName(string _newName) // Command die SyncVar zu ändern und neuen Namen anzuzeigen (server)
    {
        m_pName = _newName;
        m_nameLabel.text = m_pName;
    }

    void UpdateStates()
    {
        OnChangeName(m_pName);
    }

    void OnChangeName(string _n) // Hookfunktion, sobald SyncVar sich ändert wird diese Funktion auf den Clients ausgeführt
    {
        m_pName = _n;
        m_nameLabel.text = m_pName;
    }

    private void OnGUI()
    {
        if (isLocalPlayer)
        {
            m_textboxname = GUI.TextField(new Rect(25, 15, 100, 25), m_textboxname);
            if (GUI.Button(new Rect(130, 15, 35, 25), "Set")) // Wenn Button gedrückt, dann Command ausführen 
            {
                CmdChangeName(m_textboxname);
            }

        }
    }

    public void OnDestroy() // Label ist kein Netzwerkobjekt, Unitys Garbagecollector muss es löschen sobald das Gameobject zerstört wurde
    {
        if (m_nameLabel != null)
        {
            Destroy(m_nameLabel.gameObject);
        }
    }
}
