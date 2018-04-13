using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipModelOnClick : MonoBehaviour
{
    public GameObject m_Prefab;
    public int m_Index;

    public void ChangeModel()
    {
        ModelBuilder builder = FindObjectOfType<ModelBuilder>();

        //m_modelBuilder.BuildModelWithThis(m_Prefab, m_Index);

        if (m_Index == 0)
        {

        }
        else if (m_Index == 1)
        {
            builder.BuildModelWithThis(ref builder.m_mBody, builder.m_MBodyAnchor, m_Prefab);
        }
        else if (m_Index == 2)
        {
            builder.BuildModelWithThis(ref builder.m_mFace, builder.m_MFaceAnchor, m_Prefab);
        }
        else if (m_Index == 3)
        {
            builder.BuildModelWithThis(ref builder.m_mHair, builder.m_MHairAnchor, m_Prefab);
        }
    }
}