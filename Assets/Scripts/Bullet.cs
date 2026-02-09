using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float timeUntilDestroy = 5f;

    private Rigidbody2D rb;
    private int playerLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.NameToLayer("Player");
    }

    void Start()
    {
        Destroy(gameObject, timeUntilDestroy);
        rb.linearVelocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hit = collision.gameObject;

        if (hit.CompareTag("Player1") ||
            hit.CompareTag("Player2") ||
            hit.CompareTag("Enemy"))
        {
            GameManager.Instance.TankDestroyed(hit.tag);
            Destroy(hit);
            Destroy(gameObject);
        }
        else if (hit.CompareTag("Projectile"))
        {
            Destroy(hit);
            Destroy(gameObject);
        }
    }
}