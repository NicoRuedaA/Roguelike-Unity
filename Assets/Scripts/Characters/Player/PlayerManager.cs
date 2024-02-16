using System;
using TMPro;
using UnityEngine;

namespace nicorueda.Player
{
    public class PlayerManager : nicorueda.CharacterController
    {
        //SINGLETON DECLARATION
        private static PlayerManager _instance;
        public static PlayerManager instance{
            get{
                if(_instance == null){
                    print("Player Manager is Null!!!");
                }
                return _instance;
            }
        }
        
 

    
        
    
        protected int attDistCost= 25, attMeleeCost=25, runCost = 10;
        public int RunCost { get => runCost; }  public int AttDisCost {get => attDistCost;} public int AttMeleeCost { get => attMeleeCost; }

    //STATE VARIABLES
    protected static bool walking, running, pointing, idle;

    private void Awake()
    {
                    _instance = this;
    }


    private void Start()
        {


            vulnerable = true;
            PrepareCharacter();
        }

        
        /* public TMP_Text counterText;
 private void FixedUpdate()
 {

counterText.text = "HP: " + health + Environment.NewLine +
                     "MAX HP :" + MAX_HEALTH + Environment.NewLine +
                     "STAMINA: " + stamina + Environment.NewLine +
                     "MAX STAMINA :" + MAX_STAMINA +  Environment.NewLine +
                     "CARGAS: " + mana + Environment.NewLine +
                     "MAX CARGAS :" + MAX_MANA + Environment.NewLine +
                     "SPEED: " + speed + Environment.NewLine +
                     "INITIAL SPEED :" + INITIAL_SPEED;

        }*/



        protected override void Die()
        {
            print("murió");
        }
    
        /*public void GodMode()
        {
            health = 9999;
            INITIAL_HEALTH = 9999;
            stamina = 9999;
            INITIAL_STAMINA = 9999;
            mana = 9999;
            INITIAL_MANA = 9999;
        }*/
    }
}
