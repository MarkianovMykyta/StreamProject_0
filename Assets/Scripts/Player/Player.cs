using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Player : MonoBehaviour, IDestructable
    {
        public event Action<IDestructable> OnHealthChanged;
        
        
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerInput _playerInput;
        [Space] 
        [SerializeField] private float _speed;
        [Space] 
        [SerializeField] private Weapon _weapon;
        [Space] 
        [SerializeField] private int _maxHealth;

        public bool IsAlive => Health > 0;
        public int Health { get; private set; }
        public int MaxHealth => _maxHealth;
    
        private void Awake()
        {
            Health = _maxHealth;
            OnHealthChanged?.Invoke(this);
        }

        public void ApplyDamage(int damage)
        {
            if(!IsAlive) return;
        
            Health -= damage;
            
            if (Health <= 0)
            {
                Health = 0;
                Die();
            }
            else
            {
                _animator.ResetTrigger("Damage");
                _animator.SetTrigger("Damage");
                _weapon.ResetCooldownTime();
            }
            
            OnHealthChanged?.Invoke(this);
        }
    
        private bool _isLookingLeft;

        private Vector2 _moveInput;

        public void OnAnimationAttackTriggered()
        {
            _weapon.OnAnimationAttack();
        }
    
        private void Update()
        {
            ReadInput();

            if (_moveInput.x < 0)
            {
                _isLookingLeft = true;
            }
            else if(_moveInput.x > 0)
            {
                _isLookingLeft = false;
            }

            if (_moveInput.magnitude == 0)
            {
                _weapon.Attack();
            }

            _sr.flipX = _isLookingLeft;
        
            _animator.SetBool("Move", _moveInput.magnitude > 0.1f);
        }

        private void ReadInput()
        {
            _moveInput = _playerInput.actions["Move"].ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            _rb.velocity = _moveInput * _speed * Time.fixedDeltaTime;
        }

        private void Die()
        {
            gameObject.SetActive(false);
        }
    }
}