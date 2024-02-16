using System.Collections;
using System.Collections.Generic;
using nicorueda.Player;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform m_Player;
    private Vector2 target;

    [SerializeField] bool isExplosive, followPlayer;
    bool exploding, exploited;
    [SerializeField] float explosionRange;

    Animator m_Animator;

    [SerializeField] private float TimeToLive = 50f;
    public int dmg = 10;


    public float offset;


    private Vector3 thisPos;
    private float angle;
    private Vector2 toShot;

    [SerializeField] float maxDistance;
    private Rigidbody2D m_Rb;

    private void Start()
    {

        m_Animator = GetComponent<Animator>();

        //Destroy(gameObject, TimeToLive);
        //timeSpawned = Time.deltaTime;



        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(m_Player.position.x, m_Player.position.y);
        toShot = new Vector2(m_Player.position.x-transform.position.x, m_Player.position.y-transform.position.y);



        //limpiar codigo
        Transform toPoint = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Vector3 targetPos = toPoint.position;
        thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));

        m_Rb = GetComponent<Rigidbody2D>();
        
        toShot = new Vector2(m_Player.transform.position.x, m_Player.transform.position.y);

        //toShot.Normalize();


    }

    public int frameInterval = 60;
    
    private void FixedUpdate()
    {


        //if (Time.frameCount % frameInterval == 0) Debug.Log(target);


        if (!exploited) {

                
                //target.Normalize();
                
                //Vector2 direction = new Vector2(m_Player.transform.position.x - m_Player.transform.position.y)
                //target *= maxDistance;
                //Vector2 newTarget = target * maxDistance;
                //m_Rb.AddForce(direction, ForceMode2D.Impulse);
                transform.position = Vector2.MoveTowards(transform.position, toShot,
                    speed* Time.deltaTime);
                
                if(Vector3.Distance(transform.position, toShot) < 0.1f){
                    if(!isExplosive) Destroy(gameObject);
                    else
                    {
                        m_Animator.Play("Explosion");
                        StartCoroutine(DestrooyPro());
                    }
                }
                
                //transform.position = Vector2.MoveTowards(transform.position, target.Normalize()o, speed * Time.deltaTime);
 
        }
            
        else DetectCollision();
                    
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword") || other.CompareTag("Enemy") || (other.CompareTag("EnemyProjectile"))){ }
        else  {
            if (other.CompareTag("Player")){
                MakeDMG();
            }
            DestroyProjectile();
        }

    }






    
    //sustituir por evento al detectar que se acaba la animacion
    IEnumerator DestrooyPro(){
    yield return new WaitForSeconds(1.5f);
     DestroyProjectile();
    }






    void DetectCollision(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(this.transform.position, 
        explosionRange);

                //***** DAÑAR A ENEMIGOS *****
                foreach (Collider2D enemies in hitEnemies){
                    if(enemies.CompareTag("Player")){
                       MakeDMG();

                        /*enemies.GetComponent<EnemyController>().ReduceHealth();
                    }
                    else if(enemies.CompareTag("EnemyProjectile")){
                        Destroy(enemies.gameObject);*/
                    }
        }
    }


    void MakeDMG()
    {
 PlayerManager.instance.ReduceHealth(this.transform.position);

        if (isExplosive)
        {
            m_Animator.Play("Explosion");
            this.GetComponent<Collider2D>().enabled = false;
            exploited = true; 
            //staticCEnemy,cEnemy,PlayAnim,amm)_Animator.Play**("Resting"();: "iftotTackle< 0.1f  boobl l lcanBlock; [[serialzied\[sSerialiizeField]if (canBlock){StartCoroutine(DestrooyPro());
        }
        
        else DestroyProjectile();
    }

    
    void DestroyProjectile(){
        Destroy(gameObject);
    }


    void OnDrawGizmosSelected(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, explosionRange);
    }



}
