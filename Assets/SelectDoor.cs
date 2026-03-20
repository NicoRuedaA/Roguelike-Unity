using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{
    public class SelectDoor : MonoBehaviour
    {


        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                DoorManager[] scriptsHijos = GetComponentsInChildren<DoorManager>();
                foreach (DoorManager hijo in scriptsHijos)
                {
                    //al pasar por una puerta, se cierra
                    hijo.Select();
                    //GameManager.instance.SelectDoors();
                }
            }
        }
    }
}
