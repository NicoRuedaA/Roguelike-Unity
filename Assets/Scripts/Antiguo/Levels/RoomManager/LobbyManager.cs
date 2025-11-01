using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager sharedInstance;
    //[SerializeField] GameObject lobbyPrefab;

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject); // Evita duplicados
            return;
        }

        sharedInstance = this;
        DontDestroyOnLoad(gameObject); // Opcional: persiste entre escenas
    }

    public void Move(Vector3 vec)
    {

        //8.5 por los 10 de la sala - 2 del puente ???????
        vec.y -= 8.5f;
        vec.x += 4.5f;

        transform.position = vec;
    }

}
