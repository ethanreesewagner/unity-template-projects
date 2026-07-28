using UnityEngine;

// INFORMATION ==================================================
// This script handles logic for players to allow attacking
// any entity with a health script.
// ==============================================================
public class Attacking : MonoBehaviour
{
    [SerializeField] private float _attackDamage = 12f;
    [SerializeField] private float _attackRadius = 1.4f;
    [SerializeField] private float _attackCooldown = 0.35f;

    private float _nextAttackTime;

    //Each frame, if the key is pressed, start an attack
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= _nextAttackTime)
        {
            HandleAttack();
            _nextAttackTime = Time.time + _attackCooldown;
        }
    }

    //Performs calculations to deal damage.
    private void HandleAttack()
    {
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
}