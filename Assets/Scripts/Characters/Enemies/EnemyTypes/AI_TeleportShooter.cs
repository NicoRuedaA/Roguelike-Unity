using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{
    public class AI_TeleportShooter : EnemyAI_Base
{
    [SerializeField] private float activeDistance = 15f;
    [SerializeField] private float teleportCooldown = 5f;
    [SerializeField] private Transform[] moveSpots; // Los puntos de TP
    private float lastTeleportTime;
    
    public override void HandleAI()
    {
        IsMoving = false; // El TP no es "movimiento"
        float distance = Vector2.Distance(transform.position, manager.Player.position);

        if (distance < activeDistance)
        {
            // 1. Intentar disparar
            if (manager.Attack.CanAttack())
            {
                IsAttacking = true;
                manager.Attack.PerformAttack();
            }
            else
            {
                IsAttacking = false;
            }

            // 2. Intentar Teleportarse
            if (Time.time > lastTeleportTime + teleportCooldown)
            {
                lastTeleportTime = Time.time;
                int randomSpot = Random.Range(0, moveSpots.Length);
                transform.position = moveSpots[randomSpot].position;
            }
        }
    }
}
}
