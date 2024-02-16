using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeRoom : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D other) {
        if(LevelManager.instance.NumEnemies > 0)         LevelManager.instance.CloseDoors();

        Destroy(gameObject);
    }

    }



