using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda.Player
{

    public class PlayerProjectile : MonoBehaviour
    {

        public GameObject carga_prefab;

        public float TimeToLive = 5f;
        private float velocidad = 10f;
        private Vector3 direccion = Vector3.left;
        [SerializeField] private bool residuo = false;

        private void Start()
        {

            Destroy(gameObject, TimeToLive);
        }


        private void Update()
        {
            if (!residuo)
                transform.Translate(direccion * velocidad * Time.deltaTime);
        }

        public void cambiarDireccion(int x)
        {
            if (x == 1)
            {

                this.direccion = Vector3.right;
            }
            else if (x == -1)
            {

                this.direccion = Vector3.left;
            }
        }

        void DestroyProjectile()
        {
            Destroy(gameObject);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.CompareTag("Player")) { }

            else
            {
                if (other.CompareTag("Enemy"))
                {
                    //other.GetComponent<EnemyController2>().TakeDamage(1, this.transform.position);
                }

                else if (other.CompareTag("EnemyProjectile"))
                {
                    Destroy(other.gameObject);
                }

                else if (other.CompareTag("BossBody"))
                {
                    other.GetComponent<Body>().ReduceHealth();
                }
                else if (other.CompareTag("Sword"))
                {
                    //other.GetComponent<Sword>().TakeDamage(1, this.transform.position);
                }

                DestroyProjectile();
                Instantiate(carga_prefab, transform.position, Quaternion.identity);
            }


        }


    }
}

/*



    else if(other.CompareTag("Wall")){
DestroyProjectile();
}*/


