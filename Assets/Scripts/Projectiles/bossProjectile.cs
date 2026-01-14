using System.Collections;
using System.Collections.Generic;
using nicorueda.Player;
using UnityEngine;

public class bossProjectile : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.CompareTag("BossBody") || (other.CompareTag("EnemyProjectile"))) { }
        else
        {
            if (other.CompareTag("Player"))
            {
                MakeDMG();
            }
            DestroyProjectile();
        }

        //
    }

    void MakeDMG()
    {
        PlayerManager.instance.TakeDamage(1, this.transform.position);
        DestroyProjectile();
    }


    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
