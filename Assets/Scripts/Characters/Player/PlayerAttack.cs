using UnityEngine;
using UnityEngine.Serialization;

namespace nicorueda.Player
{
    public class PlayerAttack : PlayerManager
    {
        private static PlayerAttack _instance;
    
        public new static PlayerAttack instance{
            get{
                if(_instance == null){
                    Debug.Log("Player Manager is Null!!!");
                }
                return _instance;
            }
        }
    

        [SerializeField]int meleeDamage, distanceDamage, mageDamage; 

        //veces qe podemos atacar por segundo, segundos a esperar para el siguiente ataque
        float attackRate = 1f, nextAttackTime = 0f;
  

        float bulletDistanceForce = 10f, bulletMageForce = 20f;
        //variables distancia
        [SerializeField] Transform rangePoint;
        [SerializeField] GameObject bulletPrefab;


        //variables magia
        [SerializeField] Transform spellPoint;
        [SerializeField] GameObject spellPrefab;

        [SerializeField] private AudioSource ataque_melee;
        [SerializeField] private AudioSource ataque_escupir;

        [FormerlySerializedAs("attackingMele")] public bool canAttack, attackingMele;


        private void Awake()
        {
            canAttack = false;
            _instance = this;


        }

        private void Update()
        {
            if(Time.time > nextAttackTime && !canAttack)
            {
                canAttack = true;
            }
        }


        public void AttackingAsMelee(){
            ataque_melee.Play();
            attackingMele = true;
        }


    
        public void AttackingAsDistance(){
            ataque_escupir.Play();
            GameObject spell = Instantiate(spellPrefab, spellPoint.position, spellPoint.rotation);
            Rigidbody2D rb = spell.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(bulletMageForce * PlayerMovement.instance.Direction() , 0) , ForceMode2D.Impulse);

        }

        public void AttackinAsMage(){

            Vector2 pointTo = PlayerMovement.instance.ReturnMove();
            if ((pointTo.magnitude) == 0)
            {
                pointTo = new Vector2(0, 1);
            }


            pointTo.Normalize();

            GameObject bullet = Instantiate(bulletPrefab, rangePoint.position, rangePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(pointTo * bulletDistanceForce, ForceMode2D.Impulse);
            nextAttackTime = Time.time + attackRate;
        }


        private void OnTriggerStay2D(Collider2D enemies)
        {
            if (attackingMele)
            {
                if(enemies.CompareTag("Enemy")){
                    Debug.Log("le hiso danyo wwwwwwwwwwwwwy");
                    enemies.gameObject.GetComponent<EnemyController2>().ReduceHealth(this.transform.position);
                }
                if(enemies.CompareTag("BossBody")){
                    enemies.GetComponent<Body>().ReduceHealth();
                }
                if(enemies.CompareTag("Sword")){
                    enemies.GetComponent<Sword>().ReduceHealth(this.transform.position);
                }
                else if(enemies.CompareTag("EnemyProjectile")){
                    Destroy(enemies.gameObject);
                }
            
                PlayerManager.instance.RestoreMana(35);
            }
            attackingMele = false;
        }


        public void NoAttack()
        {
            attackingMele = false;
        }
    
    }
}
