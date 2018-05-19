using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CamController : NetworkBehaviour
{
    public Transform m_Parent;
    public bool m_inDungeon = false;

    private void Awake()
    {
        m_Parent = this.transform;
    }

    public void SetCamPos(Transform _newPos)
    {               
            m_Parent = _newPos;
            this.transform.SetParent(null);
            this.transform.SetParent(m_Parent);
            this.transform.SetPositionAndRotation(m_Parent.position, m_Parent.rotation);              
    }
}
