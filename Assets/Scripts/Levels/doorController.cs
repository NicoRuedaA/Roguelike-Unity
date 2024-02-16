using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorController : MonoBehaviour
{

   
    private bool opened;
    
    // Start is called before the first frame update
    void Start()
    {
        LevelManager.instance.InsertDoor(gameObject);
        Open();
    }

    public void Open()
    {
        gameObject.SetActive(false);
    }


    public void Close()
    {
        gameObject.SetActive(true);
    }
    
}
