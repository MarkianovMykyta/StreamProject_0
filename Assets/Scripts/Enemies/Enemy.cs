using System;
using System.Threading.Tasks;
using DefaultNamespace;
using Enemies;
using UnityEngine;

public class Enemy : MonoBehaviour, IDestructable
{
    public event Action<Enemy> Died; 
    public event Action<IDestructable> OnHealthChanged;
    
    [SerializeField] private GameObject _tomb;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private EnemyWeapon _weapon;
    [SerializeField] private float _attackIdleTime;
    
    private Player.Player _player;
    
    public int Health { get; private set; }
    public int MaxHealth => _maxHealth;
    public bool IsAlive => Health > 0;
    private bool _isLookingLeft;

    private float _lastAttackTime;
    

    public void ApplyDamage(int damage)
    {
        if(!IsAlive) return;
        
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
        
        OnHealthChanged?.Invoke(this);
    }

    public void OnAnimationAttack()
    {
        _weapon.OnAnimationAttackTriggered();
    }
    
    private void Awake()
    {
        _player = FindAnyObjectByType<Player.Player>();

        Health = _maxHealth;
        
        _animator.SetBool("Move", true);
    }

    private void Update()
    {
        if(!IsAlive) return;
        
        if (_weapon.Attack())
        {
            _lastAttackTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if(!IsAlive) return;
        
        if (_player == null || !_player.IsAlive)
        {
            Stop();
            return;
        }
        
        if (Time.time - _lastAttackTime < _attackIdleTime)
        {
            Stop();
            return;
        }
       
        _animator.SetBool("Move", true);
        
        var moveDirection = _player.transform.position - transform.position;

        if (moveDirection.x < 0)
        {
            _isLookingLeft = true;
        }
        else if (moveDirection.x > 0)
        {
            _isLookingLeft = false;
        }

        _sr.flipX = _isLookingLeft;
        
        _rb.velocity = moveDirection.normalized * _moveSpeed * Time.fixedDeltaTime;
    }

    private void Stop()
    {
        _animator.SetBool("Move", false);
        _rb.velocity = Vector2.zero;
    }

    private async void Die()
    {
        Died?.Invoke(this);
        
        _rb.velocity = Vector3.zero;
        _animator.SetTrigger("Die");
        Instantiate(_tomb, transform.position, Quaternion.identity);
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(1), destroyCancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
        Destroy(gameObject);
    }
}