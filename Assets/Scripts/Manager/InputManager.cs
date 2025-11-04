using nicorueda.Player;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gestiona todas las entradas del jugador y actúa como "Director de Orquesta".
/// 1. Comprueba Cooldowns (desde PlayerAttack)
/// 2. Comprueba Recursos (desde PlayerManager)
/// 3. Llama a la Lógica (PlayerAttack / PlayerMovement)
/// 4. Llama a la Animación (PlayerAnimation)
/// </summary>
public class InputManager : MonoBehaviour
{
    // Flag para el ataque cargado (apuntar y soltar)
    private bool isAimingDistance = false;

    // --- ACCIONES DE COMBATE Y HABILIDAD ---

    public void OnAttackMelee(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.LogWarning("--- InputManager: OnAttackMelee INICIADO ---");

            // --- COMPROBACIÓN 1: ¿Está listo PlayerAttack? ---
            if (PlayerAttack.instance == null)
            {
                Debug.LogError("¡¡ERROR (InputManager)!! PlayerAttack.instance es NULO.");
                return;
            }
            if (!PlayerAttack.instance.IsMeleeReady())
            {
                Debug.LogError("¡BLOQUEADO (InputManager)! Cooldown de PlayerAttack NO LISTO.");
                return;
            }

            Debug.Log("InputManager: Cooldown de Ataque OK.");

            // --- COMPROBACIÓN 2: ¿Tenemos estamina? ---
            if (PlayerManager.instance == null)
            {
                Debug.LogError("¡¡ERROR (InputManager)!! PlayerManager.instance es NULO.");
                return;
            }
            if (!PlayerManager.instance.ReduceStamina(0)) // Asumiendo que ReduceStamina() devuelve true/false
            {
                Debug.LogError("¡BLOQUEADO (InputManager)! Estamina INSUFICIENTE.");
                return;
            }

            Debug.Log("InputManager: Estamina OK.");

            // --- COMPROBACIÓN 3: ¿Podemos llamar a la animación? ---
            if (PlayerAnimation.instance == null)
            {
                Debug.LogError("¡¡ERROR (InputManager)!! PlayerAnimation.instance es NULO. Imposible llamar a changeState.");
                return;
            }

            Debug.LogWarning("--- InputManager: ¡Todo OK! LLAMANDO A PlayerAnimation.changeState... ---");

            // La llamada final
            PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.AttackMelee);
        }
    }

public void OnAttackDistanceNoPoint(InputAction.CallbackContext context)
{
    Debug.LogWarning("--- InputManager: OnAttackDistanceNoPoint INICIADO ---");

    // Solo reaccionamos al 'started' (al pulsar el botón)
    if (context.started)
    {
        // --- COMPROBACIÓN 1: ¿Cooldown listo? ---
        if (!PlayerAttack.instance.IsDistanceReady()) 
        {
            Debug.LogError("¡BLOQUEADO (InputManager)! Cooldown de Distancia NO LISTO.");
            return;
        }
        Debug.Log("InputManager: Cooldown de Distancia OK.");

        // --- COMPROBACIÓN 2: ¿Estamina lista? ---
        // (Usando el coste de 10 que definimos)
        if (!PlayerManager.instance.ReduceStamina(0)) 
        {
            Debug.LogError("¡BLOQUEADO (InputManager)! Estamina INSUFICIENTE para Distancia.");
            return;
        }
        Debug.Log("InputManager: Estamina de Distancia OK.");

        // --- COMPROBACIÓN 3: ¿Podemos llamar a la lógica? ---
        if(PlayerAttack.instance == null)
        {
            Debug.LogError("¡¡ERROR (InputManager)!! PlayerAttack.instance es NULO.");
            return;
        }

        // 3. Lógica de juego (¡Sin instanciar proyectil!)
        PlayerAttack.instance.AttackingAsDistanceNoPointing(); 
        
        // --- COMPROBACIÓN 4: ¿Podemos llamar a la animación? ---
        if(PlayerAnimation.instance == null)
        {
            Debug.LogError("¡¡ERROR (InputManager)!! PlayerAnimation.instance es NULO.");
            return;
        }
        
        // 4. Animación
        Debug.LogWarning("--- InputManager: ¡Todo OK! LLAMANDO A PlayerAnimation.changeState(AttackDistanceNoPoint)... ---");
        PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.AttackDistanceNoPoint);
    }
}

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // 1. Comprobamos cooldown (este cooldown vive en PlayerAnimation)
            if (Time.time < PlayerAnimation.instance.nextSprint) return;

            // 2. Comprobamos stamina
            //comprobar la stamina que reducimos
            if (!PlayerManager.instance.ReduceStamina(1f)) return;

            // 3. Lógica de juego
            PlayerMovement.instance.Sprint();

            // 4. Animación
            PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.Sprint);
        }
    }

    // --- ACCIONES DE MOVIMIENTO (Sin Cambios) ---

    /// <summary>
    /// OnMove ahora SOLO envía el vector de movimiento al PlayerMovement.
    /// </summary>
    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 movement = value.ReadValue<Vector2>();
        PlayerMovement.instance.SetMoveInput(movement);
    }

    /// <summary>
    /// OnRun ahora SOLO envía un booleano (true/false) al PlayerMovement.
    /// </summary>
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerMovement.instance.SetRunInput(true);
        }

        if (context.canceled)
        {
            PlayerMovement.instance.SetRunInput(false);
        }
    }

    // --- OTRAS ACCIONES ---

    public void OnGodMode(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("GOD MODE ACTIVADO");
            // Asumo que tu GameManager (no PlayerManager) tiene el GodMode
            GameManager.instance.GodMode();
        }
    }
}