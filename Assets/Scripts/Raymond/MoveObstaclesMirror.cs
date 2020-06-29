using UnityEngine;
using Mirror;

public class MoveObstaclesMirror : NetworkBehaviour
{
    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private Vector2 screenBounds;
    
    void Start()
    {
        rigidBody.velocity = new Vector2(-speed, 0);
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    [Server]
    void Update()
    {
        if (transform.position.x < screenBounds.x * 2) {
            NetworkServer.Destroy(gameObject);
        }
    }
    [ServerCallback]
    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.tag == "Player") {
            collision.gameObject.GetComponent<PlayerControllerMirror>().TakeDamage(1);
        }
    }
}
