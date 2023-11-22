using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private CircleCollider2D _damageZone;
    [SerializeField] private int _damage;

    private float _nextAttackTime;

    [SerializeField] private Enemy _nearestEnemy;

    private Quaternion _targetRotation;

    private Enemy[] _enemies;

    private void Awake()
    {
        _enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
    }

    public void Attack()
    {
        if (Time.time < _nextAttackTime) return;


        _nextAttackTime = Time.time + _attackCooldown;

        _animator.ResetTrigger("Attack");
        _animator.SetTrigger("Attack");
    }

    public void OnAnimationAttack()
    {
        var colliders = Physics2D.OverlapCircleAll(_damageZone.transform.position, _damageZone.radius);

        foreach (var collider in colliders)
        {
            var destructable = collider.GetComponent<IDestructable>();
            destructable?.Attack(_damage);
        }
    }

    private void Update()
    {
        var minDistance = float.MaxValue;
        for (var i = 0; i < _enemies.Length; i++)
        {
            var enemy = _enemies[i];
            if (!enemy.IsAlive) continue;

            var distance = Vector2.Distance(enemy.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                _nearestEnemy = enemy;
            }
        }

        var attackDirection = Vector2.up;

        if (_nearestEnemy != null && _nearestEnemy.IsAlive)
        {
            attackDirection = _nearestEnemy.transform.position - transform.position;
            attackDirection.Normalize();
        }

        _targetRotation = Quaternion.LookRotation(Vector3.forward, attackDirection);

        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, .3f);
    }
}