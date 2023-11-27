using System;
using System.Collections.Generic;
using DefaultNamespace;
using Enemies;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private CircleCollider2D _damageZone;
    [SerializeField] private int _damage;
    [SerializeField] private Player.Player _player;

    private float _nextAttackTime;

    [SerializeField] private Enemy _nearestEnemy;

    private Quaternion _targetRotation;

    private EnemiesSpawner _enemiesSpawner;

    private void Awake()
    {
        _enemiesSpawner = FindAnyObjectByType<EnemiesSpawner>();
    }

    public void Attack()
    {
        if(!gameObject.activeSelf) return;
        if (Time.time < _nextAttackTime) return;
        
        _nextAttackTime = Time.time + _attackCooldown;

        _animator.ResetTrigger("Attack");
        _animator.SetTrigger("Attack");
    }

    public void ResetCooldownTime()
    {
        _nextAttackTime = Time.time + _attackCooldown;
    }

    public void OnAnimationAttack()
    {
        var colliders = Physics2D.OverlapCircleAll(_damageZone.transform.position, _damageZone.radius);

        foreach (var collider in colliders)
        {
            var destructable = collider.GetComponent<IDestructable>();
            if (destructable != _player)
            {
                destructable?.ApplyDamage(_damage);
            }
        }
    }

    private void Update()
    {
        var minDistance = float.MaxValue;
        var enemies = _enemiesSpawner.AliveEnemies;
        for (var i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
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