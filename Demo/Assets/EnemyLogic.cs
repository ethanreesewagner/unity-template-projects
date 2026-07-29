using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyLogic : MonoBehaviour, IDamageable
{
    [Header("Chase settings")]
    [SerializeField] private float chaseSpeed = 2.2f;
    [SerializeField] private float stopDistance = 1.4f;
    [SerializeField] private float attackRange = 1.4f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float maxHealth = 80f;

    private Rigidbody2D _rb;
    private Transform _player;
    private IDamageable _playerDamageable;
    private PlayerHealth _playerHealth;
    private SpriteRenderer _spriteRenderer;
    private float _nextAttackTime;
    private float _currentHealth;
    private bool _isDead;

    private void Awake()
    {
        EnsureEnemyComponents();
    }

    private void Start()
    {
        EnsureEnemyComponents();

        maxHealth = Mathf.Max(maxHealth, 120f);
        _currentHealth = maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        FindPlayerReference();

        if (_player != null)
        {
            var playerMovement = _player.GetComponent<TopDownMovement>();
            if (playerMovement != null && chaseSpeed <= 0f)
            {
                chaseSpeed = Mathf.Max(0.5f, playerMovement.speed * 0.7f);
            }
        }
    }

    private void EnsureEnemyComponents()
    {
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        if (_rb == null)
        {
            _rb = gameObject.AddComponent<Rigidbody2D>();
        }

        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;

        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        if (GetComponent<SpriteRenderer>() == null)
        {
            gameObject.AddComponent<SpriteRenderer>();
        }

        if (!gameObject.CompareTag("Enemy"))
        {
            TrySetTag("Enemy");
        }
    }

    private void TrySetTag(string tagName)
    {
        if (string.IsNullOrEmpty(tagName))
        {
            return;
        }

        try
        {
            gameObject.tag = tagName;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"Could not assign tag '{tagName}' to '{gameObject.name}': {ex.Message}");
        }
    }

    private void FindPlayerReference()
    {
        _player = null;
        _playerDamageable = null;
        _playerHealth = null;

        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            playerObject = FindObjectOfType<PlayerHealth>()?.gameObject;
        }

        if (playerObject == null)
        {
            playerObject = FindObjectOfType<TopDownMovement>()?.gameObject;
        }

        if (playerObject != null)
        {
            _player = playerObject.transform;
            _playerHealth = playerObject.GetComponent<PlayerHealth>();
            _playerDamageable = _playerHealth as IDamageable;

            if (_playerDamageable == null)
            {
                _playerDamageable = playerObject.GetComponent<IDamageable>();
            }

            if (_playerDamageable == null)
            {
                _playerDamageable = playerObject.GetComponentInChildren<IDamageable>();
            }
        }
    }

    private void Update()
    {
        if (_player == null || _currentHealth <= 0f || _isDead)
        {
            if (_rb != null)
            {
                _rb.velocity = Vector2.zero;
            }

            return;
        }

        Vector2 delta = _player.position - transform.position;
        float distance = delta.magnitude;
        Vector2 movement = Vector2.zero;

        if (distance > stopDistance)
        {
            movement = delta.normalized * chaseSpeed;
        }

        if (_rb != null)
        {
            _rb.velocity = movement;
        }

        UpdateFacing(delta.x);

        if (distance <= attackRange && Time.time >= _nextAttackTime)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        if (_player == null)
        {
            FindPlayerReference();
        }

        if (_playerDamageable != null)
        {
            _playerDamageable.DealDamage(attackDamage);
        }
        else if (_playerHealth != null)
        {
            _playerHealth.DealDamage(attackDamage);
        }
        else
        {
            var player = FindObjectOfType<PlayerHealth>();
            if (player != null)
            {
                player.DealDamage(attackDamage);
            }
        }

        _nextAttackTime = Time.time + attackCooldown;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_isDead || _player == null)
        {
            return;
        }

        var playerObject = collision.collider.gameObject;
        if (playerObject == null)
        {
            return;
        }

        if (playerObject.CompareTag("Player") || playerObject.GetComponent<PlayerHealth>() != null || playerObject.GetComponent<TopDownMovement>() != null)
        {
            if (Time.time >= _nextAttackTime)
            {
                AttackPlayer();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_isDead || _player == null)
        {
            return;
        }

        var playerObject = other.gameObject;
        if (playerObject.CompareTag("Player") || playerObject.GetComponent<PlayerHealth>() != null || playerObject.GetComponent<TopDownMovement>() != null)
        {
            if (Time.time >= _nextAttackTime)
            {
                AttackPlayer();
            }
        }
    }

    public void DealDamage(float amount)
    {
        if (_isDead)
        {
            return;
        }

        _currentHealth -= amount;
        if (_currentHealth <= 0f)
        {
            _currentHealth = 0f;
            _isDead = true;

            if (_rb != null)
            {
                _rb.velocity = Vector2.zero;
            }

            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = new Color(0.6f, 0.6f, 0.6f, 0.8f);
            }

            var collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }

    public void HealHealth(float amount)
    {
        if (_isDead)
        {
            return;
        }

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
