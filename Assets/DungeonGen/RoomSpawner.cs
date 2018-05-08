using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RoomSpawner : NetworkBehaviour
{
    public int m_OpeningDirection;
    private RoomTemplates m_Templates;
    private int rand;
    private bool spawned = false;
    private float selfkill = 5f;    

    private void Start()
    {
        //Destroy(gameObject, selfkill);
        m_Templates = GameStatus.GetInstance().GetTemplates();
        if(GameStatus.GetInstance().m_buildDungeon)
        Invoke("Build", 0.5f);
    }

    // Update is called once per frame
    void Build()
    {        
            if (spawned == false && GameStatus.GetInstance().m_buildDungeon == true)
            {
                if (m_OpeningDirection == 1)
                {
                    rand = Random.Range(0, m_Templates.m_BottomRooms.Length);
                    Instantiate(m_Templates.m_BottomRooms[rand], transform.position, m_Templates.m_BottomRooms[rand].transform.rotation);
                    GameStatus.GetInstance().m_BotTilesA.Add(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), rand);
                }
                else if (m_OpeningDirection == 2)
                {
                    rand = Random.Range(0, m_Templates.m_TopRooms.Length);
                    Instantiate(m_Templates.m_TopRooms[rand], transform.position, m_Templates.m_TopRooms[rand].transform.rotation);
                    GameStatus.GetInstance().m_TopTilesA.Add(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), rand);

                }
                else if (m_OpeningDirection == 3)
                {
                    rand = Random.Range(0, m_Templates.m_LeftRooms.Length);
                    Instantiate(m_Templates.m_LeftRooms[rand], transform.position, m_Templates.m_LeftRooms[rand].transform.rotation);
                GameStatus.GetInstance().m_LeftTilesA.Add(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), rand);

                }
                else if (m_OpeningDirection == 4)
                {
                    rand = Random.Range(0, m_Templates.m_RightRooms.Length);
                    Instantiate(m_Templates.m_RightRooms[rand], transform.position, m_Templates.m_RightRooms[rand].transform.rotation);
                GameStatus.GetInstance().m_RightTilesA.Add(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), rand);

                }
                spawned = true;
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TileSpawnPoint") && GameStatus.GetInstance().m_buildDungeon == true)
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                Instantiate(m_Templates.m_ClosedRoom, transform.position, m_Templates.transform.rotation);
                GameStatus.GetInstance().m_DeadendsA.Add(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z));

            }
            spawned = true;
        }
    }
}
