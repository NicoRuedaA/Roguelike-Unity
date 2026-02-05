using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{
    public class DoorManager : MonoBehaviour
    {

        private BoxCollider2D miCollider;
        private SpriteRenderer miRenderer;

        // Start is called before the first frame update
        private void Awake()
        {

            miCollider = GetComponent<BoxCollider2D>();
            miRenderer = GetComponent<SpriteRenderer>();
            LevelManager.instance.InsertDoor(gameObject);

            // 2. Llamamos a la función para que empiece "desactivada"
            Open();
        }


        public void Close()
        {
            if (miRenderer != null) miRenderer.enabled = true;
            if (miCollider != null) miCollider.enabled = true;
            Debug.Log("La puerta se ha activado: Ahora es sólida y visible.");
        }

        // Función para que el muro "No exista" (Invisible y se traspasa)
        public void Open()
        {
            if (miRenderer != null) miRenderer.enabled = false;
            if (miCollider != null) miCollider.enabled = false;
            Debug.Log("La puerta se ha desactivado: No se ve y se traspasa.");
        }


    }
}
