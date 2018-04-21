using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PlayerHealth : NetworkBehaviour
{
    [SyncVar(hook = "OnHealthChange")]
    float m_currentHealth = 100;

    public float m_maxHealth = 100;

    [SyncVar]
    public bool m_isDead = false;

    public GameObject m_deathPrefab;
    public Transform m_healthPos;
    public Slider m_healthSliderPrefab;
    public Slider m_healthSlider;
    public Camera m_PlayerCam;

    public override void OnStartClient() // nach einem Delay nach Clienverbindungen zwingen die SyncVars auch anzuzeigen
    {
        base.OnStartClient();
        Invoke("UpdateStates", 1f);

    }

    // Use this for initialization
    void Start()
    {
        GameObject canvas = GameObject.FindWithTag("MainCanvas");
        m_healthSlider = Instantiate(m_healthSliderPrefab, Vector3.zero, Quaternion.identity) as Slider;
        m_healthSlider.transform.SetParent(canvas.transform);
        Reset();        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 healthLabelPos = m_PlayerCam.WorldToScreenPoint(m_healthPos.position); //Positions des Labels von WorldSpace in Screenspace
        m_healthSlider.transform.position = healthLabelPos;
    }

    void OnHealthChange(float _value)
    {
        if (m_healthSlider != null)
        {
            m_currentHealth = _value;
            m_healthSlider.value = m_currentHealth;
        }
    }

    void UpdateStates()
    {
        OnHealthChange(m_currentHealth);
    }


    public void Damage(float _dmg)
    {
        m_currentHealth += _dmg;
        m_healthSlider.value = m_currentHealth;

        if (m_currentHealth <= 0 && !m_isDead)
        {
            m_isDead = true;
            RpcDie();
        }
    }

    [ClientRpc]
    void RpcDie()
    {
        if (m_deathPrefab)
        {
            GameObject deathFX = Instantiate(m_deathPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
            Destroy(deathFX, 3f);
        }

        SetActiveState(false);

        gameObject.SendMessage("Disable");
    }

    void SetActiveState(bool _state)
    {
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = _state;
        }
        foreach (Canvas c in GetComponentsInChildren<Canvas>())
        {
            c.enabled = _state;
        }
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = _state;
        }

    }
    public void Reset()
    {
        m_currentHealth = m_maxHealth;
        SetActiveState(true);
        m_isDead = false;
    }

    private void OnDestroy()
    {
        if (m_healthSlider != null)
        {
            Destroy(m_healthSlider.gameObject);
        }
    }
}
