using nicorueda.Player; // Asegúrate de que esto sea necesario
using UnityEngine;

namespace nicorueda
{
    public class EnemyManager : CharacterBase 
    {
        // --- CAMBIO 1: Convertir Propiedades en Campos ---
        // Ahora [Header] y [SerializeField] apuntan a un CAMPO privado (ej: _ai)
        // La propiedad pública (ej: AI) solo "lee" ese campo.

        [Header("Referencias (Autodetectadas)")]
        [SerializeField] private EnemyAI_Base _ai;
        [SerializeField] private EnemyAttack _attack;
        [SerializeField] private EnemyAnimation _animation;
        
        [Header("Referencias (Componentes)")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Animator _anim;
        [SerializeField] private SpriteRenderer _spriteRend;
        [SerializeField] private Transform _player;

        // --- Propiedades públicas de solo lectura ---
        // Otros scripts (como la IA) leerán esto
        public EnemyAI_Base AI => _ai;
        public EnemyAttack Attack => _attack;
        public EnemyAnimation Animation => _animation;
        public Rigidbody2D Rb => _rb;
        public Animator Anim => _anim;
        public SpriteRenderer SpriteRend => _spriteRend;
        public Transform Player => _player;
        
        
        protected override void Awake()
        {
            base.Awake(); 

            // --- CAMBIO 2: Asignar a los campos privados ---
            _player = FindObjectOfType<PlayerManager>().transform; 

            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
            _spriteRend = GetComponent<SpriteRenderer>();
            
            _ai = GetComponent<EnemyAI_Base>();
            _attack = GetComponent<EnemyAttack>();
            _animation = GetComponent<EnemyAnimation>();
            
            if(_ai == null) Debug.LogError("Este enemigo no tiene CEREBRO (EnemyAI_Base)", gameObject);
            if(_attack == null) Debug.LogError("Este enemigo no tiene PUÑOS (EnemyAttack)", gameObject);
            if(_animation == null) Debug.LogError("Este enemigo no tiene CARA (EnemyAnimation)", gameObject);
        }

        // Lógica de muerte COMÚN a todos los enemigos
        protected override void Die()
        {
            isDead = true; 
            
            // --- CAMBIO 3: Leer desde las propiedades públicas (esto funciona igual) ---
            AI.enabled = false; 
            Rb.velocity = Vector2.zero;
            Rb.isKinematic = true; 
            GetComponent<Collider2D>().enabled = false; 
            
            LevelManager.instance.SubstractEnemy(); 
            
            Destroy(gameObject, 3f);
        }
    }
}