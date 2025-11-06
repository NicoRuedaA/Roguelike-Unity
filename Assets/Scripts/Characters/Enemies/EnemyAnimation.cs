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
            // En Awake, solo obtenemos la referencia a nuestro propio manager
            manager = GetComponent<EnemyManager>();
        }
    
    private void Start()
    {
        // En Start, le pedimos al manager sus "partes"
        // Esto se ejecuta DESPUÉS de que EnemyManager.Awake() haya terminado
        ai = manager.AI;

        // Añadimos una comprobación de seguridad para evitar futuros errores
        if (ai == null)
        {
            Debug.LogError("EnemyAnimation (" + gameObject.name + ") no pudo encontrar el CEREBRO (AI) desde el EnemyManager. ¿Está el script AI adjunto?", gameObject);
            this.enabled = false; // Desactivamos este script para evitar spam de errores
        }
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
