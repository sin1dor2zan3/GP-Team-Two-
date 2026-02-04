using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int lives;
    public float moveSpeed;
    public float turnSpeed;

    public float shootCooldown;
    private float lastShootTime;

    //private GameManager gameManager;

    public GameObject bulletPrefab;
    public Transform barrel;

    void Start()
    {
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lives = 3;
        moveSpeed = 2.0f;
        turnSpeed = 0.5f;
        shootCooldown = 1.0f;
        lastShootTime = -shootCooldown;

        // If this tank is Player2, face left at the start
        if (CompareTag("Player2"))
        {
            transform.rotation = Quaternion.Euler(0, 0, 180f);
        }
    }

    void Update()
    {
        Movement();
        Shooting();
    }

    // public void LoseALife()
    // {
    //     lives--;
    //     if (lives == 0)
    //     {
    //         //load game over scene
    //     }
    // }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    void Shooting()
    {
        if (barrel != null)
        {
            bool canShoot = Time.time - lastShootTime >= shootCooldown;
            if (canShoot)
            {
                if (CompareTag("Player1") && Input.GetKeyDown(KeyCode.C))
                {
                    Instantiate(bulletPrefab, barrel.position, barrel.rotation);
                    lastShootTime = Time.time;
                }
                else if (CompareTag("Player2") && Input.GetKeyDown(KeyCode.N))
                {
                    Instantiate(bulletPrefab, barrel.position, barrel.rotation);
                    lastShootTime = Time.time;
                }
            }
        }
    }

    void Movement()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (CompareTag("Player1"))
        {
            // WASD controls: W = up, S = down, A = left, D = right
            if (Input.GetKey(KeyCode.D))
            {
                moveX = 1;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                moveX = -1;
            }
            else
            {
                moveX = 0;
            }

            if (Input.GetKey(KeyCode.W))
            {
                moveY = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveY = -1;
            }
            else
            {
                moveY = 0;
            }
        }
        else if (CompareTag("Player2"))
        {
            // IJKL controls: I = up, K = down, J = left, L = right
            if (Input.GetKey(KeyCode.L))
            {
                moveX = 1;
            }
            else if (Input.GetKey(KeyCode.J))
            {
                moveX = -1;
            }
            else
            {
                moveX = 0;
            }

            if (Input.GetKey(KeyCode.I))
            {
                moveY = 1;
            }
            else if (Input.GetKey(KeyCode.K))
            {
                moveY = -1;
            }
            else
            {
                moveY = 0;
            }
        }

        Vector2 moveDir = new Vector2(moveX, moveY);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            // Calculate the angle (in degrees) the player should face based on input direction
            float targetAngle = Mathf.Atan2(moveY, moveX) * Mathf.Rad2Deg;
            // Get the current z rotation of the player
            float currentAngle = transform.eulerAngles.z;
            // Find the shortest difference between current and target angle
            float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);
            // Calculate the maximum angle we can rotate this frame (degrees/second * deltaTime)
            float maxStep = turnSpeed * 360f * Time.deltaTime; // turnSpeed is in [0,1], scale to degrees/sec
            float newAngle;

            // If the angle difference is small, snap to the target angle
            if (Mathf.Abs(angleDiff) < maxStep)
            {
                newAngle = targetAngle;
            }
            else
            {
                // Otherwise, rotate toward the target angle at the allowed speed
                newAngle = currentAngle + Mathf.Sign(angleDiff) * maxStep;
            }
            
            // Apply the new rotation
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }

        // Move the player in the input direction at the specified speed
        transform.position = (Vector2)transform.position + moveDir * moveSpeed * Time.deltaTime;
    }
}