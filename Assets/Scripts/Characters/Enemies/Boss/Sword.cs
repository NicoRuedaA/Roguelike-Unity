using System.Collections;
using System.Collections.Generic;
using nicorueda.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using CharacterController = nicorueda.CharacterBase;


namespace nicorueda // <-- AÑADE ESTA LÍNEA
{
    public class Sword : EnemyManager
    {/*
    
        //es un estado pero hice copiar y pegar del character controller y no hay tiempo jejejeej 
        private enum stateMachine
        {
            idle,
            rest,
            charge,
            shoot,
        }

        [SerializeField] stateMachine m_State;

        //borrar la 23532 variables innecesarias

        GameObject m_player;

        public float offset;

        private Vector3 targetPos;
        private Vector3 thisPos;
        private float angle;



        //VARIABLES SEGUIR
        [SerializeField] private float initialStoppingDistance, initialRetreadDistance;
        private float stoppingDistance, retreadDistance;


        Vector2 toTackle;

        //VARIABLES DISPARAR Y PLACAR
        private float timeToEndState;
        private float shootTime, tackleTime;
        [SerializeField] private GameObject[] projectile;
        [FormerlySerializedAs("initialTimeBtwShoots")][SerializeField] private float initialTimeShoots;
        [FormerlySerializedAs("initialTimeBtwCharge")][SerializeField] private float initialTimeCharge;
        [SerializeField] private float restTime, idleTime;


        private bool playerInArea;
        private float timeInArea;

        [SerializeField] private int maxBullet = 2;
        private int attack;

        private bool mattacking;
        private int projectileToInstantiate;
        [SerializeField] private float chargeSpeed;

        [SerializeField] AudioSource placaje_sonido;
        [SerializeField] AudioSource disparo_sonido;

        private void Awake()
        {
            m_player = GameObject.FindGameObjectWithTag("Player");
        }


        void Start()
        {
            isVulnerable = true;

            shootTime = initialTimeShoots;
            tackleTime = initialTimeCharge;

            stoppingDistance = initialStoppingDistance;
            retreadDistance = initialRetreadDistance;

            playerInArea = false;
            timeInArea = 0f;

            attack = 0;
            m_State = stateMachine.idle;
            changeState(m_State);


        }

        void LateUpdate()
        {
            //quitar detectionarea del body
            targetPos = m_player.transform.position;
            thisPos = transform.position;
            targetPos.x -= thisPos.x;
            targetPos.y -= thisPos.y;
            angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        }


        private void FixedUpdate()
        {



            switch (m_State)
            {
                case (stateMachine.shoot):
                    if (Time.time > timeToEndState)
                    {
                        m_State = stateMachine.idle;
                        changeState(m_State);
                    }
                    break;
                case (stateMachine.charge):
                    Charge();
                    if ((Vector2.Distance(transform.position, toTackle) < 0.1f) || Time.time > timeToEndState)
                    {
                        m_State = stateMachine.rest;
                        changeState(m_State);
                    }
                    break;
                case (stateMachine.rest):
                    if (Time.time > timeToEndState)
                    {
                        m_State = stateMachine.idle;
                        changeState(m_State);
                    }
                    break;
                case (stateMachine.idle):
                    FollowPlayer();
                    if (Time.time > timeToEndState)
                    {
                        Roll();
                    }
                    break;
            }
        }


        void changeState(stateMachine state)
        {
            switch (state)
            {
                case stateMachine.charge:
                    placaje_sonido.Play();
                    toTackle = new Vector2(m_player.transform.position.x, m_player.transform.position.y);

                    timeToEndState = Time.time + tackleTime;
                    stoppingDistance = 1;
                    retreadDistance = 1;
                    attacking = true;

                    break;
                case stateMachine.shoot:
                    projectileToInstantiate = Random.Range(0, projectile.Length);

                    //for (int i = 0; i < Random.Range(0, maxBullet); i++)
                    //{
                    Shoot(projectileToInstantiate);
                    //}
                    timeToEndState = Time.time + shootTime;
                    stoppingDistance = initialStoppingDistance;
                    retreadDistance = initialRetreadDistance;
                    attacking = false;
                    break;
                case stateMachine.rest:

                    timeToEndState = Time.time + restTime;
                    stoppingDistance = initialStoppingDistance;
                    retreadDistance = initialRetreadDistance;
                    attacking = false;

                    break;
                case stateMachine.idle:

                    timeToEndState = Time.time + idleTime;
                    stoppingDistance = initialStoppingDistance;
                    retreadDistance = initialRetreadDistance;
                    attacking = false;

                    break;

            }

        }


        void Roll()
        {
            attack = Random.Range(0, 2);
            if (attack == 0)
            {
                m_State = stateMachine.shoot;
            }
            else if (attack == 1)
            {
                m_State = stateMachine.charge;
            }
            changeState(m_State);
        }


        void Charge()
        {
            transform.position = Vector2.MoveTowards(transform.position, toTackle,
                speed * 3 * Time.deltaTime);
        }


        void FollowPlayer()
        {
            if (Vector2.Distance(transform.position, m_player.transform.position) > stoppingDistance)
            {

                transform.position = Vector2.MoveTowards(transform.position, m_player.transform.position,
                    chargeSpeed * Time.deltaTime);

            }


            else if (Vector2.Distance(transform.position, m_player.transform.position) < retreadDistance)
            {
                transform.position =
                    Vector2.MoveTowards(transform.position, m_player.transform.position, -speed * Time.deltaTime);
            }
        }


        void Shoot(int x)
        {
            disparo_sonido.Play();
            var newProjectile = Instantiate(projectile[x], transform.position, Quaternion.identity, transform);
            newProjectile.transform.parent = gameObject.transform.parent;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && attacking)
            {
                PlayerAnimation.instance.Hurt(transform.position.x);

                m_State = stateMachine.idle;
                changeState(m_State);
            }
        }


    protected override void Die()
    {

    }
*/
    }
}

    