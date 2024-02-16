using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class spawn4  : MonoBehaviour
{

    public List<GameObject> roomList;
    public GameObject[] roomListArray;

    // Start is called before the first frame update
    void Start()
    {
        
    string e = (LevelManager.instance.D.ToString() + LevelManager.instance.B.ToString());
    


    roomListArray = Resources.LoadAll<GameObject>("4/" + e);
    roomList = roomListArray.ToList();

    GameObject roomToBuild = roomList [Random.Range (0, roomList.Count)];
    GameObject newRoom = Instantiate (roomToBuild, transform.position, Quaternion.identity) as GameObject;
    newRoom.transform.parent = gameObject.transform;


    }
}
