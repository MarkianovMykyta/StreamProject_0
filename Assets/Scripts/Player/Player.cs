using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerInput _playerInput;
    [Space] 
    [SerializeField] private float _speed;
    [Space] 
    [SerializeField] private Weapon _weapon;
    
    
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
}