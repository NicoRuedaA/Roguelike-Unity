using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgePoint : MonoBehaviour
{
    //[SerializeField] string bridgePosition;

    [SerializeField] int openingDirection = 1;
    [SerializeField] GameObject[] templates;
    bool spawned = false;


    Vector3 wallPosition;
    bool actived = false;
    int size;


    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !actived){
            
            Spawn();
            
        }
    }

    private void Spawn()
    {
        if (!spawned)
        {
            Instantiate(templates[openingDirection - 1], transform.position, Quaternion.identity);
            spawned = true;
            Destroy(gameObject);
            LevelManager.instance.DeleteDoors();

        }
    }
}

/**



*/


