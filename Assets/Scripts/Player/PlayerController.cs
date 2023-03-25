using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _player;
    [SerializeField] private Rigidbody2D _rb;
    public Rigidbody2D Rb => _rb;

    [SerializeField] private Transform[] _items;
    [SerializeField] private float _gravityScale = 1.5f, _jumpForce = 300.0f, _speed = 10.0f;

    private GameObject _currentHeadphones, _currentShield;
    private Vector2 _moveInput = Vector2.zero;
    private Vector3 _moveDirection = Vector3.zero;
    private float _heightBeforeJumping;
    private bool _isFacingLeft = false, _isJumping = false;
    public bool IsFacingLeft => _isFacingLeft;

    private bool _isUsingHeadphones = false, _isUsingShield = false;
    private bool _isAlive = true, _isStunned = false;
    public bool IsAlive => _isAlive;
    public bool IsStunned { get => _isStunned; set => _isStunned = value; }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        // animation speed = Mathf.Abs(_input.x != 0 ? _input.x : _inpt.y)
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && !_isJumping)
        {
            _heightBeforeJumping = transform.position.y;
            _isJumping = true;
            _rb.gravityScale = _gravityScale;
            _rb.WakeUp();
            Jump();
            // animation isJumping = isJumping
            Debug.Log("Player Jumped.");
        }
        else Debug.LogError("Jump action failed: Player is in the air.");
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Player Attacked.");
        }
        else Debug.LogError("Attack action failed: No logic.");
    }
    public void OnInteractOne(InputAction.CallbackContext context)
    {
        if (context.started && !_isUsingHeadphones)
        {
            _currentHeadphones = Instantiate(_player.Data.HeadPhonesPrefab, _items[0]);
            _isUsingHeadphones = true;
            // other logic
            Debug.Log("Player Used Headphones.");
        }
        else if (context.started && _isUsingHeadphones)
        {
            Destroy(_currentHeadphones);
            _isUsingHeadphones = false;
            // other logic
            Debug.Log("Player Unused Headphones.");
        }
        else Debug.LogError("Jump action failed: Player don't have headphones.");
    }
    public void OnInteractTwo(InputAction.CallbackContext context)
    {
        if (context.started && !_isUsingShield)
        {
            _currentShield = Instantiate(_player.Data.ShieldPrefab, _items[1]);
            _isUsingShield = true;
            // other logic
            Debug.Log("Player Used Shield.");
        }
        else if (context.started && _isUsingShield)
        {
            Destroy(_currentShield);
            _isUsingShield = false;
            // other logic
            Debug.Log("Player Unused Shield.");
        }
        else Debug.LogError("Jump action failed: Player don't have shield.");
    }

    private void Start()
    {
        Initialize();
    }
    private void Update()
    {
        if (_player.Data.Health <= 0)
            Die();

        GetMoveDirection();

        if (transform.position.y < _heightBeforeJumping)
            Land();
    }
    private void FixedUpdate()
    {
        FlipSprite();
        Walk();
    }

    private void Initialize()
    {
        _rb.Sleep();
        _heightBeforeJumping = transform.position.y;
        UIManager.Instance.InitializePlayerHealth(_player);
    }
    private void GetMoveDirection()
    {
        _moveDirection = new(_moveInput.x, _moveInput.y, 0.0f);
    }
    private void FlipSprite()
    {
        Vector3 tempScale = transform.localScale;
        
        if (_moveInput.x < 0 && _isFacingLeft || _moveInput.x > 0 && !_isFacingLeft)
        {
            _isFacingLeft = !_isFacingLeft;
            tempScale.x *= -1;

            transform.localScale = tempScale;
        }
    }
    private void Land()
    {
        _isJumping = false;
        _rb.gravityScale = 0.0f;
        _rb.Sleep();
        _heightBeforeJumping = transform.position.y;
        // animation isJumping = _isJumping
    }
    private void Walk()
    {
        transform.position += _speed * Time.fixedDeltaTime * _moveDirection;
    }
    private void Jump()
    {
        _rb.AddForce(Vector2.up * _jumpForce);
    }
    public void TakeDamage(int damage)
    {
        _player.Data.Health -= damage;
        UIManager.Instance.RemoveHeart();
    }

   
    private void Die()
    {
        _isAlive = false;

        if (_player.Data.Lives >= 0)
            Respawn();
    }
    private void Respawn()
    {
        transform.position = GameManager.Instance.Spawn.position;

        _isAlive = true;
    }
}
