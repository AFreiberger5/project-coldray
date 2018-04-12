using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessorySlotsOnClick : MonoBehaviour
{
    public GameObject m_AccessoryContentPanel;
    public GameObject m_AccessoryButtonPrefab;
    public byte m_AccessorySlotAmount = 10;
    public byte m_AccessorySlotBodyPartId;
    public byte[] m_AccessoryMaleSlotBodyPartId;
    public byte[] m_AccessoryFemaleSlotBodyPartId;

    private GameObject[] m_AccessorySlots;

    public void AccessoryFillScrollContentWith()
    {
        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        DummyEquipOnClick[] toBeDeleted;
        try
        {
            toBeDeleted = m_AccessoryContentPanel.GetComponentsInChildren<DummyEquipOnClick>();
        }
        catch (System.Exception)
        {
            toBeDeleted = new DummyEquipOnClick[0];
        }

        foreach (DummyEquipOnClick y in toBeDeleted)
        {
            if (y != null)
            {
                Destroy(y.gameObject);
            }
        }

        if (cd.m_DummyModel[0] == 0)// Male
        {
            //m_MaleSlotBodyPartId = new byte[m_SlotAmount];
            m_AccessorySlots = new GameObject[m_AccessorySlotAmount];

            for (int i = 0; i < m_AccessorySlotAmount; i++)
            {
                m_AccessorySlots[i] = Instantiate(m_AccessoryButtonPrefab);
                m_AccessorySlots[i].transform.SetParent(m_AccessoryContentPanel.transform);
                DummyEquipOnClick script = m_AccessorySlots[i].GetComponent<DummyEquipOnClick>();
                script.m_DummyBodyPartId = m_AccessorySlotBodyPartId;
                script.m_DummyPrefabId = m_AccessoryMaleSlotBodyPartId[i];
                m_AccessorySlots[i].name = "Accessory" + m_AccessoryMaleSlotBodyPartId[i].ToString();// Name
                m_AccessorySlots[i].GetComponentInChildren<Text>().text = "Accessory" + m_AccessoryMaleSlotBodyPartId[i].ToString();// Text
            }
        }
        else// Female
        {
            //m_FemaleSlotBodyPartId = new byte[m_SlotAmount];
            m_AccessorySlots = new GameObject[m_AccessorySlotAmount];

            for (int i = 0; i < m_AccessorySlotAmount; i++)
            {
                m_AccessorySlots[i] = Instantiate(m_AccessoryButtonPrefab);
                m_AccessorySlots[i].transform.SetParent(m_AccessoryContentPanel.transform);
                DummyEquipOnClick script = m_AccessorySlots[i].GetComponent<DummyEquipOnClick>();
                script.m_DummyBodyPartId = m_AccessorySlotBodyPartId;
                script.m_DummyPrefabId = m_AccessoryFemaleSlotBodyPartId[i];
                m_AccessorySlots[i].name = "Accessory" + m_AccessoryFemaleSlotBodyPartId[i].ToString();// Name
                m_AccessorySlots[i].GetComponentInChildren<Text>().text = "Accessory" + m_AccessoryFemaleSlotBodyPartId[i].ToString();// Text
            }
        }
    }
}