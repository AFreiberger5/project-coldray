using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairSlotsOnClick : MonoBehaviour
{
    public GameObject m_HairContentPanel;
    public GameObject m_HairButtonPrefab;
    public byte m_HairSlotAmount = 10;
    public byte m_HairSlotBodyPartId;
    public byte[] m_HairMaleSlotBodyPartId;
    public byte[] m_HairFemaleSlotBodyPartId;

    private GameObject[] m_HairSlots;

    public void HairFillScrollContentWith()
    {
        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        DummyEquipOnClick[] toBeDeleted;
        try
        {
            toBeDeleted = m_HairContentPanel.GetComponentsInChildren<DummyEquipOnClick>();
        }
        catch (System.Exception)
        {
            toBeDeleted = new DummyEquipOnClick[0];
        }

        foreach (DummyEquipOnClick z in toBeDeleted)
        {
            if (z != null)
            {
                Destroy(z.gameObject);
            }
        }

        if (cd.m_DummyModel[0] == 0)// Male
        {
            //m_MaleSlotBodyPartId = new byte[m_SlotAmount];
            m_HairSlots = new GameObject[m_HairSlotAmount];

            for (int i = 0; i < m_HairSlotAmount; i++)
            {
                m_HairSlots[i] = Instantiate(m_HairButtonPrefab);
                m_HairSlots[i].transform.SetParent(m_HairContentPanel.transform);
                DummyEquipOnClick script = m_HairSlots[i].GetComponent<DummyEquipOnClick>();
                script.m_DummyBodyPartId = m_HairSlotBodyPartId;
                script.m_DummyPrefabId = m_HairMaleSlotBodyPartId[i];
                m_HairSlots[i].name = "Hair" + m_HairMaleSlotBodyPartId[i].ToString();// Name
                m_HairSlots[i].GetComponentInChildren<Text>().text = "Hair" + m_HairMaleSlotBodyPartId[i].ToString();// Text
            }
        }
        else// Female
        {
            //m_FemaleSlotBodyPartId = new byte[m_SlotAmount];
            m_HairSlots = new GameObject[m_HairSlotAmount];

            for (int i = 0; i < m_HairSlotAmount; i++)
            {
                m_HairSlots[i] = Instantiate(m_HairButtonPrefab);
                m_HairSlots[i].transform.SetParent(m_HairContentPanel.transform);
                DummyEquipOnClick script = m_HairSlots[i].GetComponent<DummyEquipOnClick>();
                script.m_DummyBodyPartId = m_HairSlotBodyPartId;
                script.m_DummyPrefabId = m_HairFemaleSlotBodyPartId[i];
                m_HairSlots[i].name = "Hair" + m_HairFemaleSlotBodyPartId[i].ToString();// Name
                m_HairSlots[i].GetComponentInChildren<Text>().text = "Hair" + m_HairFemaleSlotBodyPartId[i].ToString();// Text
            }
        }
    }
}