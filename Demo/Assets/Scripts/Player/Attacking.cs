using UnityEngine;

// INFORMATION ==================================================
// This script handles logic for players to allow attacking
// any entity with a health script.
// ==============================================================
public class Attacking : MonoBehaviour
{
    [SerializeField] private float _attackDamage = 12f;
    [SerializeField] private float _attackRadius = 1.8f;
    [SerializeField] private float _attackCooldown = 0.35f;

    private float _nextAttackTime;
    private EnemyLogic _cachedEnemy;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= _nextAttackTime)
        {
            HandleAttack();
            _nextAttackTime = Time.time + _attackCooldown;
        }
    }

    private void HandleAttack()
    {
        var enemy = FindTargetEnemy();
        if (enemy != null)
        {
            enemy.DealDamage(_attackDamage);
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _attackRadius);
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject)
            {
                continue;
            }

            var damageable = hit.GetComponent<IDamageable>();
            if (damageable == null)
            {
                damageable = hit.GetComponentInChildren<IDamageable>();
            }

            if (damageable != null)
            {
                damageable.DealDamage(_attackDamage);
                return;
            }
        }
    }

    private EnemyLogic FindTargetEnemy()
    {
        if (_cachedEnemy != null && _cachedEnemy.gameObject != null)
        {
            return _cachedEnemy;
        }

        var enemy = FindObjectOfType<EnemyLogic>();
        if (enemy != null)
        {
            _cachedEnemy = enemy;
        }

        return enemy;
    }
}