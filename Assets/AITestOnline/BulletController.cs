/**************************************************
*  Credits: Created by Alexander Freiberger		  *
*                                                 *
*  Additional Edits by:                           *
*                                                 *
*                                                 *
*                                                 *
***************************************************/

using UnityEngine;
using UnityEngine.Networking;


public class BulletController : NetworkBehaviour 
{
    private readonly EDamageType DamageType = EDamageType.PHYSICAL | EDamageType.RANGED;
    private float m_damage = 15;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    [ServerCallback]
    private void  OnTriggerEnter(Collider _col)
    {
        if(_col.gameObject.CompareTag("Player"))
        {
            _col.gameObject.GetComponent<PlayerController>().OnPlayerTakeDamage(m_damage, DamageType);
            Destroy(this.gameObject);
        }
        if(_col.gameObject.CompareTag("Wall"))
            Destroy(this.gameObject);
    }
}
