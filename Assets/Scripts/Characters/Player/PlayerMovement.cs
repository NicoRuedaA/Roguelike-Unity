using UnityEngine;

namespace nicorueda.Player
{
    public class PlayerMovement : PlayerManager
    {
        private static PlayerMovement _instance;
    
        public new static PlayerMovement instance{
            get{
                if(_instance == null){
                    Debug.Log("Player Manager is Null!!!");
                }
                return _instance;
            }
        }

    
        private Vector2 movement, mousePos;
        private Rigidbody2D rb;
        private SpriteRenderer m_mySpriteRenderer;
        private LineRenderer m_lr;

        private int sprintIncrease = 5;

        //variables para pulsardos veces
        float sprintForce = 5000f;
    

        float angle;
        [SerializeField] Vector2 lookDir;


       /* [SerializeField] private AudioSource caminar_sound;
        [SerializeField] private AudioSource correr_sound;*/

        private void Awake() {
            _instance = this;

            rb=GetComponent<Rigidbody2D>();
            m_mySpriteRenderer = GetComponent<SpriteRenderer>();
            m_lr = GetComponent<LineRenderer>();

            running = false;
        }
    
        public void MoveLogicMethod(Vector2 moveGet){
            movement = moveGet;
        }

        public Vector2 ReturnMove(){
            return movement;
        }

    
        private void FixedUpdate() {
            //apuntado
            //in scene
            //Debug.DrawRay(transform.position, new Vector2(movement.x*10, movement.y*10) , Color.green);
            //in game
        
            //fase de apuntar y fase de disaprar
            if(pointing){

                m_lr.enabled = true;
                m_lr.SetPosition(0,  transform.position);
                m_lr.SetPosition(1, new Vector3(transform.position.x + movement.x*7, transform.position.y + movement.y*7,0));
            }

            else{
                m_lr.enabled = false;
                rb.MovePosition(rb.position + movement * (PlayerManager.instance.Speed * Time.fixedDeltaTime));
                lookDir = mousePos - rb.position;
                angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            
                if(movement.x>0) m_mySpriteRenderer.flipX = true;
                else if(movement.x<0) m_mySpriteRenderer.flipX = false;
            }
        }

        public int Direction()
        {
            return m_mySpriteRenderer.flipX ? 1 : -1;
        }
    
    
        void RecoverStamina()
        {
            print("revisar funcion RecoverStamina");
            //PlayerManager.instance.RestoreStamina(10*Time.deltaTime);
        }


        void RecoverMana(){
            print("revisar funcion RecoverMana()");
            //PlayerManager.instance.RestoreMana(35);
        }


        public void Pointing(){
            print("revisar funcion pointing");
            //if(PlayerManager.instance.ReduceStamina(25)) apuntando = true;
        }

        public void Walking(){
           // caminar_sound.Play();
           
            //llamar a player manager y que haga cosas
        }

        public void Sprint()
        {
            /*if (PlayerManager.instance.ReduceStamina(25) && !apuntando)
        {
            rb.AddForce(new Vector2(movement.x*sprintForce/4, movement.y*sprintForce/4), ForceMode2D.Force);   
        }*/
        
            print("revisar funcion Sprinting()");
        }

  
    }
}
