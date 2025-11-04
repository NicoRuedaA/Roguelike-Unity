using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{
    public class AI_StaticShooter : EnemyAI_Base
{
    [SerializeField] private float activeDistance = 15f;
    
    public override void HandleAI()
    {
        IsMoving = false; // Nunca se mueve
        
        float distance = Vector2.Distance(transform.position, manager.Player.position);
        
        // Si el jugador está a la vista y podemos atacar...
        if (distance < activeDistance && manager.Attack.CanAttack())
        {
            IsAttacking = true;
            manager.Attack.PerformAttack(); // Llama a "Puños"
        }
        else
        {
            IsAttacking = false;
        }
    }
}
}
