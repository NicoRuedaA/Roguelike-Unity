using System.Collections;
using System.Collections.Generic;
using nicorueda.Player;
using UnityEngine;
using UnityEngine.UI;

public class SistemaVida : MonoBehaviour
{

    public static SistemaVida sharedInstance;

    List<GameObject> hearts = new List<GameObject>();

    public GameObject heart;

    int manaCount;



    void Awake(){
        sharedInstance = this;
    }

    private void Start() {
      /*  //manaCount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().GetMaxHealth();
        manaCount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().GetInitialHealth();
        //manaCount=PlayerManager.instance.GetMaxHealth();
        for(int i = 0; i<manaCount; i++) addCharge(i);*/

    }

    public void addCharge(int i){
      /*  //int manaCount = m_player.GetComponent<PlayerManager>().GetMana();
        var charge = Instantiate(heart, new Vector3(heart.transform.position.x+i*0.85f,
        heart.transform.position.y, heart.transform.position.z), Quaternion.identity, GameObject.FindWithTag("healthUI").transform);
        hearts.Add(charge);*/
    }
    /*public void addChargeNew(int i){
        //int manaCount = m_player.GetComponent<PlayerManager>().GetMana();
        var charge = Instantiate(heart, new Vector3(heart.transform.position.x+i*0.88f,
        heart.transform.position.y*1.087f, heart.transform.position.z), Quaternion.identity, GameObject.FindWithTag("healthUI").transform);
        hearts.Add(charge);
    }*/

    public void ReduceCharge(int i){
        //Debug.Log("quedann " + i + " cargas");
        hearts[i].transform.GetChild(0).gameObject.SetActive(false);


    }
   
}


