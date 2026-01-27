using System;
using TMPro;
using UnityEngine;

namespace nicorueda
{
    public class PlayerManager : nicorueda.CharacterBase
    {
        //SINGLETON DECLARATION
        public static PlayerManager instance { get; private set; }






        protected int attDistCost = 25, attMeleeCost = 25, runCost = 10;
        public int RunCost { get => runCost; }
        public int AttDisCost { get => attDistCost; }
        public int AttMeleeCost { get => attMeleeCost; }

        //STATE VARIABLES
        protected static bool walking, running, pointing, idle;
        protected override void Awake()
        {
            // 1. IMPORTANTE: Esto ejecuta SetStats() de CharacterBase (el 5 de vida)
            base.Awake();

            // 2. Configuración del Singleton corregida
            if (instance == null)
            {
                instance = this;
                // Opcional: DontDestroyOnLoad(gameObject); si quieres que persista entre niveles
            }
            else
            {
                Destroy(gameObject);
            }
        }


        private void Start()
        {


            isVulnerable = true;
        }





        protected override void Die()
        {
            print("murió");
        }

    }
}
