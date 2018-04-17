using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FaceSlotsOnClick : MonoBehaviour
{
    public GameObject m_FaceContentPanel;
    public GameObject m_FaceButtonPrefab;
    public byte m_FaceSlotAmount = 10;
    public byte m_FaceSlotBodyPartId;
    public byte[] m_FaceMaleSlotBodyPartId;
    public byte[] m_FaceFemaleSlotBodyPartId;

    private GameObject[] m_FaceSlots;

    public void FaceFillScrollContentWith()
    {
        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        Scrollbar sb = m_FaceContentPanel.transform.parent.transform.parent.gameObject.GetComponentsInChildren<Scrollbar>().Where(o => o.gameObject.name == "Scrollbar Vertical").SingleOrDefault();
        sb.value = 1;

        DummyEquipOnClick[] toBeDeleted;
        try
        {
            toBeDeleted = m_FaceContentPanel.GetComponentsInChildren<DummyEquipOnClick>();
        }
        catch (System.Exception)
        {
            toBeDeleted = new DummyEquipOnClick[0];
        }

        foreach (DummyEquipOnClick v in toBeDeleted)
        {
            if (v != null)
            {
                Destroy(v.gameObject);
            }
        }

        if (cd.m_DummyModel[0] == 0)// Male
        {
            //m_MaleSlotBodyPartId = new byte[m_SlotAmount];
            m_FaceSlots = new GameObject[m_FaceSlotAmount];

            for (int i = 0; i < m_FaceSlotAmount; i++)
            {
                m_FaceSlots[i] = Instantiate(m_FaceButtonPrefab);
                m_FaceSlots[i].transform.SetParent(m_FaceContentPanel.transform);
                DummyEquipOnClick script = m_FaceSlots[i].GetComponent<DummyEquipOnClick>();
                script.m_DummyBodyPartId = m_FaceSlotBodyPartId;
                script.m_DummyPrefabId = m_FaceMaleSlotBodyPartId[i];
                m_FaceSlots[i].name = "Face" + m_FaceMaleSlotBodyPartId[i].ToString();// Name
                m_FaceSlots[i].GetComponentInChildren<Text>().text = "Face" + m_FaceMaleSlotBodyPartId[i].ToString();// Text
            }
        }
        else// Female
        {
            //m_FemaleSlotBodyPartId = new byte[m_SlotAmount];
            m_FaceSlots = new GameObject[m_FaceSlotAmount];

            for (int i = 0; i < m_FaceSlotAmount; i++)
            {
                m_FaceSlots[i] = Instantiate(m_FaceButtonPrefab);
                m_FaceSlots[i].transform.SetParent(m_FaceContentPanel.transform);
                DummyEquipOnClick script = m_FaceSlots[i].GetComponent<DummyEquipOnClick>();
                script.m_DummyBodyPartId = m_FaceSlotBodyPartId;
                script.m_DummyPrefabId = m_FaceFemaleSlotBodyPartId[i];
                m_FaceSlots[i].name = "Face" + m_FaceFemaleSlotBodyPartId[i].ToString();// Name
                m_FaceSlots[i].GetComponentInChildren<Text>().text = "Face" + m_FaceFemaleSlotBodyPartId[i].ToString();// Text
            }
        }
    }
}