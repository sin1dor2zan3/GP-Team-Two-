using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float minMoveSpeed = 1.5f;
    public float maxMoveSpeed = 3.0f;
    public float turnSpeed = 0.5f;

    public float minDecisionTime = 1.5f;
    public float maxDecisionTime = 3.5f;

    public float minShootCooldown = 0.8f;
    public float maxShootCooldown = 1.5f;
    public float reactionDelay = 0.5f;

    public float visionConeAngle = 60f;
    public float visionDistance = 15f;
    public LayerMask shootBlockMask;

    public GameObject bulletPrefab;
    public Transform barrel;
    public Transform targetPlayer;

    private float moveSpeed;
    private float shootCooldown;
    private float decisionTimer;
    private float nextDecisionTime;
    private float lastShotTime;
    private float reactionTimer;

    private Vector2 moveDir;
    private Transform cachedTransform;

    private int playerLayer;
    private int wallLayer;

    void Awake()
    {
        cachedTransform = transform;
        playerLayer = LayerMask.NameToLayer("Player");
        wallLayer = LayerMask.NameToLayer("Wall");
    }

    void Start()
    {
        RandomizeStats();
        ScheduleNextDecision();
        lastShotTime = -shootCooldown;
        PickRandomDirection();
    }

    void Update()
    {
        Movement();
        HandleDecisionTimer();
        TryShoot();
    }

    void RandomizeStats()
    {
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        shootCooldown = Random.Range(minShootCooldown, maxShootCooldown);
    }

    void Movement()
    {
        if (moveDir.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
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

    void HandleDecisionTimer()
    {
        decisionTimer += Time.deltaTime;

        if (decisionTimer >= nextDecisionTime)
        {
            MakeDecision();
            ScheduleNextDecision();
        }
    }

    void ScheduleNextDecision()
    {
        decisionTimer = 0f;
        nextDecisionTime = Random.Range(minDecisionTime, maxDecisionTime);
    }

    void MakeDecision()
    {
        if (targetPlayer == null || Random.value >= 0.75f)
        {
            PickRandomDirection();
            return;
        }

        moveDir = (targetPlayer.position - cachedTransform.position).normalized;
    }

    void PickRandomDirection()
    {
        switch (Random.Range(0, 4))
        {
            case 0: moveDir = Vector2.right; break;
            case 1: moveDir = Vector2.left; break;
            case 2: moveDir = Vector2.up; break;
            case 3: moveDir = Vector2.down; break;
        }
    }

    void TryShoot()
    {
        if (targetPlayer == null || barrel == null)
            return;

        // Cooldown check
        if (Time.time - lastShotTime < shootCooldown)
            return;

        Vector2 toPlayer = targetPlayer.position - cachedTransform.position;
        float sqrDistance = toPlayer.sqrMagnitude;

        // Distance check (squared for performance)
        if (sqrDistance > visionDistance * visionDistance)
        {
            reactionTimer = 0f;
            return;
        }

        // Angle check
        float angle = Vector2.Angle(cachedTransform.right, toPlayer);
        if (angle > visionConeAngle * 0.5f)
        {
            reactionTimer = 0f;
            return;
        }

        // Line-of-sight check
        RaycastHit2D hit = Physics2D.Raycast(
            barrel.position,
            toPlayer.normalized,
            visionDistance,
            shootBlockMask
        );

        if (hit.collider == null || hit.collider.transform != targetPlayer)
        {
            reactionTimer = 0f;
            return;
        }

        // Reaction delay
        reactionTimer += Time.deltaTime;

        if (reactionTimer >= reactionDelay)
        {
            Instantiate(bulletPrefab, barrel.position, barrel.rotation);
            lastShotTime = Time.time;
            reactionTimer = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layer = collision.gameObject.layer;

        if (layer == wallLayer)
        {
            HandleDecisionTimer();
        }

        if (layer == playerLayer)
        {
            GameManager.Instance.TankDestroyed(gameObject.tag);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}