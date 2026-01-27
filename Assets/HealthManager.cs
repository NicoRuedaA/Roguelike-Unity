using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{
    using System.Collections.Generic;
    using UnityEngine;

    public class HealthManager : MonoBehaviour
    {
        [Header("Configuración")]
        public GameObject heartPrefab;
        public Transform heartContainer;
        private Vector2 heartPos;

        [Header("Ajustes de Barra")]
        public float xOffset = 1f; // Los "X" píxeles de desplazamiento

        private List<GameObject> hearts = new List<GameObject>();


        private void Start()
        {
            Transform containerRect = heartContainer.GetComponent<Transform>();

            // 2. Accedemos a la posición X e Y
            float posX = containerRect.position.x;
            float posY = containerRect.position.y;

            heartPos = new Vector2(posX, posY);


            if (PlayerManager.instance != null)
            {
                int initialHealth = PlayerManager.instance.Health;

                InitializeHealth(initialHealth);
            }
            else
            {
                Debug.LogError("No se encontró PlayerManager");
            }
        }

        public void InitializeHealth(int x)
        {




            foreach (GameObject heart in hearts) Destroy(heart);
            hearts.Clear();

            for (int i = 0; i < x; i++)
            {
                AddLife();
            }
        }

        public void AddLife()
        {

            float newX = hearts.Count * xOffset; // Posición relativa al padre

            // Instanciamos directamente en la posición local deseada
            GameObject newHeart = Instantiate(heartPrefab, heartContainer);
            newHeart.transform.localPosition = new Vector3(newX, 0, 0);

            hearts.Add(newHeart);


        }

        public void RemoveLife()
        {
            if (hearts.Count > 0)
            {
                GameObject lastHeart = hearts[hearts.Count - 1];
                hearts.Remove(lastHeart);
                Destroy(lastHeart);
            }
        }
    }
}
