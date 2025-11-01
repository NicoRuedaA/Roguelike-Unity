using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  /*public Transform player;
  public float yOffset, xOffset;
  public float followSpeed = 2f;

  void LateUpdate ()
  {
    Vector3 newPos = new Vector3(player.position.x, player.position.y + yOffset, - 10f);
    transform.position = Vector3.Slerp(transform.position, newPos, followSpeed*Time.deltaTime);
  }*/

  public GameObject followObject;
  public Vector2 followOffset;
  public float speed = 3f;
  private Vector2 threshold;
  private Rigidbody2D rb;
  float xDifference;
  float yDifference;

  // Start is called before the first frame update
  void Start()
  {
    threshold = calculateThreshold();
    rb = followObject.GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  void LateUpdate()
  {
    Vector2 follow = followObject.transform.position;
    xDifference = Mathf.Abs(transform.position.x - follow.x);
    yDifference = Mathf.Abs(transform.position.y - follow.y);

    Vector3 newPosition = transform.position;
    if (xDifference >= threshold.x)
    {
      newPosition.x = follow.x;
    }
    if (yDifference >= threshold.y)
    {
      newPosition.y = follow.y;
    }
    float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;
    transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);

    /* EJE Z
    newPosition.z = transform.position.z; // Asegura que la Z no cambie (ej. -10)
transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);*/
  }
  private Vector3 calculateThreshold()
  {
    Rect aspect = Camera.main.pixelRect;
    Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
    t.x -= followOffset.x;
    t.y -= followOffset.y;
    return t;
  }
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.blue;
    Vector2 border = calculateThreshold();
    Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
  }
}
