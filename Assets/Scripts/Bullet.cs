using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float timeUntilDestroy = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        Destroy(gameObject, timeUntilDestroy);

        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //PlayerMovement.LoseALife();
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}