using System.Collections;
using System.Collections.Generic;
using nicorueda.Player;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{

    /*
        public static Mana sharedInstance;

        List<GameObject> hearts = new List<GameObject>();

        GameObject m_player;
        public GameObject heart;


        void Awake(){
            sharedInstance = this;
        }

        private void Start() {

            int manaCount = PlayerManager.instance.Mana;
            for(int i = 0; i<manaCount; i++) addCharge(i);

        }

        public void addCharge(int i){
            //int manaCount = m_player.GetComponent<PlayerManager>().GetMana();
            var charge = Instantiate(heart, new Vector3(heart.transform.position.x+i, heart.transform.position.y, heart.transform.position.z), Quaternion.identity, GameObject.FindWithTag("manaUI").transform);
            hearts.Add(charge);
        }

        public void ReduceCharge(int i){
            //Debug.Log("quedann " + i + " cargas");
            hearts[i].transform.GetChild(0).gameObject.SetActive(false);


        }
    */

}
