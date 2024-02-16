using UnityEngine;

namespace nicorueda.Player
{
    public class PlayerAnimation : PlayerManager
    {
        
        
        private static PlayerAnimation _instance;
        public new static PlayerAnimation instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log("Animation Manager is Null!!!");
                }

                return _instance;
            }
        }
        
        public enum AnimationState
        {
            Idle,
            Walking,
            Running,
            AttackMelee,
            AttackDistance,
            Hurt,
            Sprint,
            Pointing,
        }
        
        public static AnimationState m_State = AnimationState.Idle, m_newState = AnimationState.Idle;
        
        Animator anim;
        
        public float timeToEnd = 0.0f,
            sprintDelay = 0.5f,
            attackMageDelay = 2f,
            attackMeleeDay = 1f,
            hurtDelay = 1f,
            attDistanceDelay = 4f,
            nextAttackMageTime = 0,
            nextAttackDistanceTime = 0,
            nextAttackMeleeTime = 0,
            nextHurt = 0,
            nextSprint = 0,
            changeStateTime = 0.5f;
        
    
    //CAMBIAR, OBTENERLO DE PLAYER MANAGER
    private int initialSpeed = 3;
 


    private void Awake()
    {
        _instance = this;
        anim = GetComponent<Animator>();
    }


    public void FixedUpdate()
    {
        if (m_newState == m_State) return;
        switch (m_newState)
        {
            case AnimationState.Sprint:
                if (Time.time > timeToEnd)
                {
                    changeState(AnimationState.Idle);
                }

                break;
            case AnimationState.Hurt:
                if (Time.time > timeToEnd)
                {
                    changeState(AnimationState.Idle);
                }
                break;
            case AnimationState.Idle:
                PlayerManager.instance.RestoreStamina(1);
                PlayerManager.instance.Speed = PlayerManager.instance.RunSpeed;
                break;
            case AnimationState.Walking:
                PlayerManager.instance.Speed = PlayerManager.instance.RunSpeed;
                PlayerManager.instance.RestoreStamina(1);
                break;
            case AnimationState.Running:
                if (PlayerManager.instance.ReduceStamina())
                {
                    //velocidad actual = velocidad de correr
                    PlayerManager.instance.Speed = PlayerManager.instance.RunSpeed;
                }
                else changeState(AnimationState.Walking);

                break;

        }
    }

    


    public void changeState(AnimationState state)
    {
        switch (state)
        {
            case AnimationState.Idle:
                if (m_State != AnimationState.Pointing)
                {
                    anim.SetFloat("a_speed", 1);
                    m_State = AnimationState.Idle;
                    anim.Play("Idle");
                    PlayerAttack.instance.NoAttack();
                }

                break;

            case AnimationState.Walking:
                if ((m_State != AnimationState.Pointing))
                {
                    anim.SetFloat("a_speed", 1);

                    //PlayerManager.instance.SetSpeed(PlayerManager.instance.GetInitialSpeed());
                    m_State = AnimationState.Walking;
                    anim.Play("Running");
                    PlayerAttack.instance.NoAttack();
                }

                break;

            case AnimationState.Running:
                if (PlayerManager.instance.ReduceStamina())
                {
                    if ((m_State == AnimationState.Walking) && (m_State != AnimationState.Pointing))
                    {
                        anim.SetFloat("a_speed", 2);

                        m_State = AnimationState.Running;
                        anim.Play("Running");
                        PlayerAttack.instance.NoAttack();
                    }
                }

                break;


            case AnimationState.Sprint:
                if ((m_State == AnimationState.Walking) || (m_State == AnimationState.Running))
                {
                    if (Time.time > timeToEnd && Time.time > nextSprint)
                    {
                        PlayerMovement.instance.Sprint();
                        m_State = AnimationState.Walking;
                        anim.Play("Running");
                        timeToEnd = Time.time + changeStateTime;
                        nextSprint = Time.time + sprintDelay;
                    }
                }

                break;

            case AnimationState.Pointing:
                if ((m_State == AnimationState.Walking) || (m_State == AnimationState.Idle) ||
                    (m_State == AnimationState.Running))
                {
                    if (Time.time > timeToEnd && Time.time > nextAttackMageTime)
                    {
                        if (PlayerManager.instance.ReduceStamina())
                        {
                            PlayerMovement.instance.Pointing();
                            m_State = AnimationState.Pointing;
                            anim.Play("AttackMage");
                        }
                    }
                }

                break;


            case AnimationState.AttackMelee:
                if ((m_State == AnimationState.Walking) || (m_State == AnimationState.Idle) ||
                    (m_State == AnimationState.Running))
                {
                    if (Time.time > timeToEnd && Time.time > nextAttackMeleeTime)
                    {
                        if (PlayerManager.instance.ReduceStamina())
                        {
                            PlayerAttack.instance.AttackingAsMelee();
                            m_State = AnimationState.AttackMelee;
                            anim.Play("AttackMelee");
                            timeToEnd = Time.time + changeStateTime;
                            nextAttackMageTime = Time.time + attackMeleeDay;
                        }
                    }
                }

                break;

            case AnimationState.AttackDistance:
                if ((m_State == AnimationState.Walking) || (m_State == AnimationState.Idle) ||
                    (m_State == AnimationState.Running))
                {
                    if (Time.time > timeToEnd && Time.time > nextAttackDistanceTime)
                    {
                        if (PlayerManager.instance.ReduceStamina())
                        {

                            PlayerAttack.instance.AttackingAsDistance();
                            m_State = AnimationState.AttackDistance;
                            anim.Play("AttackDistance");
                            timeToEnd = Time.time + changeStateTime;
                            nextAttackDistanceTime = Time.time + attDistanceDelay;
                        }
                    }
                }

                break;
        }
    }



    public void Hurt(float x)
    {
        if (Time.time > timeToEnd && Time.time > nextHurt)
        {
            anim.Play("Hurted");
            PlayerManager.instance.ReduceHealth(this.transform.position);
            m_State = AnimationState.Hurt;
            timeToEnd = Time.time + 1f;
            nextHurt = Time.time + hurtDelay;
        }
    }
    }
}
    