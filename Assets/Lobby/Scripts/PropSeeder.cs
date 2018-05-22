using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSeeder :MonoBehaviour
{
    public struct Prop
    {
        GameObject PropPrefab;
        byte OccupationRadius;

        public Prop(GameObject _propPrefab, byte _occupationRadius)
        {
            PropPrefab = _propPrefab;
            OccupationRadius = _occupationRadius;
        }
    }

    public GameObject m_PortalAPrefab;
    public GameObject m_PortalDungeonAPrefab;
    public GameObject m_TreePrefab;
    public Prop PropPortalA;
    public Prop PropPortalDungeonA;
    public Prop PropTree;

    private void Awake()
    {
        PropPortalA = new Prop(m_PortalAPrefab, 4);
        PropPortalDungeonA = new Prop(m_PortalDungeonAPrefab, 3);
        PropTree = new Prop(m_TreePrefab, 1);
    }
}
