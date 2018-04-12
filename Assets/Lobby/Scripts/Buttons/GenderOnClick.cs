using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenderOnClick : MonoBehaviour
{
    public GameObject m_GenderContentPanel;

    public void MakeDummyMale()
    {
        DeleteBodyPartSlots();

        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        cd.m_DummyModel[0] = 0;

        for (int i = 1; i < cd.m_DummyModel.GetLength(0); i++)
        {
            cd.m_DummyModel[0] = 0;// Default character here
        }
    }

    public void MakeDummyFemale()
    {
        DeleteBodyPartSlots();

        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        cd.m_DummyModel[0] = 1;

        for (int i = 1; i < cd.m_DummyModel.GetLength(0); i++)
        {
            cd.m_DummyModel[i] = 0;// Default character here
        }
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