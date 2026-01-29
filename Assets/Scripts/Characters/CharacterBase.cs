using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


namespace nicorueda
{
    public abstract class CharacterBase : MonoBehaviour
    {
        // --- ESTADÍSTICAS ---
        // CAMBIO: Todas las estadísticas ahora son privadas.
        // Se exponen públicamente usando propiedades de solo lectura (get)
        // o con 'private set' para que solo esta clase pueda modificarlas.

        [Header("Valores Iniciales")]
        [SerializeField] private int INITIAL_HEALTH = 5;
        [SerializeField] private int INITIAL_SPEED = 3;
        [SerializeField] private int INITIAL_RUNSPEED = 8;
        [SerializeField] private int INITIAL_MANA = 4;
        [SerializeField] private int INITIAL_STAMINA = 150;
        [SerializeField] protected float invulnerabilityTime = 1f;

        // --- Propiedades Públicas (La forma correcta) ---
        public int MaxHealth { get; private set; }
        public int Health { get; private set; }

        // CAMBIO: Uso 'float' para estamina y maná.
        // Esto permite regeneración gradual (ej. +10 * Time.deltaTime).
        public float MaxStamina { get; private set; }
        public float Stamina { get; private set; }

        public float MaxMana { get; private set; }
        public float Mana { get; private set; }

        public int Speed { get; private set; }
        public int RunSpeed { get; private set; }

        // --- Banderas de Estado (protected para que los hijos las usen) ---
        public bool isDead { get; protected set; } = false;
        protected bool isVulnerable = true;

        // CAMBIO: Ya no se necesitan las propiedades 'get/set' del final.
        // CAMBIO: Ya no se necesitan 'MIN_MANA', 'MIN_HEALTH', etc. Es siempre 0.

        // --- INICIALIZACIÓN ---

        // CAMBIO: Se usa 'Awake' para garantizar la inicialización.
        // Es 'virtual' para que las clases hijas puedan añadir su propia lógica 'Awake'.
        protected virtual void Awake()
        {
            SetStats();
        }

        private void SetStats()
        {
            MaxHealth = INITIAL_HEALTH;
            MaxMana = INITIAL_MANA;
            MaxStamina = INITIAL_STAMINA;

            Health = MaxHealth;
            Mana = MaxMana;
            Stamina = MaxStamina;
            Speed = INITIAL_SPEED;
            RunSpeed = INITIAL_RUNSPEED;

            isDead = false;
            Debug.Log("vida inicial: " + Health);
        }

        // --- LÓGICA DE COMBATE ---

        // CAMBIO: Esta es la nueva función de daño.
        /// <summary>
        /// Aplica daño a este personaje.
        /// </summary>
        /// <param name="damageAmount">La cantidad de vida a perder.</param>
        /// <param name="damageSourcePosition">La posición de quien hizo el daño (para knockback).</param>
        /// 
        /// 
        public virtual void TakeDamage(int damageAmount, Vector3 damageSourcePosition)
        {
            // Solo comprobamos si está muerto o es invulnerable
            if (isDead || !isVulnerable) return;

            // Llamamos a ReduceHealth. Él se encargará de restar, loguear y chequear muerte.
            bool tookDamage = ReduceHealth(damageAmount);

            if (tookDamage)
            {
                ApplyKnockback(damageSourcePosition);
                // Aquí podrías disparar el sonido de daño: hurt_sound.Play();
            }
        }

        // CAMBIO: Función de Knockback separada.
        // Es 'virtual' para que el Jugador o el Enemigo puedan desactivarla.
        protected virtual void ApplyKnockback(Vector3 damageSourcePosition)
        {
            // Mueve al personaje 0.5 unidades lejos de la fuente de daño
            if (damageSourcePosition.x > transform.position.x)
            {
                transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
            }

            // TODO: Un 'AddForce' al Rigidbody sería mucho mejor que teletransportar
        }

        protected abstract void Die();


        public virtual bool ReduceHealth(int amountToReduce)
        {
            // 1. Si no es vulnerable o ya está muerto, salimos
            if (!isVulnerable || Health <= 0) return false;

            Debug.Log("Vida pre: " + Health);

            // 2. Restamos vida (usamos Mathf.Max para no bajar de 0)
            Health -= amountToReduce;
            Health = Mathf.Max(Health, 0);

            Debug.Log("Vida post: " + Health);

            // 3. Activamos el delay de invulnerabilidad para todos
            StartCoroutine(InvulnerabilityCooldown());

            // 4. Si la vida llega a 0, ejecutamos la muerte
            if (Health <= 0)
            {
                Die();
            }

            return true; // Indicamos que el daño fue procesado con éxito
        }

        private IEnumerator InvulnerabilityCooldown()
        {
            isVulnerable = false;
            yield return new WaitForSeconds(invulnerabilityTime);
            isVulnerable = true;
        }

        public bool ReduceMana(float amountToReduce)
        {
            if (Mana >= amountToReduce)
            {
                Mana -= amountToReduce;
                return true;
            }
            return false;
        }

        public bool ReduceStamina(float amountToReduce)
        {
            if (Stamina >= amountToReduce)
            {
                Stamina -= amountToReduce;
                return true;
            }
            return false;
        }

        protected void SetSpeed(int newSpeed)
        {
            // Aquí puedes añadir lógica de seguridad
            if (newSpeed < 0)
            {
                newSpeed = 0;
            }

            // Esta clase SÍ PUEDE escribir en 'Speed'
            Speed = newSpeed;
        }


        public void RestoreMana(float amount)
        {
            Mana = Mathf.Min(Mana + amount, MaxMana);
        }

        public void RestoreStamina(float amount)
        {
            Stamina = Mathf.Min(Stamina + amount, MaxStamina);
        }

        public void RestoreHealth(int amount)
        {
            Health = Mathf.Min(Health + amount, MaxHealth);
        }

        // --- GESTIÓN DE MEJORAS ---
        public void IncreaseHealthMax(int amount)
        {
            MaxHealth += amount;
            RestoreHealth(amount); // Cura al jugador por la misma cantidad
        }

        public void IncreaseManaMax(float amount)
        {
            MaxMana += amount;
            RestoreMana(amount);
        }

        public void IncreaseStaminaMax(float amount)
        {
            MaxStamina += amount;
            RestoreStamina(amount);
        }
    }
}