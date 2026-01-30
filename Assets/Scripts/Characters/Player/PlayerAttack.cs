using UnityEngine;
using System.Collections.Generic;
using nicorueda;

namespace nicorueda.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        // --- INSTANCIA SINGLETON ---
        private static PlayerAttack _instance;
        public static PlayerAttack instance
        {
            get
            {
                if (_instance == null) Debug.LogError("PlayerAttack instance is Null!!!");
                return _instance;
            }
        }

        [Header("Daño")]
        [SerializeField] int meleeDamage = 1;
        [SerializeField] int distanceDamage = 1;
        [SerializeField] int mageDamage;

        [Header("Cooldowns de Ataque")]
        [SerializeField] float meleeAttackRate = 0.5f;
        [SerializeField] float distanceAttackRate = 0.5f;
        [SerializeField] float mageAttackRate = 2f;

        // Timers privados
        private float nextMeleeAttackTime = 0f;
        private float nextDistanceAttackTime = 0f;
        private float nextMageAttackTime = 0f;

        [Header("Proyectiles")]
        [SerializeField] Transform rangePoint;
        [SerializeField] GameObject bulletPrefab;

        [Header("Audio")]
        [SerializeField] private AudioSource ataque_melee;
        [SerializeField] private AudioSource ataque_escupir;

        private PlayerMovement playerMovement;

        // Flag para saber si el hitbox hace daño ahora mismo
        private bool attackingMele;

        // Lista para no golpear al mismo enemigo 2 veces en el mismo ataque
        private List<EnemyManager> enemiesHit = new List<EnemyManager>();

        private void Awake()
        {
            if (_instance != null && _instance != this) Destroy(this.gameObject);
            else _instance = this;

            attackingMele = false;
            playerMovement = GetComponent<PlayerMovement>();
        }

        // --- MÉTODOS PÚBLICOS (Inputs) ---

        public void AttackingAsMelee()
        {
            PlayerMovement.instance.startAttack();
            attackingMele = true;
            enemiesHit.Clear();
            // if(ataque_melee != null) ataque_melee.Play();
            nextMeleeAttackTime = Time.time + meleeAttackRate;
        }

        public void AttackingAsDistanceNoPointing()
        {
            PlayerMovement.instance.startAttack();
            GameObject bullet = Instantiate(bulletPrefab, rangePoint.position, rangePoint.rotation);
            bullet.GetComponent<PlayerProjectile>().setDamage(distanceDamage);
            bullet.GetComponent<PlayerProjectile>().cambiarDireccion(playerMovement.Direction());
            Destroy(bullet, 5f);
            nextDistanceAttackTime = Time.time + distanceAttackRate;
        }

        /// <summary>
        /// EVENTO DE ANIMACIÓN: Se llama al final de la animación de ataque.
        /// </summary>
        public void NoAttack()
        {
            attackingMele = false;
        }

        // --- LÓGICA DE DAÑO FÍSICO ---

        private void OnTriggerStay2D(Collider2D collision)
        {

            if (!attackingMele) return;


            // --- DETECCIÓN DE ENEMIGOS ---
            EnemyManager enemy = collision.GetComponent<EnemyManager>();

            if (enemy != null)
            {
                if (!enemiesHit.Contains(enemy))
                {
                    // --- CORRECCIÓN AQUÍ ---
                    // Pasamos el daño Y la posición del jugador (para el knockback)
                    enemy.TakeDamage(meleeDamage, transform.position);

                    if (PlayerManager.instance != null)
                        PlayerManager.instance.RestoreMana(35);

                    enemiesHit.Add(enemy);
                }
            }
            // --- DETECCIÓN DE OTROS OBJETOS ---
            else if (collision.CompareTag("Boss"))
            {
                collision.GetComponent<Body>().ReduceHealth();
                if (PlayerManager.instance != null) PlayerManager.instance.RestoreMana(35);
            }
            else if (collision.CompareTag("EnemyProjectile"))
            {
                Destroy(collision.gameObject);
            }
        }

        // --- CHEQUEOS DE COOLDOWN ---

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