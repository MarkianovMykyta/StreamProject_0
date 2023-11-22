using System;
using DefaultNamespace;
using UnityEngine;

public class Enemy : MonoBehaviour, IDestructable
{
    [SerializeField] private GameObject _tomb;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody2D _rb;
    
    private Player _player;
    
    public int Health { get; private set; }
    public bool IsAlive => Health > 0;
    

    public void Attack(int damage)
    {
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
    }

    private void FixedUpdate()
    {
        var moveDirection = _player.transform.position - transform.position;

        _rb.velocity = moveDirection.normalized * _moveSpeed * Time.fixedDeltaTime;
    }

    private void Die()
    {
        gameObject.SetActive(false);
        Instantiate(_tomb, transform.position, Quaternion.identity);
    }
}