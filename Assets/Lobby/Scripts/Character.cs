using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Test
    public Transform m_BodyAnchor;
    public Transform m_FaceAnchor;
    public Transform m_HairAnchor;

    public GameObject m_B0;
    public GameObject m_F0;
    public GameObject m_H0;

    private GameObject m_body;
    private GameObject m_face;
    private GameObject m_hair;
    // Test

    public string m_CName;
    public int[] m_CModel;

    private void Start()
    {
        // Aktivate when Game starts
        //DontDestroyOnLoad(gameObject);
    }

    public void CreateCharacter(string _name, int _color, int _body, int _face, int _hair)
    {
        m_CName = _name;
        m_CModel = new int[4];
        m_CModel[0] = _color;
        m_CModel[1] = _body;
        m_CModel[2] = _face;
        m_CModel[3] = _hair;

        Save();
    }

    public void Save()
    {
       // SaveLoadManager.SaveCharacter(this);
    }

    public void Load(string _characterName)
    {
        //SaveLoadManager.LoadCharacter(_characterName, out m_CName, out m_CModel);
        gameObject.name = _characterName;
    }

    public void BuildCharacter()
    {
        // Color

        // Body
        InstantiateAndParent(ref m_body, m_BodyAnchor, m_B0);
        // Face
        InstantiateAndParent(ref m_face, m_FaceAnchor, m_F0);
        // Hair
        InstantiateAndParent(ref m_hair, m_HairAnchor, m_H0);
    }

    private void InstantiateAndParent(ref GameObject _part, Transform _anchor, GameObject _prefab)
    {
        Destroy(_part);
        _part = Instantiate(_prefab, _anchor.transform.position, Quaternion.identity);
        _part.transform.SetParent(_anchor);
    }
}