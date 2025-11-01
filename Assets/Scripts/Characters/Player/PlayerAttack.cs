using UnityEngine;
using UnityEngine.Serialization;

namespace nicorueda.Player
{
    // 1. HERENCIA CORREGIDA: Ya no hereda de PlayerManager
    public class PlayerAttack : MonoBehaviour
    {
        // --- INSTANCIA SINGLETON (Corregida) ---
        private static PlayerAttack _instance;
        public static PlayerAttack instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("PlayerAttack instance is Null!!!");
                }
                return _instance;
            }
        }

        [Header("Daño")]
        [SerializeField] int meleeDamage; // 'meleeDamage' no se usa, pero lo dejamos
        [SerializeField] int distanceDamage;
        [SerializeField] int mageDamage;

        [Header("Cooldowns de Ataque")]
        // 2. COOLDOWNS CENTRALIZADOS: Un rate por cada ataque
        [SerializeField] float meleeAttackRate = 1f;
        [SerializeField] float distanceAttackRate = 0.5f;
        [SerializeField] float mageAttackRate = 2f;

        // Timers privados para los cooldowns
        private float nextMeleeAttackTime = 0f;
        private float nextDistanceAttackTime = 0f;
        private float nextMageAttackTime = 0f;

        [Header("Proyectiles")]
        [SerializeField] float bulletDistanceForce = 10f;
        [SerializeField] float bulletMageForce = 20f;
        [SerializeField] Transform rangePoint;
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] Transform spellPoint;
        [SerializeField] GameObject spellPrefab;

        [Header("Audio")]
        [SerializeField] private AudioSource ataque_melee;
        [SerializeField] private AudioSource ataque_escupir;

        // Flag para el ataque melee
        private bool attackingMele;


        // --- AWAKE (Corregido) ---
        private void Awake()
        {
            // Lógica de Singleton robusta
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning("Instancia duplicada de PlayerAttack destruida.");
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            attackingMele = false;
        }

        // --- UPDATE (Eliminado) ---
        // 3. El 'Update' que gestionaba 'canAttack' era innecesario y confuso.
        // Lo hemos quitado. Comprobaremos los timers directamente.


        // --- MÉTODOS PÚBLICOS (Llamados por InputManager) ---

        public void AttackingAsMelee()
        {
            // El InputManager ya ha comprobado estamina y cooldown
            ataque_melee.Play();
            attackingMele = true; // Activamos el trigger de daño

            // 4. APLICAMOS COOLDOWN
            nextMeleeAttackTime = Time.time + meleeAttackRate;
        }

        public void AttackingAsDistance()
        {
            // El InputManager ya ha comprobado estamina y cooldown
            ataque_escupir.Play();
            GameObject spell = Instantiate(spellPrefab, spellPoint.position, spellPoint.rotation);
            Rigidbody2D rb = spell.GetComponent<Rigidbody2D>();

            // Usamos la dirección del Sprite (que está en PlayerMovement)
            rb.AddForce(new Vector2(bulletMageForce * PlayerMovement.instance.Direction(), 0), ForceMode2D.Impulse);

            // 4. APLICAMOS COOLDOWN
            nextDistanceAttackTime = Time.time + distanceAttackRate;
        }

        // 5. TYPO CORREGIDO: "AttackinAsMage" -> "AttackingAsMage"
        public void AttackingAsMage()
        {
            // El InputManager ya ha comprobado estamina y cooldown
            Vector2 pointTo = PlayerMovement.instance.ReturnMove(); // Dirección del joystick
            if (pointTo.magnitude == 0)
            {
                // Si el joystick está quieto, disparamos hacia donde mira el jugador
                pointTo = new Vector2(PlayerMovement.instance.Direction(), 0);
            }

            pointTo.Normalize();

            GameObject bullet = Instantiate(bulletPrefab, rangePoint.position, rangePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(pointTo * bulletDistanceForce, ForceMode2D.Impulse);

            // 4. APLICAMOS COOLDOWN
            nextMageAttackTime = Time.time + mageAttackRate;
        }

        /// <summary>
        /// Este método es llamado por un EVENTO DE ANIMACIÓN
        /// al final de la animación de ataque melee.
        /// </summary>
        public void NoAttack()
        {
            attackingMele = false;
        }


        // --- LÓGICA DE DAÑO MELEE (Corregida) ---

        private void OnTriggerStay2D(Collider2D enemies)
        {
            // Solo hacemos daño si la bandera 'attackingMele' está activa
            if (attackingMele)
            {
                // 6. LÓGICA CORREGIDA: 'attackingMele = false;' SE HA QUITADO DE AQUÍ.

                if (enemies.CompareTag("Enemy"))
                {
                    Debug.Log("le hiso danyo wwwwwwwwwwwwwy");
                    enemies.gameObject.GetComponent<EnemyController2>().TakeDamage(1, this.transform.position);
                    PlayerManager.instance.RestoreMana(35); // Restauramos maná al golpear
                }
                if (enemies.CompareTag("BossBody"))
                {
                    enemies.GetComponent<Body>().ReduceHealth();
                    PlayerManager.instance.RestoreMana(35);
                }
                if (enemies.CompareTag("Sword"))
                {
                    enemies.GetComponent<Sword>().TakeDamage(1, this.transform.position);
                    PlayerManager.instance.RestoreMana(35);
                }
                else if (enemies.CompareTag("EnemyProjectile"))
                {
                    Destroy(enemies.gameObject); // Destruye proyectiles, pero no da maná
                }
            }
        }

        // --- MÉTODOS PÚBLICOS PARA EL INPUTMANAGER ---
        // 7. Creamos métodos "IsReady" para que el InputManager pueda preguntar

        public bool IsMeleeReady()
        {
            return Time.time > nextMeleeAttackTime;
        }

        public bool IsDistanceReady()
        {
            return Time.time > nextDistanceAttackTime;
        }

        public bool IsMageReady()
        {
            return Time.time > nextMageAttackTime;
        }
    }
}