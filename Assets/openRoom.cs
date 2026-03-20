using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{
    public class openRoom : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.CompareTag("Player"))
            {


                LevelManager.instance.OpenDoors();
            }
        }
    }
}
