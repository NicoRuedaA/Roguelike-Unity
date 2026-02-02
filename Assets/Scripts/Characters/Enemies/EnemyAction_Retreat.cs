using UnityEngine;
using nicorueda;

namespace nicorueda
{
    [RequireComponent(typeof(EnemyManager))]
    public class EnemyAction_Retreat : MonoBehaviour
    {
        [Header("Configuración de Retirada")]
        [SerializeField] private float triggerDistance = 3f;
        [SerializeField] private float retreatSpeed = 15f;
        [SerializeField] private float retreatDuration = 0.3f;
        [SerializeField] private float cooldown = 2f;

        [Header("Seguridad de Muros")]
        [Tooltip("Capa de los muros.")]
        [SerializeField] private LayerMask wallLayerMask;
        [Tooltip("Margen de seguridad para evitar colisiones.")]
        [SerializeField] private float safetyBuffer = 0.1f;

        public bool IsRetreating { get; private set; } = false;

        private EnemyManager manager;
        private float durationTimer;
        private float cooldownTimer;
        private Vector2 retreatDirection;
        private Collider2D myCollider;

        private void Awake()
        {
            manager = GetComponent<EnemyManager>();
            myCollider = GetComponent<Collider2D>();
        }

        private void Update()
        {
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
            if (IsRetreating)
            {
                MoveSafely();
            }
        }

        private void MoveSafely()
        {
            // Calcular cuánto nos moveríamos en este frame
            float distanceToMove = retreatSpeed * Time.fixedDeltaTime;
            float checkDistance = distanceToMove + safetyBuffer;

            // Obtener el radio real del collider considerando la escala
            float radius = GetColliderRadius();

            // Lanzar el sensor predictivo
            RaycastHit2D hit = Physics2D.CircleCast(
                transform.position,
                radius,
                retreatDirection,
                checkDistance,
                wallLayerMask
            );

            // Debug visual
            Debug.DrawRay(transform.position, retreatDirection * checkDistance,
                          hit.collider != null ? Color.red : Color.green);

            if (hit.collider != null)
            {
                // Si el obstáculo está muy cerca (dentro del buffer), detenerse completamente
                if (hit.distance < safetyBuffer)
                {
                    manager.Rb.velocity = Vector2.zero;
                    EndRetreat();
                }
                else
                {
                    // Moverse solo hasta donde es seguro
                    float safeDistance = hit.distance - safetyBuffer;
                    float safeSpeed = safeDistance / Time.fixedDeltaTime;
                    manager.Rb.velocity = retreatDirection * Mathf.Min(safeSpeed, retreatSpeed);
                }
            }
            else
            {
                // Vía libre, moverse a velocidad completa
                manager.Rb.velocity = retreatDirection * retreatSpeed;
            }
        }

        public bool ShouldRetreat()
        {
            if (cooldownTimer > 0 || IsRetreating) return false;
            float distance = Vector2.Distance(transform.position, manager.Player.position);
            return distance < triggerDistance;
        }

        public void TryStartRetreat()
        {
            if (!ShouldRetreat()) return;

            // Calcular dirección de retirada
            Vector2 rawDir = (transform.position - manager.Player.position).normalized;

            // Chequeo inicial: ¿Hay espacio para retirarse?
            float radius = GetColliderRadius();
            RaycastHit2D initialCheck = Physics2D.CircleCast(
                transform.position,
                radius,
                rawDir,
                0.5f,
                wallLayerMask
            );

            if (initialCheck.collider != null && initialCheck.distance < 0.1f)
            {
                // Ya estamos pegados a un muro, no iniciar retirada
                return;
            }

            retreatDirection = rawDir;
            IsRetreating = true;
            durationTimer = retreatDuration;
            cooldownTimer = cooldown;
        }

        private void EndRetreat()
        {
            IsRetreating = false;
            manager.Rb.velocity = Vector2.zero;
        }

        private float GetColliderRadius()
        {
            if (myCollider is BoxCollider2D box)
            {
                // Usar el lado más grande del box para mayor seguridad
                float maxSize = Mathf.Max(box.size.x, box.size.y) * 0.5f;
                return maxSize * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
            }
            else if (myCollider is CircleCollider2D circ)
            {
                return circ.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
            }

            // Valor por defecto si no es ni Box ni Circle
            return 0.3f;
        }

        // Salvavidas físico en caso de que la predicción falle
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsRetreating && ((1 << collision.gameObject.layer) & wallLayerMask) != 0)
            {
                manager.Rb.velocity = Vector2.zero;
                EndRetreat();
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (IsRetreating && ((1 << collision.gameObject.layer) & wallLayerMask) != 0)
            {
                manager.Rb.velocity = Vector2.zero;
                EndRetreat();
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Visualizar rango de activación de retirada
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, triggerDistance);

            // Visualizar dirección de retirada si está activa
            if (IsRetreating)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(transform.position, retreatDirection * 2f);
            }
        }
    }
}