using System.Collections;
using System.Collections.Generic;
using nicorueda.Player;
using UnityEngine;
using UnityEngine.UI;




public class Bar : MonoBehaviour
{
    
    private enum BarType{
    health,
    energy,
    mana
    }


    [SerializeField] BarType m_type = BarType.health;
    [SerializeField] PlayerManager player;
    Image bar;


    float value, maxValue;

    RectTransform rt;
    


    private void Start() {

        bar=GetComponent<Image>();  
        rt = GetComponent<RectTransform>();

        /*if(m_type == BarType.health){
            maxValue = player.GetMaxHealth();
        }
        else if(m_type == BarType.energy){
            maxValue = player.GetMaxStamina();
        }
        else maxValue = player.GetMaxMana();

        
        
        Debug.Log(maxValue);*/
        value = maxValue;
    }

    void Update()
    {/*

        if(m_type == BarType.health){
            value = player.GetHealth();
            maxValue = player.GetMaxHealth();
        }
        else if(m_type == BarType.energy){
            value = player.GetStamina();
            maxValue = player.GetMaxStamina();
        }
        else {
            value = player.GetMana();
            maxValue = player.GetMaxMana();
        }

        rt.sizeDelta = new Vector2(maxValue, 15); 
        bar.fillAmount = value/maxValue;*/  
    }

    
}
