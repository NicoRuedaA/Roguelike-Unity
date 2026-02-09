using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{
    public class DoorManager : MonoBehaviour
    {

        private bool closed;
        private bool selected;
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
            if (!selected)
            {
                Debug.Log("Se cierra la puerta");
                if (miRenderer != null) miRenderer.enabled = true;
                if (miCollider != null) miCollider.enabled = true;

            }

        }

        // Función para que el muro "No exista" (Invisible y se traspasa)
        public void Open()
        {


            if (miRenderer != null) miRenderer.enabled = false;
            if (miCollider != null) miCollider.enabled = false;


        }

        public void Select()
        {
            this.selected = true;

        }


    }
}
