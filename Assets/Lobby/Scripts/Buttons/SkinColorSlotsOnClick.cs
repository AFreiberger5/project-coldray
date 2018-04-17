using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SkinColorSlotsOnClick : MonoBehaviour
{
    public GameObject m_SkinContentPanel;
    public GameObject m_SkinButtonPrefab;
    public byte m_SkinSlotAmount = 10;
    public byte m_SkinSlotBodyPartId;
    public byte[] m_SkinMaleSlotBodyPartId;
    public byte[] m_SkinFemaleSlotBodyPartId;

    private GameObject[] m_SkinSlots;

    public void SkinFillScrollContentWith()
    {
        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        Scrollbar sb = m_SkinContentPanel.transform.parent.transform.parent.gameObject.GetComponentsInChildren<Scrollbar>().Where(o => o.gameObject.name == "Scrollbar Vertical").SingleOrDefault();
        sb.value = 1;

        DummyEquipOnClick[] toBeDeleted;
        try
        {
            toBeDeleted = m_SkinContentPanel.GetComponentsInChildren<DummyEquipOnClick>();
        }
        catch (System.Exception)
        {
            toBeDeleted = new DummyEquipOnClick[0];
        }

        foreach (DummyEquipOnClick u in toBeDeleted)
        {
            if (u != null)
            {
                Destroy(u.gameObject);
            }
        }

        if (cd.m_DummyModel[0] == 0)// Male
        {
            //m_MaleSlotBodyPartId = new byte[m_SlotAmount];
            m_SkinSlots = new GameObject[m_SkinSlotAmount];

            for (int i = 0; i < m_SkinSlotAmount; i++)
            {
                m_SkinSlots[i] = Instantiate(m_SkinButtonPrefab);
                m_SkinSlots[i].transform.SetParent(m_SkinContentPanel.transform);
                DummyEquipOnClick script = m_SkinSlots[i].GetComponent<DummyEquipOnClick>();
                script.m_DummyBodyPartId = m_SkinSlotBodyPartId;
                script.m_DummyPrefabId = m_SkinMaleSlotBodyPartId[i];
                m_SkinSlots[i].name = "Skin" + m_SkinMaleSlotBodyPartId[i].ToString();// Name INCOMPLETE
                m_SkinSlots[i].GetComponentInChildren<Text>().text = "Skin" + m_SkinMaleSlotBodyPartId[i].ToString();// Text INCOMPLETE
            }
        }
        else// Female
        {
            //m_FemaleSlotBodyPartId = new byte[m_SlotAmount];
            m_SkinSlots = new GameObject[m_SkinSlotAmount];

            for (int i = 0; i < m_SkinSlotAmount; i++)
            {
                m_SkinSlots[i] = Instantiate(m_SkinButtonPrefab);
                m_SkinSlots[i].transform.SetParent(m_SkinContentPanel.transform);
                DummyEquipOnClick script = m_SkinSlots[i].GetComponent<DummyEquipOnClick>();
                script.m_DummyBodyPartId = m_SkinSlotBodyPartId;
                script.m_DummyPrefabId = m_SkinFemaleSlotBodyPartId[i];
                m_SkinSlots[i].name = "Skin" + m_SkinFemaleSlotBodyPartId[i].ToString();// Name INCOMPLETE
                m_SkinSlots[i].GetComponentInChildren<Text>().text = "Skin" + m_SkinFemaleSlotBodyPartId[i].ToString();// Text INCOMPLETE
            }
        }
    }
}