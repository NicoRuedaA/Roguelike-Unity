using UnityEngine;
using nicorueda;

public abstract class EnemyProjectile : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float lifeTime = 5f;
    [SerializeField] protected float actualTime = 5f;

    protected virtual void Start()
    {
        // Destruir la flecha después de X segundos para no llenar la memoria
        Destroy(gameObject, lifeTime);
        this.actualTime = 0f;
    }

    protected virtual void Update()
    {
        // Mover la flecha hacia la derecha (su propio "frente")
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si choca con el Jugador
        if (other.CompareTag("Player"))
        {

            Destroy(gameObject); // La flecha desaparece
            if (PlayerManager.instance != null)
            {
                PlayerManager.instance.TakeDamage(damage, transform.position);

            }
            else
            {
                Debug.LogError("No se encontró PlayerManager");
            }
        }
        // Si choca con un Muro (Layer "Wall")
        else if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject); // La flecha se clava/desaparece
        }
    }
}