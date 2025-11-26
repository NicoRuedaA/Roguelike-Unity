using UnityEngine;

namespace nicorueda
{
    public class AI_FollowMelee : EnemyAI_Base
    {
        [Header("Activación (Modo Dormir)")]
        [Tooltip("Si el jugador está más lejos que esto, el enemigo se duerme.")]
        [SerializeField] private float activationDistance = 10f;

        [Header("Comportamiento")]
        [Tooltip("Distancia a la que el enemigo deja de seguir y empieza a atacar.")]
        [SerializeField] private float stoppingDistance = 1.5f;

        [Header("Evasión de Muros")]
        [Tooltip("Asigna la capa (Layer) que usan tus muros.")]
        [SerializeField] private LayerMask wallLayerMask;

        [Tooltip("Qué tan lejos 've' el enemigo hacia adelante.")]
        [SerializeField] private float obstacleCheckDistance = 1f;

        [Tooltip("Distancia de los rayos del 'escudo de seguridad'.")]
        [SerializeField] private float safetyRayDistance = 0.5f;

        [Tooltip("Velocidad a la que retrocede el enemigo.")]
        [SerializeField] private float retreatSpeed = 2f;

        public override void HandleAI()
        {
            // 1. Calcular distancia al jugador
            float distance = Vector2.Distance(transform.position, manager.Player.position);

            // --- NUEVA LÓGICA: MODO DORMIR ---
            // Si el jugador está fuera del área de activación...
            if (distance > activationDistance)
            {
                IsMoving = false;    // Deja de caminar
                IsAttacking = false; // Deja de atacar
                return;              // ¡SALIMOS DE LA FUNCIÓN AQUÍ! No calcula nada más.
            }

            // ---------------------------------------------------------
            // Si llegamos aquí, el jugador está cerca (DESPIERTO)
            // ---------------------------------------------------------

            Vector2 directionToPlayer = (manager.Player.position - transform.position).normalized;

            // 2. Lógica de Decisión de Ataque
            if (distance <= stoppingDistance)
            {
                IsMoving = false;

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
                // --- ESTAMOS LEJOS (Lógica de Movimiento) ---
                IsAttacking = false;

                // --- Paso A: Escudo de Seguridad ---
                Vector2 retreatDirection = Vector2.zero;
                bool isInsideWall = false;
                Vector2[] safetyRayDirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

                foreach (Vector2 dir in safetyRayDirs)
                {
                    RaycastHit2D safetyHit = Physics2D.Raycast(transform.position, dir, safetyRayDistance, wallLayerMask);

                    // Solo dibujamos rayos si estamos despiertos
                    Debug.DrawRay(transform.position, dir * safetyRayDistance, Color.red);

                    if (safetyHit.collider != null)
                    {
                        isInsideWall = true;
                        retreatDirection -= dir;
                    }
                }

                // --- Paso B: Rayo Frontal ---
                RaycastHit2D hitForward = Physics2D.Raycast(transform.position, directionToPlayer, obstacleCheckDistance, wallLayerMask);
                Debug.DrawRay(transform.position, directionToPlayer * obstacleCheckDistance, Color.green);

                // --- Paso C: Decidir Movimiento ---
                if (isInsideWall && retreatDirection != Vector2.zero)
                {
                    // RETROCEDER
                    IsMoving = true;
                    retreatDirection.Normalize();
                    Vector2 newPos = manager.Rb.position + retreatDirection * retreatSpeed * Time.fixedDeltaTime;
                    manager.Rb.MovePosition(newPos);
                }
                else if (hitForward.collider == null)
                {
                    // AVANZAR DIRECTO
                    IsMoving = true;
                    Vector2 newPos = manager.Rb.position + (Vector2)directionToPlayer * manager.Speed * Time.fixedDeltaTime;
                    manager.Rb.MovePosition(newPos);
                }
                else
                {
                    // DESLIZARSE POR MURO
                    IsMoving = true;
                    Vector2 slideDirection = Vector3.ProjectOnPlane(directionToPlayer, hitForward.normal).normalized;
                    Debug.DrawRay(transform.position, slideDirection * manager.Speed, Color.cyan);

                    Vector2 newPos = manager.Rb.position + slideDirection * manager.Speed * Time.fixedDeltaTime;
                    manager.Rb.MovePosition(newPos);
                }
            }
        }

        // --- AYUDA VISUAL ---
        // Esto dibuja un círculo blanco en el editor para ver el rango de activación
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, activationDistance);
        }
    }
}