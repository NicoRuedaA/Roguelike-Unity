using UnityEngine;
using nicorueda;

public class AI_FollowMelee : EnemyAI_Base
{
    // --- BORRADO: [Header("Activación")] y la variable activationDistance ---
    // Ya se heredan de EnemyAI_Base.

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
        // NOTA: Ya no comprobamos 'activationDistance' aquí.
        // El Padre (EnemyAI_Base) ya lo ha hecho. Si estamos aquí, estamos despiertos.

        // 1. Calcular distancia y dirección al jugador
        float distance = Vector2.Distance(transform.position, manager.Player.position);
        Vector2 directionToPlayer = (manager.Player.position - transform.position).normalized;

        // 2. Lógica de Decisión de Ataque
        if (distance <= stoppingDistance)
        {
            // --- ESTAMOS EN RANGO DE ATAQUE ---
            targetDirection = Vector2.zero; // Paramos
            targetSpeed = 0f;

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

            // --- Paso C: Decidir Movimiento (Establecer Objetivos para el Padre) ---
            if (isInsideWall && retreatDirection != Vector2.zero)
            {
                // RETROCEDER
                targetDirection = retreatDirection.normalized;
                targetSpeed = retreatSpeed;
            }
            else if (hitForward.collider == null)
            {
                // AVANZAR DIRECTO
                targetDirection = directionToPlayer;
                targetSpeed = manager.Speed;
            }
            else
            {
                // DESLIZARSE POR MURO
                Vector2 slideDirection = Vector3.ProjectOnPlane(directionToPlayer, hitForward.normal).normalized;
                Debug.DrawRay(transform.position, slideDirection * manager.Speed, Color.cyan);

                targetDirection = slideDirection;
                targetSpeed = manager.Speed;
            }
        }
    }

    // No necesitamos ResetAI() ni OnAttackHit() para este enemigo simple,
    // así que no los sobrescribimos.
}