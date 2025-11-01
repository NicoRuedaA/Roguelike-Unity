using UnityEngine;

namespace nicorueda.Player
{
    public class PlayerMovement : PlayerManager
    {
        // --- INSTANCIA SINGLETON (Corregida) ---
        private static PlayerMovement _instance;
        public new static PlayerMovement instance
        {
            get
            {
                if (_instance == null)
                {
                    // Es mejor usar LogError para que aparezca en rojo en la consola
                    Debug.LogError("PlayerMovement instance is Null!!!");
                }
                return _instance;
            }
        }

        // --- REFERENCIAS DE COMPONENTES ---
        private Rigidbody2D rb;
        private SpriteRenderer m_mySpriteRenderer;
        private LineRenderer m_lr;

        // --- VARIABLES DE MOVIMIENTO (NUEVO) ---
        // Guardan el input crudo que viene del InputManager
        private Vector2 m_MoveInput;
        private bool m_IsRunningInput; // true si el botón "Run" está pulsado

        // --- PROPIEDADES DE ESTADO PÚBLICAS (NUEVO) ---
        // Para que el script de Animación las lea
        public bool IsMoving { get; private set; }
        public bool IsRunning { get; private set; } // Estado real (moviéndose Y corriendo)

        // --- AJUSTES DE VELOCIDAD (NUEVO) ---
        // Necesitamos velocidades para caminar y correr. 
        // He reemplazado la llamada a 'PlayerManager.instance.Speed' por esto
        // para que la lógica de correr funcione.
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 8f;


        // --- VARIABLES ANTIGUAS (Sin Tocar) ---
        private Vector2 mousePos; // 'mousePos' no se asigna en este script, ¡revisar!
        private int sprintIncrease = 5;
        float sprintForce = 5000f;
        float angle;
        [SerializeField] Vector2 lookDir;

        /* [SerializeField] private AudioSource caminar_sound;
         [SerializeField] private AudioSource correr_sound;*/


        // --- AWAKE (Corregido) ---
        private void Awake()
        {
            // Lógica de Singleton robusta
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning("Instancia duplicada de PlayerMovement destruida.");
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                // Opcional: si el jugador debe sobrevivir a cambios de escena
                // DontDestroyOnLoad(this.gameObject); 
            }

            // Obtenemos componentes
            rb = GetComponent<Rigidbody2D>();
            m_mySpriteRenderer = GetComponent<SpriteRenderer>();
            m_lr = GetComponent<LineRenderer>();

            // 'running' probablemente es heredado de PlayerManager.
            // Ahora lo controlaremos con m_IsRunningInput.
            running = false;
        }

        // --- MÉTODOS PÚBLICOS PARA INPUTMANAGER (NUEVO) ---

        /// <summary>
        /// El InputManager llama a esto en el 'OnMove'
        /// </summary>
        public void SetMoveInput(Vector2 input)
        {
            m_MoveInput = input;
        }

        /// <summary>
        /// El InputManager llama a esto en el 'OnRun' (started/canceled)
        /// </summary>
        public void SetRunInput(bool isHoldingRun)
        {
            m_IsRunningInput = isHoldingRun;
        }

        // --- MÉTODO ANTIGUO (Reemplazado) ---
        // 'MoveLogicMethod' ya no es necesario, usamos 'SetMoveInput'
        /*
        public void MoveLogicMethod(Vector2 moveGet)
        {
            movement = moveGet;
        }
        */

        public Vector2 ReturnMove()
        {
            // Devuelve el input crudo, esto puede ser útil
            return m_MoveInput;
        }


        // --- FIXEDUPDATE (Lógica Principal Modificada) ---
        private void FixedUpdate()
        {
            // 'pointing' debe ser una variable 'bool' heredada de PlayerManager
            if (pointing)
            {
                // --- LÓGICA DE APUNTAR ---
                m_lr.enabled = true;
                m_lr.SetPosition(0, transform.position);
                // Usamos m_MoveInput (el input del joystick) para la dirección
                m_lr.SetPosition(1, new Vector3(transform.position.x + m_MoveInput.x * 7, transform.position.y + m_MoveInput.y * 7, 0));

                // Mientras apunta, el jugador no se mueve
                rb.velocity = Vector2.zero; // Detenemos el deslizamiento
                IsMoving = false;
                IsRunning = false;
            }
            else
            {
                // --- LÓGICA DE MOVERSE ---
                m_lr.enabled = false;

                // 1. Calcular el estado actual (para el animador)
                IsMoving = m_MoveInput.magnitude > 0.1f; // Deadzone pequeño
                IsRunning = IsMoving && m_IsRunningInput; // Solo corre si se mueve Y pulsa "Run"

                // 2. Determinar la velocidad
                float currentSpeed = IsRunning ? runSpeed : walkSpeed;

                // 3. Aplicar movimiento
                // Usamos m_MoveInput y currentSpeed en lugar de 'movement' y 'PlayerManager.instance.Speed'
                rb.MovePosition(rb.position + m_MoveInput * (currentSpeed * Time.fixedDeltaTime));

                // Lógica de apuntado (parece ser con ratón, aunque el input es de joystick)
                // Dejo esta parte como estaba, pero 'mousePos' no se actualiza aquí.
                lookDir = mousePos - rb.position;
                angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

                // 4. Flipear Sprite
                // Añadido un deadzone para que no flipee si el joystick está casi centrado
                if (m_MoveInput.x > 0.1f) m_mySpriteRenderer.flipX = true;
                else if (m_MoveInput.x < -0.1f) m_mySpriteRenderer.flipX = false;
            }
        }

        // --- FUNCIONES RESTANTES (Sin Tocar) ---

        public int Direction()
        {
            return m_mySpriteRenderer.flipX ? 1 : -1;
        }

        void RecoverStamina()
        {
            print("revisar funcion RecoverStamina");
            //PlayerManager.instance.RestoreStamina(10*Time.deltaTime);
        }

        void RecoverMana()
        {
            print("revisar funcion RecoverMana()");
            //PlayerManager.instance.RestoreMana(35);
        }

        public void Pointing()
        {
            print("revisar funcion pointing");
            //if(PlayerManager.instance.ReduceStamina(25)) apuntando = true;
        }

        public void Walking()
        {
            // caminar_sound.Play();
            //llamar a player manager y que haga cosas
        }

        public void Sprint()
        {
            /*if (PlayerManager.instance.ReduceStamina(25) && !apuntando)
            {
                rb.AddForce(new Vector2(movement.x*sprintForce/4, movement.y*sprintForce/4), ForceMode2D.Force); 
            }*/

            print("revisar funcion Sprinting()");
        }
    }
}