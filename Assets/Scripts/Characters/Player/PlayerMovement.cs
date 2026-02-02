using UnityEngine;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
        [SerializeField] private float initialWalkSpeed = 5f;
        private float walkSpeed = 5f;
        [SerializeField] private float initialRunSpeed = 8f;
        private float runSpeed = 8f;


        // --- VARIABLES ANTIGUAS (Sin Tocar) ---
        private Vector2 mousePos; // 'mousePos' no se asigna en este script, ¡revisar!
        private int sprintIncrease = 5;
        float sprintForce = 5000f;
        float angle;
        [SerializeField] Vector2 lookDir;
        [SerializeField] float noMovementAttackingTime = 0f;

        private bool moveOnXAxis = true; // "Memoria": ¿Nos estábamos moviendo en X?
        private Vector2 lastInput = Vector2.zero; // Para detectar cambios repentinos


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
            walkSpeed = initialWalkSpeed;
            runSpeed = initialRunSpeed;
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


        public Vector2 ReturnMove()
        {
            // Devuelve el input crudo, esto puede ser útil
            return m_MoveInput;
        }


        // --- FIXEDUPDATE (Lógica Principal Modificada) ---
        private void FixedUpdate()
        {
            m_lr.enabled = false;

            // Deadzone para evitar ruido del joystick
            float deadzone = 0.1f;
            bool hasInputX = Mathf.Abs(m_MoveInput.x) > deadzone;
            bool hasInputY = Mathf.Abs(m_MoveInput.y) > deadzone;

            // Calcular estado
            IsMoving = hasInputX || hasInputY;
            IsRunning = IsMoving && m_IsRunningInput;
            float currentSpeed = IsRunning ? runSpeed : walkSpeed;

            // --- LÓGICA DE ÚLTIMO EJE PULSADO (PRIORIDAD) ---

            // 1. Detectamos si se ACABA de pulsar una tecla
            bool justPressedX = hasInputX && Mathf.Abs(lastInput.x) <= deadzone;
            bool justPressedY = hasInputY && Mathf.Abs(lastInput.y) <= deadzone;

            // 2. Si se acaba de pulsar una, esa gana el control inmediatamente
            if (justPressedX)
            {
                moveOnXAxis = true;
            }
            else if (justPressedY)
            {
                moveOnXAxis = false;
            }

            // 3. Si soltamos la tecla que tenía el control, se lo pasamos a la otra
            if (moveOnXAxis && !hasInputX && hasInputY)
            {
                moveOnXAxis = false;
            }
            else if (!moveOnXAxis && !hasInputY && hasInputX)
            {
                moveOnXAxis = true;
            }

            // 4. Construimos el vector final
            Vector2 cardinalMove = Vector2.zero;

            if (IsMoving)
            {
                if (moveOnXAxis && hasInputX)
                {
                    cardinalMove.x = m_MoveInput.x;
                    cardinalMove.y = 0;
                }
                else if (hasInputY)
                {
                    cardinalMove.x = 0;
                    cardinalMove.y = m_MoveInput.y;
                }
                else if (hasInputX)
                {
                    cardinalMove.x = m_MoveInput.x;
                    cardinalMove.y = 0;
                }
            }

            // Guardamos el input para el siguiente frame
            lastInput = m_MoveInput;

            // 5. Aplicamos movimiento físico
            rb.MovePosition(rb.position + cardinalMove * (currentSpeed * Time.fixedDeltaTime));

            // 6. Flipear Sprite (según la dirección real en la que nos movemos)
            if (cardinalMove.x > 0.1f) m_mySpriteRenderer.flipX = true;
            else if (cardinalMove.x < -0.1f) m_mySpriteRenderer.flipX = false;
        }

        // --- FUNCIONES RESTANTES (Sin Tocar) ---

        public int Direction()
        {
            return m_mySpriteRenderer.flipX ? 1 : -1;
        }

        public void startAttack()
        {
            walkSpeed = 0;
            runSpeed = 0;
            StartCoroutine(finishAttack());

        }

        private IEnumerator finishAttack()
        {

            yield return new WaitForSeconds(noMovementAttackingTime);
            walkSpeed = initialWalkSpeed;
            runSpeed = initialRunSpeed;
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