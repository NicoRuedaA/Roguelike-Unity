using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void RestartGame()
    {
        int actualSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(actualSceneIndex);
        Time.timeScale = 1f;
    }


    public void GodMode()
    {

    }

    private int selected=0;
    public void SelectDoors(){
        selected++;
        if(selected>=2) {
            disableDoors();
            selected=0;
        }
        
        LevelManager.instance.CloseDoors();
    }


    private void disableDoors(){
        Debug.Log("disabledoors no implementada");
        //if no seleccionada
        //desactivar
    }


}

