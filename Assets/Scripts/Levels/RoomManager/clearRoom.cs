using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clearRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        foreach(Transform child in transform)
        {
            
                GameObject.Destroy(child.gameObject);
 }
        
    }

}
