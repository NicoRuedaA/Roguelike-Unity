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
            // 1. Comprobamos cooldown (preguntamos a PlayerAttack)
            if (!PlayerAttack.instance.IsMeleeReady()) return;

            // 2. Comprobamos estamina
            if (!PlayerManager.instance.ReduceStamina(1f)) return;

            // 3. Lógica de juego
            PlayerAttack.instance.AttackingAsMelee();

            // 4. Animación
            PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.AttackMelee);
        }
    }

    public void OnAttackDistance(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // --- INICIO DE APUNTADO ---
            isAimingDistance = false; // Reseteamos

            // 1. Comprobamos cooldown
            if (!PlayerAttack.instance.IsDistanceReady()) return;

            // 2. Comprobamos estamina para *empezar* a apuntar
            if (!PlayerManager.instance.ReduceStamina(1f)) return;

            // Si tenemos éxito, marcamos el flag y activamos la lógica/animación
            isAimingDistance = true;
            PlayerMovement.instance.Pointing();
            PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.Pointing);
        }

        if (context.canceled)
        {
            // --- SOLTAR Y DISPARAR ---

            // Si no empezamos a apuntar con éxito (sin estamina/en cooldown), no disparamos
            if (!isAimingDistance) return;

            isAimingDistance = false; // Reseteamos

            // 3. Lógica de juego (El cooldown real se activa en este método)
            PlayerAttack.instance.AttackingAsDistance();

            // 4. Animación
            PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.AttackDistance);
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