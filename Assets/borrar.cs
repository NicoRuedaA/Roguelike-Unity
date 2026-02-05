using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{
    public class borrar : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.CompareTag("Player"))
            {
                Debug.Log("el jugador entra");
                LevelManager.instance.OpenDoors();
            }
        }

    }
}
