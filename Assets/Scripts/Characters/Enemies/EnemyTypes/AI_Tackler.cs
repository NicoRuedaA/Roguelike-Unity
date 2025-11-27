using UnityEngine;
using nicorueda;

namespace nicorueda
{
    public class AI_Tackler : EnemyAI_Base
    {
        private enum TackleState
        {
            Idle,       // Esperando a que el jugador entre en rango
            Preparing,  // El jugador entró, cargando el ataque (aviso visual)
            Tackling,   // Corriendo hacia la posición objetivo
            Resting     // Cansado después del placaje
        }

        [Header("Configuración de Placaje")]
        // Nota: 'activationDistance' se hereda de EnemyAI_Base (Sistema de Sueño)

        [Tooltip("Tiempo que espera quieto antes de salir corriendo.")]
        [SerializeField] private float prepareTime = 1f;

        [Tooltip("Velocidad durante el placaje.")]
        [SerializeField] private float tackleSpeed = 10f;

        [Tooltip("Distancia a la que considera que ha 'llegado' al destino.")]
        [SerializeField] private float reachThreshold = 0.5f;

        [Tooltip("Tiempo de descanso después del ataque.")]
        [SerializeField] private float restTime = 2f;

        [Tooltip("Define qué capa es el Muro para detener la carga.")]
        [SerializeField] private LayerMask wallLayerMask;

        [Tooltip("Tiempo inicial donde ignoramos colisiones para poder despegar.")]
        [SerializeField] private float wallIgnoreDuration = 0.2f;
        private float wallIgnoreTimer;

        // Variables internas
        private TackleState currentState = TackleState.Idle;
        private float stateTimer;
        private Vector2 targetPosition; // Destino fijo del placaje

        public override void HandleAI()
        {
            // Calculamos distancia para la activación del ataque
            float distanceToPlayer = Vector2.Distance(transform.position, manager.Player.position);

            switch (currentState)
            {
                // --- ESTADO 1: IDLE ---
                case TackleState.Idle:
                    IsMoving = false;
                    IsAttacking = false;

                    // Si el jugador entra en el rango de activación (variable heredada)...
                    if (distanceToPlayer < activationDistance)
                    {
                        currentState = TackleState.Preparing;
                        stateTimer = prepareTime;
                        FacePlayer();
                    }
                    break;

                // --- ESTADO 2: PREPARANDO ---
                case TackleState.Preparing:
                    IsMoving = false;
                    IsAttacking = true; // Animación de alerta/carga
                    stateTimer -= Time.fixedDeltaTime;

                    FacePlayer(); // Sigue mirando al jugador

                    if (stateTimer <= 0) StartTackle();
                    break;

                // --- ESTADO 3: PLACAJE ---
                case TackleState.Tackling:
                    IsMoving = true;
                    IsAttacking = true;

                    // Reducir temporizador de inmunidad de muros
                    if (wallIgnoreTimer > 0) wallIgnoreTimer -= Time.fixedDeltaTime;

                    // IMPORTANTE: Anulamos la física del Padre para tener control manual total
                    targetSpeed = 0f;
                    targetDirection = Vector2.zero;

                    // Movimiento manual explosivo (Arcade)
                    Vector2 newPos = Vector2.MoveTowards(transform.position, targetPosition, tackleSpeed * Time.fixedDeltaTime);
                    manager.Rb.MovePosition(newPos);

                    // Comprobar si llegamos al destino
                    if (Vector2.Distance(transform.position, targetPosition) < reachThreshold)
                    {
                        EndTackle();
                    }
                    break;

                // --- ESTADO 4: DESCANSANDO ---
                case TackleState.Resting:
                    IsMoving = false;
                    IsAttacking = false;
                    stateTimer -= Time.fixedDeltaTime;

                    if (stateTimer <= 0) currentState = TackleState.Idle;
                    break;
            }
        }

        // --- Funciones Auxiliares ---

        private void StartTackle()
        {
            currentState = TackleState.Tackling;
            wallIgnoreTimer = wallIgnoreDuration; // Iniciar inmunidad

            targetPosition = manager.Player.position;
            // Extender el destino para intentar atravesar al jugador
            Vector2 dir = (targetPosition - (Vector2)transform.position).normalized;
            targetPosition += dir * 2f;

            manager.Attack.PerformAttack(); // Activar Hitbox
        }

        private void EndTackle()
        {
            currentState = TackleState.Resting;
            stateTimer = restTime;
            manager.Attack.StopTackleAttack(); // Desactivar Hitbox
        }

        private void FacePlayer()
        {
            if (manager.Player.position.x > transform.position.x)
                manager.SpriteRend.flipX = true;
            else
                manager.SpriteRend.flipX = false;
        }

        // --- Eventos y Colisiones ---

        // Si el hitbox golpea al jugador, paramos
        public override void OnAttackHit()
        {
            if (currentState == TackleState.Tackling)
            {
                Debug.Log("¡Impacto confirmado! Frenando placaje.");
                EndTackle();
            }
        }

        // Si nos alejamos mucho (lógica del padre), reseteamos todo
        protected override void ResetAI()
        {
            if (currentState != TackleState.Idle)
            {
                currentState = TackleState.Idle;
                wallIgnoreTimer = 0;
                manager.Attack.StopTackleAttack();
            }
        }

        // Detección de muros
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (currentState != TackleState.Tackling) return;
            if (wallIgnoreTimer > 0) return; // Ignorar muros al arrancar

            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Debug.Log("¡Muro detectado! Parando placaje.");
                EndTackle();
            }
        }

        // --- Debug Visual ---
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected(); // Dibuja el círculo de activación blanco

            if (currentState == TackleState.Tackling)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, targetPosition);
                Gizmos.DrawWireSphere(targetPosition, 0.5f);
            }
        }
    }
}