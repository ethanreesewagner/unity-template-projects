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
    private float _nextAttackTime;
    private float _currentHealth;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;
        _currentHealth = maxHealth;

        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (_player == null)
        {
            _player = FindObjectOfType<TopDownMovement>()?.transform;
        }

        if (_player != null)
        {
            _playerDamageable = _player.GetComponent<IDamageable>();

            var playerMovement = _player.GetComponent<TopDownMovement>();
            if (playerMovement != null && chaseSpeed <= 0f)
            {
                chaseSpeed = Mathf.Max(0.5f, playerMovement.speed * 0.7f);
            }
        }
    }

    private void Update()
    {
        if (_player == null || _currentHealth <= 0f)
        {
            _rb.velocity = Vector2.zero;
            return;
        }

        Vector2 delta = _player.position - transform.position;
        Vector2 movement = Vector2.zero;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            movement = new Vector2(Mathf.Sign(delta.x) * chaseSpeed, 0f);
        }
        else
        {
            movement = new Vector2(0f, Mathf.Sign(delta.y) * chaseSpeed);
        }

        _rb.velocity = movement;

        if (Vector2.Distance(transform.position, _player.position) <= attackRange && Time.time >= _nextAttackTime)
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

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => maxHealth;
}
