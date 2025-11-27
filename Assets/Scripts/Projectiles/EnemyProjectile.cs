using UnityEngine;
using nicorueda;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        // Destruir la flecha después de X segundos para no llenar la memoria
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Mover la flecha hacia la derecha (su propio "frente")
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si choca con el Jugador
        if (other.CompareTag("Player"))
        {
            CharacterBase player = other.GetComponent<CharacterBase>();
            if (player != null)
            {
                player.TakeDamage(damage, transform.position);
            }
            Destroy(gameObject); // La flecha desaparece
        }
        // Si choca con un Muro (Layer "Wall")
        else if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject); // La flecha se clava/desaparece
        }
    }
}