using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeManager : MonoBehaviour
{
    //[SerializeField] string bridgePosition;

    [SerializeField] int openingDirection = 1;
    [SerializeField] GameObject[] templates;
    bool spawned = false;
    [SerializeField] bool left = false;
    [SerializeField] bool right = false;


    //Vector3 wallPosition;
    bool actived = false;
    //int size;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !actived)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        if (!spawned)
        {
            if (left)
            {
                Instantiate(templates[openingDirection - 1], transform.position, Quaternion.Euler(0, 0, -90));
            }
            else if (right)
            {
                Instantiate(templates[openingDirection - 1], transform.position, Quaternion.Euler(0, 0, 90));
            }
            else
            {
                Instantiate(templates[openingDirection - 1], transform.position, Quaternion.identity);
            }

        }
        spawned = true;
        Destroy(gameObject);
    }
}

