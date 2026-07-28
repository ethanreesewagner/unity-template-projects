using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyLogic : MonoBehaviour, IDamageable
{
    [Header("Chase settings")]
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float maxHealth = 50f;

    private Rigidbody2D _rb;
    private Transform _player;
    private IDamageable _playerDamageable;
    private SpriteRenderer _spriteRenderer;
    private float _nextAttackTime;
    private float _currentHealth;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;
        _currentHealth = maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _player = FindPlayerTransform();

        if (_player != null)
        {
            _playerDamageable = _player.GetComponent<IDamageable>();
            if (_playerDamageable == null)
            {
                _playerDamageable = _player.GetComponentInChildren<IDamageable>();
            }

            var playerMovement = _player.GetComponent<TopDownMovement>();
            if (playerMovement != null && chaseSpeed <= 0f)
            {
                chaseSpeed = Mathf.Max(0.5f, playerMovement.speed * 0.7f);
            }
        }
    }

    private Transform FindPlayerTransform()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            return playerObject.transform;
        }

        var playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            return playerHealth.transform;
        }

        var playerMovement = FindObjectOfType<TopDownMovement>();
        if (playerMovement != null)
        {
            return playerMovement.transform;
        }

        return null;

        return null;
    }

    private void Update()
    {
        if (_player == null || _currentHealth <= 0f)
        {
            _rb.velocity = Vector2.zero;
            return;
        }

        Vector2 delta = _player.position - transform.position;
        float distance = delta.magnitude;
        Vector2 movement = Vector2.zero;

        float stopDistance = attackRange + 0.1f;
        if (distance > stopDistance)
        {
            movement = delta.normalized * chaseSpeed;
        }

        _rb.velocity = movement;
        UpdateFacing(delta.x);

        if (distance <= attackRange && Time.time >= _nextAttackTime)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        if (_playerDamageable != null)
        {
            _playerDamageable.DealDamage(attackDamage);
        }

        _nextAttackTime = Time.time + attackCooldown;
    }

    public void DealDamage(float amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void HealHealth(float amount)
    {
        _currentHealth = Mathf.Min(maxHealth, _currentHealth + amount);
    }

    private void UpdateFacing(float deltaX)
    {
        if (_spriteRenderer == null)
        {
            return;
        }

        if (deltaX > 0f)
        {
            _spriteRenderer.flipX = false;
        }
        else if (deltaX < 0f)
        {
            _spriteRenderer.flipX = true;
        }
    }

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => maxHealth;
}
