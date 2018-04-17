using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EarSlotsOnClick : MonoBehaviour
{
    public GameObject m_EarContentPanel;
    public GameObject m_EarButtonPrefab;
    public byte m_EarSlotAmount = 10;
    public byte m_EarSlotBodyPartId;
    public byte[] m_EarMaleSlotBodyPartId;
    public byte[] m_EarFemaleSlotBodyPartId;

    private GameObject[] m_EarSlots;

    public void EarFillScrollContentWith()
    {
        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        Scrollbar sb = m_EarContentPanel.transform.parent.transform.parent.gameObject.GetComponentsInChildren<Scrollbar>().Where(o => o.gameObject.name == "Scrollbar Vertical").SingleOrDefault();
        sb.value = 1;

        DummyEquipOnClick[] toBeDeleted;
        try
        {
            toBeDeleted = m_EarContentPanel.GetComponentsInChildren<DummyEquipOnClick>();
        }
        catch (System.Exception)
        {
            toBeDeleted = new DummyEquipOnClick[0];
        }

        foreach (DummyEquipOnClick w in toBeDeleted)
        {
            if (w != null)
            {
                Destroy(w.gameObject);
            }
        }

        if (cd.m_DummyModel[0] == 0)// Male
        {
            //m_MaleSlotBodyPartId = new byte[m_SlotAmount];
            m_EarSlots = new GameObject[m_EarSlotAmount];

            for (int i = 0; i < m_EarSlotAmount; i++)
            {
                m_EarSlots[i] = Instantiate(m_EarButtonPrefab);
                m_EarSlots[i].transform.SetParent(m_EarContentPanel.transform);
                DummyEquipOnClick script = m_EarSlots[i].GetComponent<DummyEquipOnClick>();
                script.m_DummyBodyPartId = m_EarSlotBodyPartId;
                script.m_DummyPrefabId = m_EarMaleSlotBodyPartId[i];
                m_EarSlots[i].name = "Ear" + m_EarMaleSlotBodyPartId[i].ToString();// Name
                m_EarSlots[i].GetComponentInChildren<Text>().text = "Ear" + m_EarMaleSlotBodyPartId[i].ToString();// Text
            }
        }
        else// Female
        {
            //m_FemaleSlotBodyPartId = new byte[m_SlotAmount];
            m_EarSlots = new GameObject[m_EarSlotAmount];

            for (int i = 0; i < m_EarSlotAmount; i++)
            {
                m_EarSlots[i] = Instantiate(m_EarButtonPrefab);
                m_EarSlots[i].transform.SetParent(m_EarContentPanel.transform);
                DummyEquipOnClick script = m_EarSlots[i].GetComponent<DummyEquipOnClick>();
                script.m_DummyBodyPartId = m_EarSlotBodyPartId;
                script.m_DummyPrefabId = m_EarFemaleSlotBodyPartId[i];
                m_EarSlots[i].name = "Ear" + m_EarFemaleSlotBodyPartId[i].ToString();// Name
                m_EarSlots[i].GetComponentInChildren<Text>().text = "Ear" + m_EarFemaleSlotBodyPartId[i].ToString();// Text
            }
        }
    }
}