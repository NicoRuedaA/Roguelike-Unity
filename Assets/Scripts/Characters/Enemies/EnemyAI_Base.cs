using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{

// Plantilla abstracta para TODOS los "Cerebros"
[RequireComponent(typeof(EnemyManager))]
public abstract class EnemyAI_Base : MonoBehaviour
{
    // Estados que la "Cara" (Animación) puede leer
    public bool IsMoving { get; protected set; }
    public bool IsAttacking { get; protected set; }

    protected EnemyManager manager; // Referencia al "Cuerpo"

    protected virtual void Awake()
    {
        // Obtiene la referencia al "Cuerpo" al que pertenece
        manager = GetComponent<EnemyManager>();
    }
    
    // El FixedUpdate del Manager llamará a esto
    protected virtual void FixedUpdate()
    {
        if (manager.isDead)
        {
            IsMoving = false;
            IsAttacking = false;
            return;
        }
        
        // Ejecuta la lógica de IA específica del hijo
        HandleAI();
    }
    
    // Forzamos a los hijos (Tackler, Shooter) a implementar esta lógica
    public abstract void HandleAI();
}
}
