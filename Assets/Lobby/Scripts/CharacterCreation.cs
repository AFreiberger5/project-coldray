using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour
{
    public int m_Index = 1;
    public GameObject[] m_Prefabs;
    public GameObject m_SlotButton;
    public int m_SlotAnmount = 10;
    public int m_YAmount = 5;

    private GameObject[] m_Slots;
    private int m_slotIndex = 0;

    private void Start()
    {
        int m_xAmount = m_SlotAnmount / m_YAmount;
        float m_xStep = 1f / m_xAmount;
        float m_yStep = 1f / m_YAmount;

        float m_xMin = 0f;
        float m_xMax = 1f / m_xAmount;
        float m_yMin = 1f - (1f / m_YAmount);
        float m_yMax = 1f;

        m_Slots = new GameObject[m_SlotAnmount];

        for (int y = 0; y < m_YAmount; y++)
        {
            for (int x = 0; x < m_xAmount; x++)
            {
                m_Slots[m_slotIndex] = Instantiate(m_SlotButton, Vector3.zero, Quaternion.identity);
                m_Slots[m_slotIndex].transform.SetParent(gameObject.transform);
                m_Slots[m_slotIndex].name = m_slotIndex.ToString();
                m_Slots[m_slotIndex].GetComponentInChildren<Text>().text = m_slotIndex.ToString();
                RectTransform rect = m_Slots[m_slotIndex].GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(m_xMin, m_yMin);
                rect.anchorMax = new Vector2(m_xMax, m_yMax);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;

                try
                {
                    EquipModelOnClick script = m_Slots[m_slotIndex].GetComponent<EquipModelOnClick>();
                    script.m_Prefab = m_Prefabs[m_slotIndex];
                    script.m_Index = m_Index;
                }
                catch (System.Exception)
                {

                }

                m_xMin += m_xStep;
                m_xMax += m_xStep;
                if (m_xMin == 1)
                {
                    m_xMin = 0;
                    m_xMax = m_xStep;
                }

                if (x == m_xAmount - 1)
                {
                    m_yMin -= m_yStep;
                    m_yMax -= m_yStep;
                }

                m_slotIndex++;
            }
        }
    }
}