using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instancedRoomManager : MonoBehaviour
{

    GameObject parentObject, doors, objects;


    private void Awake() {
        parentObject = transform.parent.gameObject;
        doors = parentObject.transform.Find("Doors").gameObject;
        objects = parentObject.transform.Find("Objects").gameObject;
        
        //GameManager.instance.roomCreated(this.transform.parent.gameObject);
        LevelManager.instance.CreateRoom(this.transform.parent.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            doors.gameObject.SetActive(true);
            objects.gameObject.SetActive(false);
        }
    }
    

        public void finishRoom(){
            doors.gameObject.SetActive(false);
            objects.gameObject.SetActive(true);
    }

}


