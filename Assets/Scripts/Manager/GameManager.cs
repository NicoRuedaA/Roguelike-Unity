using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Game Manager is Null!!!");
            }
            return _instance;
        }
    }


    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Debug.Log("Ya existe un GameManager. Destruyendo esta instancia duplicada.");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }


    public void GodMode()
    {

    }

    //IMPLEMENTAR EN OTRO SITIO
    /*[SerializeField] private AudioSource enemigo_muere;*/



}

