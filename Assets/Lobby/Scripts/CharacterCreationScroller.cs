using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationScroller : MonoBehaviour
{
    public GameObject m_ScrollButton;
    public int m_Index = 0;
    public GameObject[] m_Prefabs;

    private GameObject[] m_slots;
    private int m_slotIndex = 0;

    public void Start()
    {
        m_slots = new GameObject[m_Prefabs.GetLength(0)];

        foreach (GameObject g in m_Prefabs)
        {
            m_slots[m_slotIndex] = Instantiate(m_ScrollButton);
            m_slots[m_slotIndex].name = m_slotIndex.ToString();
            m_slots[m_slotIndex].transform.SetParent(gameObject.transform);
            m_slots[m_slotIndex].GetComponentInChildren<Text>().text = m_slotIndex.ToString();

            EquipModelOnClick script = m_slots[m_slotIndex].GetComponent<EquipModelOnClick>();
            script.m_Prefab = m_Prefabs[m_slotIndex];
            script.m_Index = m_Index;

            m_slotIndex++;
        }
    }
}