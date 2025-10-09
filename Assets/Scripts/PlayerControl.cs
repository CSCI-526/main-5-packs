using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 10.0f;
    public int maxJumps = 2;
    private int jumpCount;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumps;
    }

    void Update()
    {
        if (gameObject.name == "Player"
                && Input.GetKeyDown(KeyCode.UpArrow) 
                && jumpCount > 0) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount--;
        } 
    }

    private void FixedUpdate()
    {
        float moveHorizontal = 0f;

        if (gameObject.name == "Player") {
            if (Input.GetKey(KeyCode.RightArrow))
                moveHorizontal = 1f;
            else if (Input.GetKey(KeyCode.LeftArrow))
                moveHorizontal = -1f;
        } 

        rb.linearVelocity = new Vector2(moveHorizontal * speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpCount = maxJumps; // You can double jump from anywhere

        ContactPoint2D contact = collision.contacts[0];
        
    }
}

