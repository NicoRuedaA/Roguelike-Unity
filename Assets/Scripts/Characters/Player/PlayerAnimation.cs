using UnityEngine;

namespace nicorueda.Player
{
    public class PlayerAnimation : PlayerManager
    {
        // --- INSTANCIA SINGLETON (Corregida) ---
        private static PlayerAnimation _instance;
        public new static PlayerAnimation instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("Animation Manager is Null!!!");
                }
                return _instance;
            }
        }

        public enum AnimationState
        {
            Idle,
            Walking,
            Running,
            AttackMelee,
            AttackDistance,
            Hurt,
            Sprint,
            Pointing,
        }

        // --- ESTADO (Corregido) ---
        // 'static' era un error, debe ser por instancia.
        // 'm_newState' ya no es necesario.
        public AnimationState m_State = AnimationState.Idle;

        private Animator anim;

        // --- TIMERS ---
        // Estos se quedan, ya que controlan CUÁNDO la animación puede volver
        // a un estado base (Idle/Walking)
        public float timeToEnd = 0.0f;
        public float sprintDelay = 0.5f,
            attackMageDelay = 2f,
            attackMeleeDay = 1f, // Cuidado: typo "Day" en lugar de "Delay"
            hurtDelay = 1f,
            attDistanceDelay = 4f,
            changeStateTime = 0.5f;

        // Cooldowns (se quedan para los 'if' de 'changeState')
        public float nextAttackMageTime = 0,
            nextAttackDistanceTime = 0,
            nextAttackMeleeTime = 0,
            nextHurt = 0,
            nextSprint = 0;


        // --- AWAKE (Corregido) ---
        private void Awake()
        {
            // Lógica de Singleton robusta
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning("Instancia duplicada de PlayerAnimation destruida.");
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            anim = GetComponent<Animator>();
            if (anim == null)
            {
                Debug.LogError("¡¡ERROR!! El script PlayerAnimation no pudo encontrar el componente 'Animator'.", this.gameObject);
            }
        }

        // --- FIXEDUPDATE (Eliminado) ---
        // Toda la lógica de 'Running' (reducir estamina, cambiar velocidad)
        // ya está en 'PlayerMovement.cs'. Tenerla aquí creaba conflictos.
        /*
        public void FixedUpdate()
        {
             // ... CÓDIGO ELIMINADO ...
        }
        */

        // --- UPDATE (¡NUEVO!) ---
        /// <summary>
        /// Gestiona automáticamente las animaciones base (Idle, Walk, Run)
        /// leyendo el estado de PlayerMovement.
        /// </summary>
        private void Update()
        {
            // 1. Si una "acción" (ataque, hurt) se está reproduciendo,
            // no hacemos nada. El timer 'timeToEnd' nos bloquea.
            if (Time.time < timeToEnd)
            {
                return;
            }

            // 2. Si acabamos de terminar una acción, reseteamos al estado base
            if (m_State != AnimationState.Idle && m_State != AnimationState.Walking && m_State != AnimationState.Running)
            {
                m_State = AnimationState.Idle;
            }

            // 3. Leemos el estado del "cerebro" (PlayerMovement)
            bool isMoving = PlayerMovement.instance.IsMoving;
            bool isRunning = PlayerMovement.instance.IsRunning;

            // 4. Decidimos el nuevo estado base
            AnimationState newState = m_State;

            if (isRunning)
            {
                newState = AnimationState.Running;
            }
            else if (isMoving)
            {
                newState = AnimationState.Walking;
            }
            else
            {
                newState = AnimationState.Idle;
            }

            // 5. Si el estado ha cambiado, lo aplicamos
            if (m_State != newState)
            {
                SetBaseState(newState);
            }
        }

        /// <summary>
        /// Método PRIVADO solo para cambiar entre Idle, Walking y Running.
        /// No usa timers.
        /// </summary>
        private void SetBaseState(AnimationState state)
        {
            m_State = state;
            switch (state)
            {
                case AnimationState.Idle:
                    anim.SetFloat("a_speed", 1);
                    anim.Play("Idle");
                    break;
                case AnimationState.Walking:
                    anim.SetFloat("a_speed", 1);
                    anim.Play("Running"); // Usas "Running" para caminar
                    break;
                case AnimationState.Running:
                    anim.SetFloat("a_speed", 2); // Velocidad 2 para correr
                    anim.Play("Running");
                    break;
            }
        }


        // --- CHANGESTATE (Modificado) ---
        /// <summary>
        /// Método PÚBLICO para reproducir "Acciones" (Ataques, Sprint, Pointing).
        /// Estas interrumpen el ciclo de movimiento base.
        /// </summary>
        public void changeState(AnimationState state)
        {
            // --- PRUEBA 1 ---
            Debug.Log($"--- changeState LLAMADO con estado: {state} ---");

            // BLOQUEO 1
            if (state == AnimationState.Idle || state == AnimationState.Walking || state == AnimationState.Running)
            {
                Debug.LogWarning("BLOQUEO 1: Saliendo (es un estado base).");
                return;
            }

            // --- PRUEBA 2 ---
            Debug.Log($"Pasado BLOQUEO 1. Comprobando timeToEnd. (Time.time = {Time.time} vs timeToEnd = {timeToEnd})");

            // BLOQUEO 2
            if (Time.time < timeToEnd)
            {
                Debug.LogWarning($"¡¡¡BUG ENCONTRADO!!! BLOQUEO 2: Saliendo. (Time.time {Time.time} ES MENOR que timeToEnd {timeToEnd})");
                return;
            }

            // --- PRUEBA 3 ---
            Debug.LogWarning("--- ¡PASADO BLOQUEO 2! ENTRANDO AL SWITCH... ---");

            // El switch
            switch (state)
            {
                case AnimationState.Sprint:
                    Debug.Log("Switch: Entrando en Sprint.");
                    m_State = AnimationState.Sprint;
                    anim.Play("Running");
                    timeToEnd = Time.time + changeStateTime;
                    nextSprint = Time.time + sprintDelay;
                    break;

                case AnimationState.Pointing:
                    Debug.Log("Switch: Entrando en Pointing.");
                    m_State = AnimationState.Pointing;
                    anim.Play("AttackMage");
                    break;

                case AnimationState.AttackMelee:
                    Debug.Log("Switch: Entrando en AttackMelee.");
                    m_State = AnimationState.AttackMelee;
                    anim.Play("AttackMelee");
                    timeToEnd = Time.time + changeStateTime;
                    break;

                case AnimationState.AttackDistance:
                    Debug.Log("Switch: Entrando en AttackDistance.");
                    m_State = AnimationState.AttackDistance;
                    anim.Play("AttackDistance");
                    timeToEnd = Time.time + changeStateTime;
                    break;
            }
        }

        /// <summary>
        /// Método público para cuando el jugador recibe daño.
        /// </summary>
        public void Hurt(float x) // 'x' no se usa
        {
            if (Time.time > timeToEnd && Time.time > nextHurt)
            {
                // --- Lógica de Daño (PlayerManager.instance.ReduceHealth())
                // --- debe llamarse desde el script que detecta la colisión.

                anim.Play("Hurted");
                m_State = AnimationState.Hurt;
                timeToEnd = Time.time + 1f; // Bloqueamos Update()
                nextHurt = Time.time + hurtDelay; // Cooldown
            }
        }
    }
}