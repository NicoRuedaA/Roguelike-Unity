using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] GameObject[] objects;


    private void Start()
    {

        int rand = Random.Range(0, objects.Length);

        // Cambiamos Quaternion.identity por transform.rotation
        // Y pasamos el transform (el padre) directamente como tercer argumento
        var myNewSmoke = Instantiate(objects[rand], transform.position, transform.rotation, transform);
    }



}
