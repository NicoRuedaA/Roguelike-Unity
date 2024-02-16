using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomFlipX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer mySpriteRenderer = GetComponent<SpriteRenderer>();
        bool flip  = (Random.value > 0.5f);
        mySpriteRenderer.flipX = flip;
    }

}
