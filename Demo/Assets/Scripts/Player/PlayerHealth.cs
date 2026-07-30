using UnityEngine;

// INFORMATION ==================================================
// This script handles logic for players to allow taking damage
// and healing.
// ==============================================================
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth = 30f;
    [SerializeField] private float _regenAmount = 4f;
    [SerializeField] private float _regenDelay = 1.5f;
    [SerializeField] private float _regenInterval = 1f;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Collider2D collider;

    private float _nextRegenTime;

    private void Awake()
    {spriteRenderer = GetComponent<SpriteRenderer>();
    rb = GetComponent<Rigidbody2D>();
    collider = GetComponent<Collider2D>();
        _health = _maxHealth;
        _nextRegenTime = Time.time + _regenDelay;
    }

    private void Update()
    {
        if (_health < _maxHealth && Time.time >= _nextRegenTime)
        {
            HealHealth(_regenAmount);
            _nextRegenTime = Time.time + _regenInterval;
        }
    }

    //Subtract amount from health, checks for health below zero
    public void DealDamage(float amount)
    {
        _health -= amount;
        _nextRegenTime = Time.time + _regenDelay;

        if (_health <= 0)
        {
            PlayerLose();
        }
    }

    //Heals amount of health, cannot overheal
    public void HealHealth(float amount)
    {
        _health += amount;
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }

    public float CurrentHealth => _health;
    public float MaxHealth => _maxHealth;

    //Implement logic for when the player runs out of health here
    private void PlayerLose()
    {
        Debug.Log("Player has lost the game");
        rb.velocity = Vector2.zero;
                spriteRenderer.color = new Color(0.6f, 0.6f, 0.6f, 0.8f);
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }

