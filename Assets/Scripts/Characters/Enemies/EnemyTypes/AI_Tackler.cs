using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{
    public class AI_Tackler : EnemyAI_Base
{
    private enum State { Idle, Charging, Tackling, Cooldown }
    private State m_State = State.Idle;

    [SerializeField] private float chargeTime = 1f;
    [SerializeField] private float tackleSpeed = 10f;
    [SerializeField] private float cooldownTime = 3f;
    private float timeToEndState;
    private Vector2 tackleTarget;

    public override void HandleAI()
    {
        switch (m_State)
        {
            case State.Idle:
                IsMoving = false;
                IsAttacking = false;
                // Espera a que el cooldown del "Puño" esté listo
                if (manager.Attack.CanAttack())
                {
                    m_State = State.Charging;
                    timeToEndState = Time.time + chargeTime;
                    tackleTarget = manager.Player.position;
                }
                break;
                
            case State.Charging:
                IsMoving = false;
                IsAttacking = true; // La "carga" es la animación de ataque
                if (Time.time > timeToEndState)
                {
                    m_State = State.Tackling;
                    manager.Attack.PerformAttack(); // Llama a "Puños" (activa el hitbox)
                }
                break;
                
            case State.Tackling:
                IsMoving = true;
                IsAttacking = true;
                Vector2 targetPos = Vector2.MoveTowards(transform.position, tackleTarget, tackleSpeed * Time.fixedDeltaTime);
                manager.Rb.MovePosition(targetPos);
                
                if (Vector2.Distance(transform.position, tackleTarget) < 0.1f)
                {
                    m_State = State.Cooldown;
                    timeToEndState = Time.time + cooldownTime;
                    manager.Attack.StopTackleAttack(); // Desactiva el hitbox
                }
                break;
                
            case State.Cooldown:
                IsMoving = false;
                IsAttacking = false;
                if (Time.time > timeToEndState)
                {
                    m_State = State.Idle;
                }
                break;
        }
    }
}
}
