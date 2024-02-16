
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuPausa : MonoBehaviour
{

    //[SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject BotonPausa;
    [SerializeField] private GameObject MenusPausa;
    public void pauseButton()
    {
        BotonPausa.SetActive(false);
        MenusPausa.SetActive(true);
        Time.timeScale = 0;


    }
    public void playButton()
    {
        BotonPausa.SetActive(true);
        MenusPausa.SetActive(false);
        Time.timeScale = 1;

    }
    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /*public void Reanudar()
    {
        juegopausado = true;
        Time.timeScale = 1f;
        //botonPausa.SetActive(true);
        menuPausa.SetActive(false);


    }*/
    /*public void Reinciar()
    {
        juegopausado = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);

    }*/
    

}
