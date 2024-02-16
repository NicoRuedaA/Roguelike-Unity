using UnityEngine;
using UnityEngine.SceneManagement;


namespace nicorueda
{
    public abstract class CharacterController : MonoBehaviour
    {
    
        protected bool attacking, vulnerable;
        protected int speed, runSpeed, stamina, health, mana, manaToReduce = 1, staminaToReduce = 1;


        [SerializeField] private int INITIAL_HEALTH = 5,
            INITIAL_SPEED = 3,
            INITIAL_RUNSPEED = 8,
            INITIAL_MANA = 4,
            INITIAL_STAMINA = 150;

        protected int InitialSpeed() => INITIAL_SPEED;

        
        //static, ya que todos los personajes tendrán los mismos limites de movimiento
        protected int MAX_MANA, MIN_MANA, MIN_HEALTH, MAX_HEALTH, MAX_STAMINA,MIN_STAMINA;
        
        
//Physics variables
        float pushForce = 1000;

//Projectiles variables
        [SerializeField] int skullsToInstantiate;
        [SerializeField] GameObject skullPrefab; 

        /*[SerializeField] AudioSource hurt_sound;
    [SerializeField] AudioSource death_sound;*/ 
        protected void PrepareCharacter()
        {
            skullsToInstantiate = 0;
            SetStats();
        }
        
        public bool ReduceMana()
        {
            bool aux = true;
            //el mana no puede ser negativo
            if (this.mana - manaToReduce>=MIN_MANA){
                this.mana -= manaToReduce;
                
                //Mana.sharedInstance.ReduceCharge(PlayerManager.instance.GetMana());
                
                //Debug.Log("mana actual: " + mana);
            
            }
            else {
                aux = false;}
            return aux;
        }

        public bool ReduceStamina()
        {
            bool aux = true;
            if (this.stamina - this.staminaToReduce>=MIN_STAMINA){
                this.stamina -= this.staminaToReduce;
                //Debug.Log("energia actual: " + stamina);
            }
            else aux = false;
            return aux;
        }
        
        public void RestoreMana(int x)
        {
            this.mana += x;
            if(this.mana>MAX_MANA) this.mana=MAX_MANA;
        }

        public void RestoreStamina(int bonus)
        {
            this.stamina += bonus;
            if(this.stamina>MAX_STAMINA) this.stamina=MAX_STAMINA;
        }
        
        public void IncreaseHealthMax(int x){
            this.MAX_HEALTH += x;
            this.health += x;
        }

        public void IncreaseManaMax(){
            this.MAX_MANA += manaToReduce;
            this.mana += manaToReduce;
            //Mana.sharedInstance.addCharge(PlayerManager.instance.GetMana()-1);
        }
      
      
        public void IncreaseStaminaMax(int bonus){
            this.MAX_STAMINA += bonus;
            this.stamina += bonus;
        }
        

        public void SetStats(){

            MIN_HEALTH=0;
            MIN_STAMINA=0;
            MIN_MANA = 0;

            MAX_HEALTH = INITIAL_HEALTH;
            MAX_MANA = INITIAL_MANA;
            MAX_STAMINA = INITIAL_STAMINA;

            speed = INITIAL_SPEED;
            health = INITIAL_HEALTH;
            mana = INITIAL_MANA;
            stamina = INITIAL_STAMINA;
            runSpeed = INITIAL_RUNSPEED;
        }

        public void ReduceHealth(Vector3 vec)
        {
            float xPos = vec.x;
            health = health -1;
        
            if(health <= 1 ){
                Die();
            }

            else
            {
                //hurt transform. Deberia llamar al player manager y el player moverlo, cambiar le estado y la animacion
                if(xPos > transform.position.x){
                    gameObject.transform.position = new Vector3(transform.position.x-0.5f, transform.position.y, transform.position.z);
                } else {
                    gameObject.transform.position = new Vector3(transform.position.x+0.5f, transform.position.y, transform.position.z);
                }
            }
            //hurt_sound.Play();
        }

        /*if (gameObject.CompareTag("Player"))
          {
              SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
              //AQUI DEBE IR CARGAR LA ESCENA DE MUERTE
              //ELIMINA LA LINEA DE TIME.TIMESCALE = 0
          }

          else
          {

              if(gameObject.CompareTag("Enemy")){
                  LevelManager.instance.SubstractEnemy();
              }

              else if(gameObject.CompareTag("Sword")){

                  //GameObject m_sword = GameObject.FindWithTag("Sword");
                  //m_sword.SetActive(false);
                  Body.instance.PrepareFase2();
              }
              Destroy(gameObject);

          }

          for(int i = 0; i>skulls; i++){
              var positionToInstantiate = transform.position;
              var skullsObject = Instantiate(skullPrefab, positionToInstantiate, Quaternion.identity);
              //se instancian una encima de otra
              //posibilidad de moverlas

          }*/
        protected abstract void Die();
        
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        } 
        public int RunSpeed
        {
            get { return runSpeed; }
            set { runSpeed = value; }
        } 
        public int Stamina
        {
            get { return stamina; }
            set { stamina = value; }
        } 
        public int Health
        {
            get { return health; }
            set { health = value; }
        } 
        public int Mana
        {
            get { return mana; }
            set { mana = value; }
        }

        public int ManaToReduce
        {
            get { return manaToReduce; }
            set { manaToReduce = value; }
        }

        public int StaminaToReduce
        {
            get { return staminaToReduce; }
            set { staminaToReduce = value; }
        }
        

    }
}
