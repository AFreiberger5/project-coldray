using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

    private float selfkill = 5f;

    private void Start()
    {
        Destroy(gameObject, selfkill);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Occupied"))
        Destroy(other.gameObject);
        GameStatus.GetInstance().m_DeadendsA.Remove(new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z));
    }
}
