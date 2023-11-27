using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyWeapon : MonoBehaviour
    {
        [SerializeField] private float _maxAttackDistance;
        [SerializeField] private int _damage;
        [SerializeField] private float _attackCooldown;
        [SerializeField] private Animator _animator;

        private Player.Player _player;
        private float _nextAttackTime;

        private void Start()
        {
            _player = FindAnyObjectByType<Player.Player>();
        }

        public bool Attack()
        {
            if(Time.time < _nextAttackTime) return false;

            _nextAttackTime = Time.time + _attackCooldown;
            
            if(_player == null || !_player.IsAlive) return false;

            var distance = Vector2.Distance(transform.position, _player.transform.position);
            if(distance > _maxAttackDistance) return false;
            
            _animator.ResetTrigger("Attack");
            _animator.SetTrigger("Attack");

            return true;
        }

        public void OnAnimationAttackTriggered()
        {
            var distance = Vector2.Distance(transform.position, _player.transform.position);
            if(distance > _maxAttackDistance) return;
            _player.ApplyDamage(_damage);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _maxAttackDistance);
        }
    }
}