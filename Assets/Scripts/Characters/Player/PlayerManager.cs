using System;
using System.Collections;
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


        public override bool ReduceHealth(int amountToReduce)
        {
            bool tookDamage = base.ReduceHealth(amountToReduce);

            // 2. Si el padre nos confirma que se recibió daño (true)...
            if (tookDamage)
            {
                // 3. Solo entonces actualizamos la interfaz visual
                if (HealthManager.instance != null)
                {
                    for (int i = 0; i < amountToReduce; i++)
                    {
                        HealthManager.instance.RemoveLife();
                    }
                }
            }

            // Retornamos lo que diga la base
            return tookDamage;
        }



        protected override void Die()
        {
            print("murió");
            GameManager.instance.RestartGame();
        }



    }
}
