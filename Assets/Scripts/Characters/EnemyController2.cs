using System;
using System.Collections;
using System.Collections.Generic;
using nicorueda.Player;
using UnityEngine;
using CharacterController = nicorueda.CharacterController;
using Random = UnityEngine.Random;

public class EnemyController2 : CharacterController
{

    [SerializeField] private AudioSource attack_sound;

//es un estado pero hice copiar y pegar del character controller y no hay tiempo jejejeej 
    private enum stateMachine
    {
        idle,
        rest,
        charge,
        shoot,
        tp,
        slide,
        tackle,
        follow,
        attack,
        block,
        retread,
    }

    private enum enemyType
    {
        staticShooter,
        tpShooter,
        pathShooter,
        tackler,
        meleeFollow,
        distanceBlock,
    }

    [SerializeField] private enemyType m_Type = enemyType.staticShooter;

    stateMachine m_State;

    //borrar la 23532 variables innecesarias

    GameObject m_player;

    public float offset;

    private Vector3 targetPos;
    private Vector3 thisPos;
    private float angle;



    //VARIABLES SEGUIR
    [SerializeField] private float initialStoppingDistance, initialRetreadDistance, letGuardDistance;
    private float stoppingDistance, retreadDistance;


    Vector2 toTackle;

    //VARIABLES DISPARAR Y PLACAR
    private float timeToEndState;
    private float shootTime, tackleTime;
    [SerializeField] private GameObject[] projectile;
    [SerializeField] private float initialTimeShoots;
    [SerializeField] private float initialTimeCharge;
    [SerializeField] private float restTime, idleTime, tpTime;


    private bool playerInArea;
    private float timeInArea;

    [SerializeField] private int maxBullet = 2;
    private int attack;

    private int projectileToInstantiate;
    [SerializeField] private float tackleSpeed;

    Animator m_animator;

    public bool apuntar = false;


    private int randomSpot, lastSpot;
    [SerializeField] Transform[] moveSpots;

    [SerializeField] float timeFollowing = 5, timeToattack = 2;

    private void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        PrepareCharacter();
    }


    SpriteRenderer m_SpriteRenderer;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        LevelManager.instance.AddEnemy();
        shootTime = initialTimeShoots;
        tackleTime = initialTimeCharge;

        stoppingDistance = initialStoppingDistance;
        retreadDistance = initialRetreadDistance;

        playerInArea = false;
        timeInArea = 0f;

        attack = 0;
        m_State = stateMachine.idle;

        changeState(m_State);

        m_animator = GetComponent<Animator>();

    }

    void LateUpdate()
    {

        if(Time.frameCount % 20 == 0 )
          {
            if(m_player.transform.position.x > transform.position.x) m_SpriteRenderer.flipX = true;
            else m_SpriteRenderer.flipX = false;
          }
        
            



        if (apuntar)
        {
            //quitar detectionarea del body
            targetPos = m_player.transform.position;
            thisPos = transform.position;
            targetPos.x -= thisPos.x;
            targetPos.y -= thisPos.y;
            angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        }

    }


    public float activeDistance = 4f;

    private void FixedUpdate()
    {


        if (Vector2.Distance(transform.position, m_player.transform.position) < activeDistance)
        {



        switch (m_State)
        {
            case (stateMachine.rest):
                if (Time.time > timeToEndState)
                {
                    Roll();
                }

                break;
            case (stateMachine.idle):

                if (Time.time > timeToEndState)
                {
                    Roll();
                }

                break;
            case stateMachine.tp:
                if (Time.time > timeToEndState)
                {
                    Roll();
                }

                break;
            case (stateMachine.shoot):
                if (Time.time > timeToEndState)
                {
                    m_State = stateMachine.idle;
                    changeState(m_State);
                }

                break;
            case (stateMachine.slide):
                if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.1f)
                {
                    Roll();
                }
                else
                    transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position,
                        speed * Time.deltaTime);

                break;
            case (stateMachine.tackle):
                if (Vector2.Distance(transform.position, toTackle) < 0.1f)
                {
                    m_State = stateMachine.rest;
                    changeState(m_State);
                }

                Tackle();
                break;
            case (stateMachine.follow):

                FollowPlayer();

                
                if (timeInArea > timeToattack)
                {
                    m_State = stateMachine.attack;
                    changeState(m_State);
                }
                else if (Time.time > timeToEndState)
                {
                    m_State = stateMachine.rest;
                    changeState(m_State);
                }

                break;
            case (stateMachine.attack):
                if (Time.time > timeToEndState)
                {
                    if (m_Type == enemyType.meleeFollow) m_State = stateMachine.follow;
                    else m_State = stateMachine.retread;
                    changeState(m_State);
                }

                break;
            case stateMachine.block:
                if (Vector2.Distance(transform.position, 
                        m_player.transform.position) < letGuardDistance) {
                    m_State = stateMachine.retread;
                    changeState(m_State);
                }
                break;
            case stateMachine.retread:
                transform.position = Vector2.MoveTowards(transform.position, 
                    m_player.transform.position, -speed * Time.deltaTime);
                if (Vector2.Distance(transform.position, 
                        m_player.transform.position) > letGuardDistance) {
                    m_State = stateMachine.block;
                    changeState(m_State);
                }
                else if (timeInArea > timeToattack)
                {
                    m_State = stateMachine.attack;
                    changeState(m_State);
                }
                
                break;

        }
    }
                
                

    }

    


    void changeState(stateMachine state)
    {
        switch (state)
        {
            case stateMachine.charge:
                toTackle = new Vector2(m_player.transform.position.x, m_player.transform.position.y);

                timeToEndState = Time.time + tackleTime;
                stoppingDistance = 1;
                retreadDistance = 1;
                attacking = true;
                vulnerable = false;
                break;
            case stateMachine.shoot:
                projectileToInstantiate = Random.Range(0, projectile.Length);
                Shoot(projectileToInstantiate);
                m_animator.Play("Attack");
                timeToEndState = Time.time + shootTime;
                stoppingDistance = initialStoppingDistance;
                retreadDistance = initialRetreadDistance;
                attacking = false;
                vulnerable = true;
                break;
            case stateMachine.rest:
                timeInArea = 0;
                m_animator.Play("Resting");
                timeToEndState = Time.time + restTime;
                stoppingDistance = initialStoppingDistance;
                retreadDistance = initialRetreadDistance;
                attacking = false;
                vulnerable = true;
                break;
            case stateMachine.idle:

                timeToEndState = Time.time + idleTime;
                stoppingDistance = initialStoppingDistance;
                retreadDistance = initialRetreadDistance;
                attacking = false;
                if (m_Type == enemyType.tackler) vulnerable = false;
                else vulnerable = true;
                break;
            case stateMachine.tp:
                do
                {
                    lastSpot = randomSpot;
                    randomSpot = Random.Range(0, moveSpots.Length);
                } while (lastSpot == randomSpot);

                transform.position = moveSpots[randomSpot].position;
                timeToEndState = Time.time + tpTime;
                stoppingDistance = initialStoppingDistance;
                retreadDistance = initialRetreadDistance;
                attacking = false;
                vulnerable = true;
                break;
            case stateMachine.slide:
                do
                {
                    lastSpot = randomSpot;
                    randomSpot = Random.Range(0, moveSpots.Length);
                } while (lastSpot == randomSpot);

                m_animator.Play("Sliding");
                stoppingDistance = initialStoppingDistance;
                retreadDistance = initialRetreadDistance;
                attacking = true;
                vulnerable = true;
                break;
            case stateMachine.tackle:
                toTackle = m_player.transform.position;
                m_animator.Play("Sliding");
                stoppingDistance = 1;
                retreadDistance = 1;
                attacking = true;
                vulnerable = false;
                break;
            case stateMachine.follow:
                timeInArea = 0;
                timeToEndState = Time.time + timeFollowing;
                //toTackle = m_player.transform.position;
                m_animator.Play("Walk");
                stoppingDistance = 1;
                retreadDistance = 1;
                attacking = false;
                vulnerable = true;
                break;
            case stateMachine.attack:
                timeInArea = 0;
                timeToEndState = Time.time + restTime;
                //toTackle = m_player.transform.position;
                m_animator.Play("Attack");
                attack_sound.Play();
                stoppingDistance = 1;
                retreadDistance = 1;
                attacking = true;
                vulnerable = false;
                break;
            case stateMachine.block:

                toTackle = m_player.transform.position;
                m_animator.Play("Block");
                stoppingDistance = 1;
                retreadDistance = 1;
                attacking = false;
                vulnerable = false;
                break;
            case stateMachine.retread:

                toTackle = m_player.transform.position;
                m_animator.Play("Walk");
                stoppingDistance = 1;
                retreadDistance = 1;
                attacking = false;
                vulnerable = true;
                break;

        }


    }


    void Roll()
    {
        switch (m_Type)
        {
            case enemyType.staticShooter:
                m_State = stateMachine.shoot;
                break;
            case enemyType.tpShooter:
                attack = Random.Range(0, 2);
                if (attack == 0)
                {
                    m_State = stateMachine.shoot;
                }
                else if (attack == 1)
                {
                    m_State = stateMachine.tp;
                }

                break;
            case enemyType.pathShooter:
                attack = Random.Range(0, 2);
                if (attack == 0)
                {
                    m_State = stateMachine.shoot;
                }
                else if (attack == 1)
                {
                    m_State = stateMachine.slide;
                }

                break;
            case enemyType.tackler:
                m_State = stateMachine.tackle;
                break;
            case enemyType.meleeFollow:
                m_State = stateMachine.follow;
                break;
            case enemyType.distanceBlock:
                m_State = stateMachine.block;
                break;
        }

        changeState(m_State);
    }


    void Tackle()
    {
        m_animator.Play("Attack");
        transform.position = Vector2.MoveTowards(transform.position, toTackle,
            tackleSpeed * Time.deltaTime);
    }


    void FollowPlayer()
    {
        if (Vector2.Distance(transform.position, m_player.transform.position) > stoppingDistance)
        {

            transform.position = Vector2.MoveTowards(transform.position, m_player.transform.position,
                speed * Time.deltaTime);

        }


        /*else if (Vector2.Distance(transform.position, m_player.transform.position) < retreadDistance)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, m_player.transform.position, -speed * Time.deltaTime);
        }*/
    }


    void Shoot(int x)
    {
        m_animator.Play("Attack");
        var newProjectile = Instantiate(projectile[x], transform.position, Quaternion.identity, transform);
        newProjectile.transform.parent = gameObject.transform.parent;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_Type == enemyType.meleeFollow || m_Type == enemyType.distanceBlock)
        {
            playerInArea = true;
            timeInArea = 0f;
        }

        if (m_Type == enemyType.tackler && other.CompareTag("Wall"))
        {
                m_State = stateMachine.rest;
                changeState(m_State);
        }


        if (other.CompareTag("Player") && attacking)
        {

       PlayerAnimation.instance.Hurt(transform.position.x);
                m_State = stateMachine.idle;
                changeState(m_State);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (m_Type == enemyType.meleeFollow || m_Type == enemyType.distanceBlock)
        {
            if (playerInArea) timeInArea += Time.deltaTime;

            if (other.CompareTag("Player") && attacking)
            {
                PlayerManager.instance.ReduceHealth(this.transform.position);
                if (m_Type == enemyType.meleeFollow) m_State = stateMachine.follow;
                else m_State = stateMachine.retread;
                changeState(m_State);
                attacking = false;
                //if (m_Type == enemyType.meleeFollow) 
            }
        }
    }
    
  
    private void OnTriggerExit2D(Collider2D other)
    {
        if (m_Type == enemyType.meleeFollow || m_Type == enemyType.distanceBlock)
        {
            playerInArea = true;
            timeInArea = 0f;
        }
    }

    protected override void Die()
    {
        
    }
    
}