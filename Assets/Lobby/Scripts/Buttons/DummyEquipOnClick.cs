using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEquipOnClick : MonoBehaviour
{
    public byte m_DummyBodyPartId;
    public byte m_DummyPrefabId;

    public void ChangeModelIdAt()
    {
        CharacterDummy cd = FindObjectOfType<CharacterDummy>();

        if (m_DummyBodyPartId >= 2
            &&
            m_DummyBodyPartId <= 7
            &&
            m_DummyPrefabId >= 0
            &&
            m_DummyPrefabId <= 255)
        {
            cd.m_DummyModel[m_DummyBodyPartId] = m_DummyPrefabId;

            // BUILD DUMMY HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
        else
        {
            if (m_DummyPrefabId < 0
                ||
                m_DummyPrefabId > 255)
            {
                Debug.LogError("EquipDummyWithPrefab Error\nInvalid _id: " + m_DummyPrefabId);
            }
            else
            {
                Debug.LogError("EquipDummyWithPrefab Error\nInvalid _bodyPart: " + m_DummyBodyPartId);
            }
        }
    }
}