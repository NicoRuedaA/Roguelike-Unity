using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{
    public class SelectDoor : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.CompareTag("Player"))
            {
                DoorManager[] scriptsHijos = GetComponentsInChildren<DoorManager>();

                // Recorremos la lista y llamamos al método Select() en cada uno
                foreach (DoorManager hijo in scriptsHijos)
                {
                    hijo.Select();


                    this.enabled = false;

                }

            }


        }
    }
}
