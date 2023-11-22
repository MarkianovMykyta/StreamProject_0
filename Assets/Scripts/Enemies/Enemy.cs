using System;
using System.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

public class Enemy : MonoBehaviour, IDestructable
{
    public event Action<Enemy> Died; 
    
    [SerializeField] private GameObject _tomb;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _sr;
    
    private Player _player;
    
    public int Health { get; private set; }
    public bool IsAlive => Health > 0;
    private bool _isLookingLeft;
    

    public void Attack(int damage)
    {
        if(!IsAlive) return;
        
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
    }
    
    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();

        Health = _maxHealth;
        
        _animator.SetBool("Move", true);
    }

    private void FixedUpdate()
    {
        if(!IsAlive) return;
        
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

    private async void Die()
    {
        Died?.Invoke(this);
        
        _rb.velocity = Vector3.zero;
        _animator.SetTrigger("Die");
        Instantiate(_tomb, transform.position, Quaternion.identity);
        await Task.Delay(TimeSpan.FromSeconds(1));
        Destroy(gameObject);
    }
}