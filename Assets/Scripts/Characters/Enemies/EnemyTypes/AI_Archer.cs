using UnityEngine;
using nicorueda;

namespace nicorueda
{
    // Este atributo asegura que no se te olvide poner el script de retirada
    [RequireComponent(typeof(EnemyAction_Retreat))]
    public class AI_Archer : EnemyAI_Base
    {
        [Header("Comportamiento de Disparo")]
        [Tooltip("Distancia a la que se detiene para empezar a disparar.")]
        [SerializeField] private float shootingRange = 6f;

        [Tooltip("Referencia al punto de disparo para rotarlo hacia el jugador.")]
        [SerializeField] private Transform shootPoint;

        [Header("Evasión de Muros (Movimiento)")]
        [SerializeField] private LayerMask wallLayerMask;
        [SerializeField] private float obstacleCheckDistance = 1f;
        [SerializeField] private float safetyRayDistance = 0.5f;
        [SerializeField] private float retreatSpeed = 2f; // Velocidad al caminar hacia atrás (no el dash)

        // Referencia al componente modular de retirada
        private EnemyAction_Retreat retreatAction;

        protected override void Awake()
        {
            base.Awake();
            retreatAction = GetComponent<EnemyAction_Retreat>();
        }

        public override void HandleAI()
        {
            // --- PRIORIDAD 1: ¿ESTAMOS HACIENDO UN DASH DE RETIRADA? ---
            // Si el módulo de retirada está activo, él controla las físicas (velocity).
            // Nosotros nos quedamos quietos lógicamente.
            if (retreatAction.IsRetreating)
            {
                IsMoving = false;
                IsAttacking = false;
                // Anulamos la física del padre para que no interfiera con el dash
                targetSpeed = 0f;
                targetDirection = Vector2.zero;
                return; // SALIR INMEDIATAMENTE
            }

            // --- PRIORIDAD 2: ¿DEBERÍA RETIRARME AHORA? ---
            // Le preguntamos al módulo si el jugador está demasiado cerca
            if (retreatAction.ShouldRetreat())
            {
                retreatAction.TryStartRetreat();
                return; // SALIR, el módulo manejará el movimiento en el siguiente frame
            }

            // --- PRIORIDAD 3: LÓGICA NORMAL (Disparar o Perseguir) ---

            float distance = Vector2.Distance(transform.position, manager.Player.position);
            Vector2 directionToPlayer = (manager.Player.position - transform.position).normalized;

            // 1. Apuntar (Rotar el arma)
            if (shootPoint != null)
            {
                Vector2 diff = manager.Player.position - shootPoint.position;
                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                shootPoint.rotation = Quaternion.Euler(0f, 0f, rot_z);
            }

            // 2. Decidir Movimiento
            if (distance <= shootingRange)
            {
                // ESTADO: EN RANGO -> QUIETOS Y DISPARAR

                // Frenamos el movimiento
                targetDirection = Vector2.zero;
                targetSpeed = 0f;

                // Como targetSpeed es 0, IsMoving será false (gestionado por el padre),
                // pero por seguridad lógica:
                IsMoving = false;

                // Disparar
                if (manager.Attack.CanAttack())
                {
                    IsAttacking = true;
                    manager.Attack.PerformAttack();
                }
                else
                {
                    IsAttacking = false;
                }
            }
            else
            {
                // ESTADO: LEJOS -> PERSEGUIR
                IsAttacking = false;

                // Usamos la lógica de movimiento inteligente
                MoveToPlayerWithAvoidance(directionToPlayer);
            }
        }

        // Lógica de movimiento con "Bigotes" y "Escudo" (Copiada y adaptada del Enano)
        private void MoveToPlayerWithAvoidance(Vector2 directionToPlayer)
        {
            Vector2 retreatDir = Vector2.zero;
            bool isInsideWall = false;
            Vector2[] safetyRayDirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            // 1. Escudo de Seguridad
            foreach (Vector2 dir in safetyRayDirs)
            {
                if (Physics2D.Raycast(transform.position, dir, safetyRayDistance, wallLayerMask))
                {
                    isInsideWall = true;
                    retreatDir -= dir;
                }
            }

            // 2. Bigote Frontal
            RaycastHit2D hitForward = Physics2D.Raycast(transform.position, directionToPlayer, obstacleCheckDistance, wallLayerMask);

            // 3. Decisión
            if (isInsideWall && retreatDir != Vector2.zero)
            {
                // Despegarse del muro suavemente
                targetDirection = retreatDir.normalized;
                targetSpeed = retreatSpeed;
            }
            else if (hitForward.collider == null)
            {
                // Camino libre hacia el jugador
                targetDirection = directionToPlayer;
                targetSpeed = manager.Speed;
            }
            else
            {
                // Deslizarse por el muro
                // Usamos Vector3 para evitar el error de compilación
                Vector2 slideDirection = Vector3.ProjectOnPlane(directionToPlayer, hitForward.normal).normalized;

                targetDirection = slideDirection;
                targetSpeed = manager.Speed;
            }
        }

        // Dibujamos los Gizmos para ver los rangos en el editor
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected(); // Dibuja el círculo blanco (Dormir)

            // Círculo Rojo: Rango de Disparo
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, shootingRange);
        }
    }
}