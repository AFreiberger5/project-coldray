using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EyeSlotsOnClick : MonoBehaviour
{
    public GameObject m_EyeContentPanel;
    public GameObject m_EyeButtonPrefab;
    public byte m_EyeSlotAmount = 10;
    public byte m_EyeSlotBodyPartId;
    public byte[] m_EyeMaleSlotBodyPartId;
    public byte[] m_EyeFemaleSlotBodyPartId;

    private GameObject[] m_EyeSlots;

    public void EyeFillScrollContentWith()
    {
        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        Scrollbar sb = m_EyeContentPanel.transform.parent.transform.parent.gameObject.GetComponentsInChildren<Scrollbar>().Where(o => o.gameObject.name == "Scrollbar Vertical").SingleOrDefault();
        sb.value = 1;

        DummyEquipOnClick[] toBeDeleted;
        try
        {
            toBeDeleted = m_EyeContentPanel.GetComponentsInChildren<DummyEquipOnClick>();
        }
        catch (System.Exception)
        {
            toBeDeleted = new DummyEquipOnClick[0];
        }

        foreach (DummyEquipOnClick x in toBeDeleted)
        {
            if (x != null)
            {
                Destroy(x.gameObject);
            }
        }

        if (cd.m_DummyModel[0] == 0)// Male
        {
            //m_MaleSlotBodyPartId = new byte[m_SlotAmount];
            m_EyeSlots = new GameObject[m_EyeSlotAmount];

            for (int i = 0; i < m_EyeSlotAmount; i++)
            {
                m_EyeSlots[i] = Instantiate(m_EyeButtonPrefab);
                m_EyeSlots[i].transform.SetParent(m_EyeContentPanel.transform);
                DummyEquipOnClick script = m_EyeSlots[i].GetComponent<DummyEquipOnClick>();
                script.m_DummyBodyPartId = m_EyeSlotBodyPartId;
                script.m_DummyPrefabId = m_EyeMaleSlotBodyPartId[i];
                m_EyeSlots[i].name = "Eye" + m_EyeMaleSlotBodyPartId[i].ToString();// Name
                m_EyeSlots[i].GetComponentInChildren<Text>().text = "Eye" + m_EyeMaleSlotBodyPartId[i].ToString();// Text
            }
        }
        else// Female
        {
            //m_FemaleSlotBodyPartId = new byte[m_SlotAmount];
            m_EyeSlots = new GameObject[m_EyeSlotAmount];

            for (int i = 0; i < m_EyeSlotAmount; i++)
            {
                m_EyeSlots[i] = Instantiate(m_EyeButtonPrefab);
                m_EyeSlots[i].transform.SetParent(m_EyeContentPanel.transform);
                DummyEquipOnClick script = m_EyeSlots[i].GetComponent<DummyEquipOnClick>();
                script.m_DummyBodyPartId = m_EyeSlotBodyPartId;
                script.m_DummyPrefabId = m_EyeFemaleSlotBodyPartId[i];
                m_EyeSlots[i].name = "Eye" + m_EyeFemaleSlotBodyPartId[i].ToString();// Name
                m_EyeSlots[i].GetComponentInChildren<Text>().text = "Eye" + m_EyeFemaleSlotBodyPartId[i].ToString();// Text
            }
        }
    }
}