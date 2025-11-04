using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{

// Script "Puños" reutilizable
[RequireComponent(typeof(EnemyManager))]
public class EnemyAttack : MonoBehaviour
{
    public enum AttackType { Melee, Projectile, Tackle }
    
    [Header("Configuración General")]
    [SerializeField] private AttackType attackType = AttackType.Projectile;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private AudioSource attackSound;
    
    [Header("Configuración Proyectil")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;

    [Header("Configuración Melee/Tackle")]
    [SerializeField] private Collider2D hitBox; // Un Trigger hijo del enemigo
    
    private float lastAttackTime;
    private EnemyManager manager;

    private void Awake()
    {
        manager = GetComponent<EnemyManager>();
        if(hitBox != null) hitBox.enabled = false; // Asegura que el hitbox esté apagado
    }
    
    // El "Cerebro" (IA) llama a esto
    public bool CanAttack()
    {
        return Time.time > lastAttackTime + attackCooldown;
    }

    // El "Cerebro" (IA) llama a esto
    public void PerformAttack()
    {
        if (!CanAttack()) return;

        lastAttackTime = Time.time;
        if(attackSound != null) attackSound.Play();
        
        // Decide qué tipo de ataque hacer
        switch (attackType)
        {
            case AttackType.Projectile:
                Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
                break;
                
            case AttackType.Melee:
                // Activa el hitbox por 0.5s (el "Cerebro" debe esperar)
                StartCoroutine(MeleeSwingRoutine(0.5f)); 
                break;
                
            case AttackType.Tackle:
                // El "Cerebro" (IA) activa y desactiva el hitbox
                hitBox.enabled = true;
                break;
        }
    }
    
    // El "Cerebro" (IA del Tackler) llama a esto
    public void StopTackleAttack()
    {
        hitBox.enabled = false;
    }

    // Rutina para un ataque melee rápido
    private IEnumerator MeleeSwingRoutine(float duration)
    {
        hitBox.enabled = true;
        yield return new WaitForSeconds(duration);
        hitBox.enabled = false;
    }

    // Lógica de daño
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Esta función solo se activa si el 'hitBox' está activo
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterBase>().TakeDamage(damage, transform.position);
        }
    }
}
}
