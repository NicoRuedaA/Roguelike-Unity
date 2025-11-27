using UnityEngine;
using nicorueda;

namespace nicorueda
{
    /// <summary>
    /// Componente modular que permite a un enemigo realizar una retirada rápida (dash hacia atrás)
    /// cuando el jugador está demasiado cerca.
    /// </summary>
    [RequireComponent(typeof(EnemyManager))]
    public class EnemyAction_Retreat : MonoBehaviour
    {
        [Header("Configuración de Retirada")]
        [Tooltip("Distancia mínima. Si el jugador está más cerca, se activa la retirada.")]
        [SerializeField] private float triggerDistance = 3f;

        [Tooltip("Velocidad del dash hacia atrás (debe ser alta).")]
        [SerializeField] private float retreatSpeed = 15f;

        [Tooltip("Cuánto dura el impulso de retirada (en segundos).")]
        [SerializeField] private float retreatDuration = 0.3f;

        [Tooltip("Tiempo de espera antes de poder retirarse de nuevo.")]
        [SerializeField] private float cooldown = 2f;

        // Estado público para que el Cerebro principal sepa si estamos ocupados
        public bool IsRetreating { get; private set; } = false;

        // Referencias y timers internos
        private EnemyManager manager;
        private float durationTimer;
        private float cooldownTimer;
        private Vector2 retreatDirection;

        private void Awake()
        {
            manager = GetComponent<EnemyManager>();
        }

        private void Update()
        {
            // Manejo de timers
            if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;

            if (IsRetreating)
            {
                durationTimer -= Time.deltaTime;
                if (durationTimer <= 0)
                {
                    EndRetreat();
                }
            }
        }

        private void FixedUpdate()
        {
            // Si estamos en medio de una retirada, tomamos el control total de la física
            // y aplicamos velocidad directa para un movimiento explosivo.
            if (IsRetreating)
            {
                manager.Rb.velocity = retreatDirection * retreatSpeed;
            }
            // NOTA: No tocamos la velocidad si no estamos retirándonos, 
            // dejando que el script principal (AI_Archer) la controle.
        }

        /// <summary>
        /// Comprueba si se dan las condiciones para retirarse (Distancia y Cooldown).
        /// </summary>
        public bool ShouldRetreat()
        {
            if (cooldownTimer > 0 || IsRetreating) return false;

            float distance = Vector2.Distance(transform.position, manager.Player.position);
            return distance < triggerDistance;
        }

        /// <summary>
        /// Intenta iniciar la acción de retirada.
        /// </summary>
        public void TryStartRetreat()
        {
            if (!ShouldRetreat()) return;

            IsRetreating = true;
            durationTimer = retreatDuration;
            cooldownTimer = cooldown;

            // Calculamos la dirección opuesta al jugador
            retreatDirection = (transform.position - manager.Player.position).normalized;

            // Opcional: Activar una animación de "dash" o "salto atrás" si la tienes
            // manager.Anim.SetTrigger("Retreat");
        }

        private void EndRetreat()
        {
            IsRetreating = false;
            // Frenamos en seco al terminar el dash para recuperar control inmediatamente
            manager.Rb.velocity = Vector2.zero;
        }

        // --- GIZMOS ---
        private void OnDrawGizmosSelected()
        {
            // Dibujamos la zona de peligro en un color chillón (ej. Cian o Naranja)
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, triggerDistance);
        }
    }
}