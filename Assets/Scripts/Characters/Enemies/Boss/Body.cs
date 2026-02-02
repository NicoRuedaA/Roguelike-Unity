using System.Collections;
using System.Collections.Generic;
using nicorueda.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 




public class Body : MonoBehaviour
{

    [SerializeField] AudioSource boss_tp;
    [SerializeField] AudioSource escupir;
    [SerializeField] AudioSource mano_mas_pisoton;
    [SerializeField] AudioSource heavy_espada;
    [SerializeField] AudioSource heavy;
    [SerializeField] AudioSource levitar;
    [SerializeField] AudioSource vulnerable_sonido;

    private static Body _instance;
    
    public static Body instance{
        get{
            if(_instance == null){
                Debug.Log("Body is Null!!!");
            }
            return _instance;
        }
    }


    [SerializeField] int health = 25;

    public enum stateMachine
    {
        fase1,
        follow,
        rest,
        idle,
        punchMelee,
        punchEarth,
        moveSword,
        attackSwordEarth,
        attackSwoordNoEffect,
        attackSwordLateral,
        restingFromPunch,
        tPose,
        shoot,
    }

    public enum phase
    {
        phase1,
        phase2,
    }
    
    

    [SerializeField] float attackRange, detectRange;
    

    GameObject m_sword, m_player;
    int attack = 0;

    public stateMachine m_State;
    public phase m_Phase;

    public float speed = 10f;

    //variables de la epxlosion
    float cumSpeed = 2f;
    [SerializeField] GameObject objToSpawn;
    float radius = 6f;
    float degree = 360f;
    float numberOfSpawns = 15f;
    float nextSpawnTime = 2f;
    float spawnTimer = 0;
    float direction = 1;

    //variabbles state machine
    bool vulnerable, attacking, playerHitted    ;

    private float timeRolling = 1.5f,
        timePosingT = 1.5f,
        timeRestitng = 2f,
        timeRestitngPunch = 4f,
        timeFollowing = 4f,
        timePunching = 0.5f,
        timePunchingMelee = 2.5f,
        timePunchInEarth = 2f,
        timeMoveSword = 1f,
        timeToEndState;

    //variables de tp + putaso
    Vector3 newPosition, oldPosition;


    int followProb = 3 , punchProb = 6;

    [SerializeField] private float stoppingDistance, retreadDistance;

    [SerializeField ] float timeBetweenTP = 15f, timeToAttackPhase1= 7;
    float timeToTP;

    public TMP_Text counterText;
    
    SpriteRenderer m_sprite;


    
    private void Start()
    {
        _instance = this;


        m_sprite = GetComponent<SpriteRenderer>();
        //m_sword = transform.parent.transform.Find("Sword");
        //ParentGameObject.transform.GetChild (0).gameObject; 
        m_sword = GameObject.FindGameObjectWithTag("Sword");
        m_player = GameObject.FindGameObjectWithTag("Player");

        //prepara fase 1
        vulnerable = false;
        attacking = false;
        m_State = stateMachine.idle;
        m_Phase = phase.phase1;
        changeState(m_State);

        stoppingDistance = 6f;
        retreadDistance = 4f;
        speed = 6;
        timeToTP = timeBetweenTP;
    }


    private void FixedUpdate()
    {
        
        if(Time.frameCount % 60 == 0 )
        {
            Debug.Log("Tiempo actual :" + Time.time + "Tiempo para cambiar  :" +  timeToEndState);
        }
        
        m_sprite.color = new Color (1, 0, 0, 1);

        counterText.text = "Body: " + health.ToString();
        //+ " Sword: " + m_sword.GetComponent<Sword>().Returnhealth();
        

        if (Time.time > timeToTP){
            Teleport();
            m_State = stateMachine.idle;
            changeState(m_State);
        }
        
        if (m_Phase == phase.phase1){
                switch (m_State)
                {
                    case stateMachine.idle:
                        followPlayer();
                        if (Time.time > timeToEndState)
                        {
                            newPosition = new Vector3(m_player.transform.position.x, m_player.transform.position.y + 1.5f);
                            m_State = stateMachine.moveSword;
                            changeState(m_State);
                        }
                        break;
                    case stateMachine.moveSword:
                        if (Time.time > timeToEndState)
                        {
                            attackWithSword();
                            m_State = stateMachine.attackSwordEarth;
                            changeState(m_State);
                        }
                        break;
                    case stateMachine.attackSwordEarth:
                        if (Time.time > timeToEndState)
                        {
                            m_State = stateMachine.attackSwoordNoEffect;
                            changeState(m_State);
                        }
                        break;
                    case stateMachine.attackSwoordNoEffect:
                        if (Time.time > timeToEndState)
                        {
                            m_State = stateMachine.idle;
                            changeState(m_State);
                        }
                        break;
                }
        }

        else if (m_Phase == phase.phase2){
                switch (m_State)
                {
                    case stateMachine.follow:
                        followPlayer();
                        //Debug.Log(Time.time + " " + timeToEndState);
                        if (playerHitted)
                        {
                            Debug.Log("deberia reproducirse el putaso");
                            m_State = stateMachine.punchMelee;
                            changeState(m_State);
                        }
                        else if (Time.time > timeToEndState)
                        {
                            Debug.Log("deberia descansar");
                            m_State = stateMachine.rest;
                            changeState(m_State);
                        }

                        break;

                    case stateMachine.punchEarth:
                        if (Time.time > timeToEndState)
                        {
                            if (playerHitted) m_State = stateMachine.idle;
                            else m_State = stateMachine.restingFromPunch;
                            changeState(m_State);

                        }

                        break;

                    case stateMachine.moveSword:
                        if (Time.time > timeToEndState)
                        {
                            attackWithoutSword();
                            m_State = stateMachine.punchEarth;
                            changeState(m_State);
                        }

                        break;

                    case stateMachine.punchMelee:
                        if (Time.time > timeToEndState)
                        {
                            m_State = stateMachine.idle;
                            changeState(m_State);
                        }

                        break;

                    case stateMachine.tPose:
                        if (Time.time > timeToEndState)
                        {
                            Cumeada();
                            m_State = stateMachine.shoot;
                            changeState(m_State);
                        }

                        break;
                    case stateMachine.shoot:
                        if (Time.time > timeToEndState)
                        {
                            Roll();
                        }

                        break;
                    case stateMachine.rest:
                        if (Time.time > timeToEndState)
                        {
                            Roll();
                        }

                        break;
                    case stateMachine.idle:
                        if (Time.time > timeToEndState)
                        {
                            Roll();
                        }

                        break;
                    case stateMachine.restingFromPunch:
                        if (Time.time > timeToEndState)
                        {
                            transform.position = oldPosition;
                            m_State = stateMachine.idle;
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
            case stateMachine.follow:
                BodyAnimationController.sharedInstance.animPlayIdle();
                attacking = true;
                vulnerable = false;
                timeToEndState = Time.time + timeFollowing;
                break;
            case stateMachine.punchMelee:
                BodyAnimationController.sharedInstance.animPlayPunch();
                //MARC
                //mano_mas_pisoton.Play();
                timeToEndState = Time.time + timePunchingMelee;
                attacking = true;
                vulnerable = false;
                break;
            case stateMachine.restingFromPunch:
                BodyAnimationController.sharedInstance.animRestAttackPunchEarth();
                timeToEndState = Time.time + timeRestitngPunch;
                attacking = false;
                vulnerable = true;
                vulnerable_sonido.Play();
                break;
            case stateMachine.punchEarth:
                //MARC
                //heavy.Play();
                timeToEndState = Time.time + timePunchInEarth;
                attacking = true;
                vulnerable = false;
                break;
            case stateMachine.moveSword:
                //if(m_Phase==phase.phase1)m_sword.SetActive(false);
                BodyAnimationController.sharedInstance.animPlayMoveSword();
                timeToEndState = Time.time + timeMoveSword;
                attacking = false;
                vulnerable = false;
                break;
            case stateMachine.tPose:
                BodyAnimationController.sharedInstance.animPlaytPose();
                vulnerable = false;
                attacking = false;
                timeToEndState = Time.time + timePosingT;
                break;
            case stateMachine.shoot:
                BodyAnimationController.sharedInstance.animPlayRest();
                vulnerable = true;
                vulnerable_sonido.Play();
                attacking = false;
                timeToEndState = Time.time + timeRestitng;
                break;
            case stateMachine.rest:
                //if(m_Phase==phase.phase1)m_sword.SetActive(true);
                
                BodyAnimationController.sharedInstance.animPlayRest();
                vulnerable = true;
                //MARC
                //vulnerable_sonido.Play();
                attacking = false;
                timeToEndState = Time.time + timeRestitng;
                break;
            case stateMachine.attackSwordEarth:
                BodyAnimationController.sharedInstance.animPlayAttackSword();
                //MARC
                //heavy_espada.Play();
                vulnerable = false;
                attacking = true;
                timeToEndState = Time.time + timeRestitng;
                break;
            case stateMachine.attackSwoordNoEffect:
                BodyAnimationController.sharedInstance.animPlayAttackSwordNoEffect();
                vulnerable = false;
                attacking = false;
                timeToEndState = Time.time + timeRestitng;
                break;
            default:
                //if(m_Phase==phase.phase1)m_sword.SetActive(true);
                BodyAnimationController.sharedInstance.animPlayIdle();
                vulnerable = false;
                attacking = false;
                if(m_Phase == phase.phase1) timeToEndState = Time.time + timeToAttackPhase1;
                else timeToEndState = Time.time + timeRestitng;

                break;
        }

        Debug.Log(m_State);

        playerHitted = false;
    }


        void Roll()
    {
        attack = Random.Range(0, 10);
        if (attack < followProb)
        {
            m_State = stateMachine.follow;
        }
        else if (attack > punchProb)
        {
            newPosition = new Vector3(m_player.transform.position.x, m_player.transform.position.y + 1.5f);
             m_State = stateMachine.moveSword;
        }
        else
        {
            m_State = stateMachine.tPose;
        }
        changeState(m_State);
    }


    

    public void PrepareFase2()
    {
        
        m_Phase = phase.phase2;
        timeToEndState =  Time.time + timeRestitng;
        m_State = stateMachine.follow;
        changeState(m_State);
        stoppingDistance = 1f;
        retreadDistance = 1f;
        speed = 5;
    }





    void followPlayer()
    {
        Debug.Log("asdasdasd");

        if (Vector2.Distance(transform.position, m_player.transform.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_player.transform.position,
                speed / 2 * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, m_player.transform.position) < retreadDistance)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, m_player.transform.position, -(speed - 2) * Time.deltaTime);
        }
        //MARC
        //levitar.Play();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            if (m_State == stateMachine.fase1) {

            }
            else if (attacking) {
                PlayerAnimation.instance.Hurt(transform.position.x);
                
                playerHitted = true;
            }
        }
    }


    void attackWithoutSword(){
        // This will wait 1 second like Invoke could do, remove this if you don't need it


        
        BodyAnimationController.sharedInstance.animPlayAttackPunchEarth();
        oldPosition = transform.position;
        transform.position = newPosition;
        timeToEndState = Time.time + timePunching;

        //m_State = stateMachine.;
        //changeState(m_State);
    }

    void attackWithSword()
    {
                
        BodyAnimationController.sharedInstance.animPlayAttackSword();
        oldPosition = transform.position;
        transform.position = newPosition;
        timeToEndState = Time.time + timePunching;
    }

    
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }



    public void Cumeada()
    {
        
        //MARC
        //escupir.Play();
        float arclenght = (degree / 360) * 2 * Mathf.PI;
        float nextAngle = arclenght / numberOfSpawns;
        float angle = 0;
        for (int i = 0; i < numberOfSpawns; i++)
        {
            float x = Mathf.Cos(angle) * radius * direction;
            float y = Mathf.Sin(angle) * radius * direction;

            var obj = Instantiate(objToSpawn, transform.position, Quaternion.identity);
            var rb = obj.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = new Vector2(x, y) * cumSpeed;
            angle += nextAngle;

            Destroy(obj, 2f);
        }
    }

    public GameObject[] GameObjectList;

    void Teleport()
    {
        //MARC
        //boss_tp.Play();
        float FurthestDistance = 0;
        GameObject FurthestObject = null;
        foreach (GameObject Object in GameObjectList)
        {
            float ObjectDistance = Vector3.Distance(transform.position, Object.transform.position);
            if (ObjectDistance > FurthestDistance)
            {
                FurthestObject = Object;
                FurthestDistance = ObjectDistance;
            }
        }

        transform.position = FurthestObject.transform.position;
        timeToTP = Time.time + timeBetweenTP;
    }


    public void ReduceHealth()
    {
        if (vulnerable)
        {
            health--;
            if (health <= 0)
            {
                //acabar partida
                Destroy(gameObject);
            }
        }

    }
}