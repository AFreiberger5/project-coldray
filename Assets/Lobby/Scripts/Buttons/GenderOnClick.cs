using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenderOnClick : MonoBehaviour
{
    public GameObject m_GenderContentPanel;
    public Scrollbar m_BodyPartScrollbar;

    public void MakeDummyMale()
    {
        DeleteBodyPartSlots();

        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        cd.m_DummyModel[0] = 0;

        for (int i = 1; i < cd.m_DummyModel.GetLength(0); i++)
        {
            cd.m_DummyModel[0] = 0;// Default male character here
        }

        m_BodyPartScrollbar.value = 1;
    }

    public void MakeDummyFemale()
    {
        DeleteBodyPartSlots();

        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        cd.m_DummyModel[0] = 1;

        for (int i = 1; i < cd.m_DummyModel.GetLength(0); i++)
        {
            cd.m_DummyModel[i] = 0;// Default female character here
        }

        m_BodyPartScrollbar.value = 1;
    }

    private void DeleteBodyPartSlots()
    {
        DummyEquipOnClick[] toBeDeleted;
        try
        {
            toBeDeleted = m_GenderContentPanel.GetComponentsInChildren<DummyEquipOnClick>();
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
    }
}