using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{

// Script "Cara" reutilizable
[RequireComponent(typeof(EnemyManager))]
public class EnemyAnimation : MonoBehaviour
{
    private EnemyManager manager;
    private EnemyAI_Base ai;
    
    private void Awake()
    {
        manager = GetComponent<EnemyManager>();
        ai = manager.AI; // El manager ya lo encontró
    }

    private void Update()
    {
        // 1. Muerte (prioridad máxima)
        if (manager.isDead)
        {
            manager.Anim.Play("Die");
            return;
        }

        // 2. Ataque
        if (ai.IsAttacking)
        {
            manager.Anim.Play("Attack");
        }
        // 3. Movimiento
        else if (ai.IsMoving)
        {
            manager.Anim.Play("Walk");
        }
        // 4. Reposo
        else
        {
            manager.Anim.Play("Idle");
        }
        
        // 5. Flipear Sprite
        if (manager.Player.position.x > transform.position.x)
            manager.SpriteRend.flipX = true;
        else
            manager.SpriteRend.flipX = false;
    }
}
}
