using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{

    public class EnemyProjectileExplosion : EnemyProjectile
    {

        private bool hasExploded = false;
        private Animator anim;

        protected override void Start()
        {
            anim = gameObject.GetComponent<Animator>();
            // Destruir la flecha después de X segundos para no llenar la memoria
            this.actualTime = 0f;
        }


        protected override void Update()
        {

            if (!hasExploded)
            {
                actualTime += Time.deltaTime;
            }

            if (actualTime < 0.5f)
            {

                transform.Translate(Vector2.right * speed * Time.deltaTime);

            }

            else if (actualTime > 1.5f)
            {
                Explode();
            }



        }

        private void Explode()
        {

            this.hasExploded = true;
            anim.Play("Explosion");
        }

        private void Delete()
        {
            Destroy(gameObject);
        }


    }
}
