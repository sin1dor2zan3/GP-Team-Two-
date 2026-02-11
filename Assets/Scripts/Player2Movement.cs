using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float turnSpeed = 0.5f;

    public float shootCooldown = 1.0f;
    private float lastShootTime;

    public GameObject bulletPrefab;
    public Transform barrel;

    private Transform cachedTransform;

    void Awake()
    {
        cachedTransform = transform;
    }

    void Start()
    {
        lastShootTime = -shootCooldown;
    }

    void Update()
    {
        Movement();
        Shooting();
        QuitGame();
    }

    void QuitGame()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void Shooting()
    {
        if (barrel == null)
            return;

        if (Time.time - lastShootTime < shootCooldown)
            return;

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            Instantiate(bulletPrefab, barrel.position, barrel.rotation);
            lastShootTime = Time.time;
        }
    }

    void Movement()
    {
        float moveX = 0f;
        float moveY = 0f;

        moveX = Input.GetKey(KeyCode.RightArrow) ? 1 :
                Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;

        moveY = Input.GetKey(KeyCode.UpArrow) ? 1 :
                Input.GetKey(KeyCode.DownArrow) ? -1 : 0;

        Vector2 moveDir = new Vector2(moveX, moveY);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(moveY, moveX) * Mathf.Rad2Deg;
            float currentAngle = cachedTransform.eulerAngles.z;
            float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);
            float maxStep = turnSpeed * 360f * Time.deltaTime;

            float newAngle = Mathf.Abs(angleDiff) < maxStep
                ? targetAngle
                : currentAngle + Mathf.Sign(angleDiff) * maxStep;

            cachedTransform.rotation = Quaternion.Euler(0, 0, newAngle);
        }

        cachedTransform.position += (Vector3)(moveDir * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") ||
            collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.TankDestroyed(gameObject.tag);
            Destroy(gameObject);
        }
    }
}