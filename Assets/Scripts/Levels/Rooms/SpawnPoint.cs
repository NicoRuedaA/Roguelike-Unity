using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
/*
    //1 bot, 2 top, 3 left, 4 right
    [SerializeField] int openingDirection ;
    [SerializeField] GameObject[] templates;
    int rand;
    bool spawned = false;

    private void Start() {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomManager>();
        Invoke("Spawn", 1f);
    }


    private void Spawn() {
        
        if (!spawned){
            switch (openingDirection){
                case 1:
                    rand = Random.Range(0, templates.bottomRooms.Length);
                    Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                break;
                case 2:
                    rand = Random.Range(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
                break;
                case 3:
                    rand = Random.Range(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                break;
                case 4:
                    rand = Random.Range(0, templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                break;
            }
        spawned = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("SpawnPoint") && other.GetComponent<SpawnPoint>().spawned==true){
            Debug.Log("asdsadsa");
            Destroy(gameObject);
        }
    }
    */

}
