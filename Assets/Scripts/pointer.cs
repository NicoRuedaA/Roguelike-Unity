using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointer : MonoBehaviour
{
    public float prueba;
    GameObject m_Player;
    Vector2 toRotate;
    float angle;

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //toRotate = PlayerMovement.instance.ReturnMove();
        //angle = Vector2.Angle(toRotate, transform.up);
        //Vector3 newDirection = Vector3.RotateTowards(transform.forward, toRotate, 1, 0);
        //transform.rotation = Quaternion.LookRotation(toRotate, Vector3.up);

        //transform.rotation = Quaternion.Euler(new Vector3(0,0 , angle)); 

    }
}
