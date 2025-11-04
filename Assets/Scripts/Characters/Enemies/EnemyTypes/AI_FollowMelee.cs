using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{
    public class AI_FollowMelee : EnemyAI_Base
{
    [SerializeField] private float stoppingDistance = 1f;
    
    public override void HandleAI()
    {
        float distance = Vector2.Distance(transform.position, manager.Player.position);

        if (distance > stoppingDistance)
        {
            // 1. Seguir
            IsMoving = true;
            IsAttacking = false;
            Vector2 targetPos = Vector2.MoveTowards(transform.position, manager.Player.position, manager.Speed * Time.fixedDeltaTime);
            manager.Rb.MovePosition(targetPos);
        }
        else if (manager.Attack.CanAttack())
        {
            // 2. Atacar
            IsMoving = false;
            IsAttacking = true;
            manager.Attack.PerformAttack();
        }
        else
        {
            // 3. Esperar (en rango, pero en cooldown)
            IsMoving = false;
            IsAttacking = false;
        }
    }
}
}
