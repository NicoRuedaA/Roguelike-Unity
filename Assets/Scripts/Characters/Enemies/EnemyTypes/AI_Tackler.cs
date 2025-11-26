using UnityEngine;
using nicorueda;

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
    [Tooltip("Distancia para despertar al enemigo.")]
    [SerializeField] private float activationDistance = 8f;

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

    // Variables internas
    private TackleState currentState = TackleState.Idle;
    private float stateTimer;
    private Vector2 targetPosition; // Dónde estaba el jugador cuando empezó la carga

    public override void HandleAI()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, manager.Player.position);

        switch (currentState)
        {
            // --- ESTADO 1: IDLE (Dormido / Vigilando) ---
            case TackleState.Idle:
                IsMoving = false;
                IsAttacking = false;

                // Si el jugador entra en el rango...
                if (distanceToPlayer < activationDistance)
                {
                    // ¡Empezar a preparar el ataque!
                    currentState = TackleState.Preparing;
                    stateTimer = prepareTime;

                    // Opcional: Orientarse hacia el jugador antes de preparar
                    FacePlayer();
                }
                break;

            // --- ESTADO 2: PREPARANDO (Cargando impulso) ---
            case TackleState.Preparing:
                IsMoving = false;
                IsAttacking = true; // Ponemos animación de ataque/alerta

                stateTimer -= Time.fixedDeltaTime;

                // Miramos al jugador mientras preparamos
                FacePlayer();

                if (stateTimer <= 0)
                {
                    // ¡Lanzar el placaje!
                    StartTackle();
                }
                break;

            // --- ESTADO 3: PLACAJE (Corriendo y dañando) ---
            case TackleState.Tackling:
                IsMoving = true;
                IsAttacking = true;

                // Moverse hacia el punto objetivo (NO hacia el jugador en tiempo real)
                // Esto hace que el jugador pueda esquivarlo.
                Vector2 newPos = Vector2.MoveTowards(transform.position, targetPosition, tackleSpeed * Time.fixedDeltaTime);
                manager.Rb.MovePosition(newPos);

                // Comprobar si hemos llegado al destino
                if (Vector2.Distance(transform.position, targetPosition) < reachThreshold)
                {
                    // Hemos llegado (golpeemos o no)
                    EndTackle();
                }
                break;

            // --- ESTADO 4: DESCANSANDO (Cooldown) ---
            case TackleState.Resting:
                IsMoving = false;
                IsAttacking = false;

                stateTimer -= Time.fixedDeltaTime;

                if (stateTimer <= 0)
                {
                    // Volver a vigilar
                    currentState = TackleState.Idle;
                }
                break;
        }
    }

    // --- Funciones Auxiliares ---

    private void StartTackle()
    {
        currentState = TackleState.Tackling;

        // 1. Guardamos la posición ACTUAL del jugador (para ir en línea recta)
        targetPosition = manager.Player.position;

        // 2. Extendemos un poco el destino para que intente "traspasar" al jugador
        // Esto evita que se pare justo delante de sus narices.
        Vector2 dir = (targetPosition - (Vector2)transform.position).normalized;
        targetPosition += dir * 2f;

        // 3. Activamos el Hitbox de daño (¡IMPORTANTE!)
        manager.Attack.PerformAttack(); // Esto activa el collider en EnemyAttack
    }

    private void EndTackle()
    {
        currentState = TackleState.Resting;
        stateTimer = restTime;

        // Desactivamos el Hitbox de daño
        manager.Attack.StopTackleAttack();
    }

    private void FacePlayer()
    {
        // Lógica simple para flipear el sprite (aunque EnemyAnimation ya lo suele hacer)
        if (manager.Player.position.x > transform.position.x)
            manager.SpriteRend.flipX = true;
        else
            manager.SpriteRend.flipX = false;
    }

    // Ayuda visual en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, activationDistance);

        if (currentState == TackleState.Tackling)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, targetPosition);
            Gizmos.DrawWireSphere(targetPosition, 0.5f);
        }
    }

    public override void OnAttackHit()
    {
        // Solo nos importa si estamos en medio de un placaje
        if (currentState == TackleState.Tackling)
        {
            Debug.Log("¡Impacto confirmado! Frenando placaje.");
            EndTackle(); // Llamamos a la función que ya tenías para pasar a 'Resting'
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Solo nos importa si estamos corriendo (Tackling)
        if (currentState != TackleState.Tackling) return;

        // 2. Comprobamos si el objeto con el que chocamos está en la capa 'Wall'
        // (Esta es la forma matemática de comprobar si una Layer está dentro de una LayerMask)
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Debug.Log("¡El Espartano se chocó contra un muro!");
            EndTackle();
        }
    }

}