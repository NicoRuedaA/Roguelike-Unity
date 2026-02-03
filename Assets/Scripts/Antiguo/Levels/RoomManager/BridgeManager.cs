using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeManager : MonoBehaviour
{
    [SerializeField] int openingDirection = 1;
    [SerializeField] GameObject[] bridges;

    bool spawned = false;
    [SerializeField] bool left = false;
    [SerializeField] bool right = false;
    bool actived = false;


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

                Instantiate(bridges[1], transform.position, Quaternion.identity);
            }
            else if (right)
            {

                Instantiate(bridges[2], transform.position, Quaternion.identity);
            }
            else
            {

                Instantiate(bridges[0], transform.position, Quaternion.identity);
            }

        }
        spawned = true;
        Destroy(gameObject);
    }
}

