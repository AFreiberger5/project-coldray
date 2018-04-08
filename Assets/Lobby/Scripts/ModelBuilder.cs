using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBuilder : MonoBehaviour
{
    public GameObject m_DefaultBody;
    public GameObject m_DrfaultFace;
    public GameObject m_DefaultHair;

    public Transform m_MBodyAnchor;
    public Transform m_MFaceAnchor;
    public Transform m_MHairAnchor;

    public GameObject m_mColor;
    public GameObject m_mBody;
    public GameObject m_mFace;
    public GameObject m_mHair;

    private void Start()
    {
        BuildModelWithThis(ref m_mBody, m_MBodyAnchor, m_DefaultBody);
        BuildModelWithThis(ref m_mFace, m_MFaceAnchor, m_DrfaultFace);
        BuildModelWithThis(ref m_mHair, m_MHairAnchor, m_DefaultHair);
    }

    public void BuildModelWithThis(ref GameObject _part, Transform _anchor, GameObject _prefab)
    {
        Destroy(_part);
        _part = Instantiate(_prefab, _anchor.transform.position, Quaternion.identity);
        _part.transform.SetParent(_anchor);
    }
}