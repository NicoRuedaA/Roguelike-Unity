using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{
    public class AI_RetreatMelee : EnemyAI_Base
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float retreatDistance = 5f;

    public override void HandleAI()
    {
        float distance = Vector2.Distance(transform.position, manager.Player.position);

        if (distance < attackRange && manager.Attack.CanAttack())
        {
            // 1. Atacar si está muy cerca
            IsMoving = false;
            IsAttacking = true;
            manager.Attack.PerformAttack();
        }
        else if (distance < retreatDistance)
        {
            // 2. Alejarse si está demasiado cerca
            IsMoving = true;
            IsAttacking = false;
            Vector2 targetPos = Vector2.MoveTowards(transform.position, manager.Player.position, -manager.Speed * Time.fixedDeltaTime);
            manager.Rb.MovePosition(targetPos);
        }
        else
        {
            // 3. Esperar
            IsMoving = false;
            IsAttacking = false;
        }
    }
}
}
