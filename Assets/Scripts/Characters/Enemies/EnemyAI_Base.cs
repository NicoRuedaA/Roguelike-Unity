using UnityEngine;
using nicorueda;

namespace nicorueda
{
    [RequireComponent(typeof(EnemyManager))]
    public abstract class EnemyAI_Base : MonoBehaviour
    {
        // --- NUEVO: Variable común para todos ---
        [Header("Sistema de Sueño (Optimización)")]
        [Tooltip("Distancia a la que el enemigo se despierta.")]
        [SerializeField] protected float activationDistance = 15f;

        // ... (Variables de suavizado que ya tenías: acceleration, deceleration...) ...
        [Header("Suavizado de Movimiento")]
        [SerializeField] private float acceleration = 5f;
        [SerializeField] private float deceleration = 10f;

        public bool IsMoving { get; protected set; }
        public bool IsAttacking { get; protected set; }

        protected EnemyManager manager;

        // Variables internas de movimiento (del padre)
        protected Vector2 targetDirection = Vector2.zero;
        protected float targetSpeed = 0f;
        private float currentSpeed = 0f;

        protected virtual void Awake()
        {
            manager = GetComponent<EnemyManager>();
        }

        protected virtual void FixedUpdate()
        {
            if (manager.isDead) return; // (Resumido)

            float distanceToPlayer = Vector2.Distance(transform.position, manager.Player.position);

            // --- DEBUG 1: Ver distancia ---
            // Debug.Log($"Distancia: {distanceToPlayer} / Activación: {activationDistance}");

            if (distanceToPlayer > activationDistance)
            {
                // --- DEBUG 2: Confirmar que entra en modo dormir ---
                Debug.LogWarning("Modo DORMIR activado. Forzando parada.");

                IsAttacking = false;
                IsMoving = false; // <-- Aquí forzamos el false

                targetSpeed = 0f;
                currentSpeed = 0f;
                manager.Rb.velocity = Vector2.zero;

                ResetAI();
                return;
            }

            // ------------------------------------

            // Si está despierto, ejecuta la IA del hijo
            HandleAI();

            // Aplica el movimiento físico (aceleración/frenado suave)
            ApplyMovementPhysics();
        }

        // Separo esto para poder llamarlo también cuando duerme (para frenar suave)
        private void ApplyMovementPhysics()
        {
            float accel = (targetSpeed > currentSpeed) ? acceleration : deceleration;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accel * Time.fixedDeltaTime);

            if (currentSpeed > 0.01f)
            {
                manager.Rb.MovePosition(manager.Rb.position + targetDirection * currentSpeed * Time.fixedDeltaTime);
            }

            IsMoving = currentSpeed > 0.1f;
        }

        public abstract void HandleAI();

        // --- NUEVO MÉTODO VIRTUAL ---
        // Los hijos pueden (opcionalmente) sobrescribir esto para resetear sus estados internos
        protected virtual void ResetAI() { }

        public virtual void OnAttackHit() { }

        // Dibujo del área en el editor
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, activationDistance);
        }
    }
}