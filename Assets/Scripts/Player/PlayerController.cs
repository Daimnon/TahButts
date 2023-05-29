using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _player;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb;
    public Rigidbody2D Rb => _rb;

    [SerializeField] private Vector2 _xBounds = Vector2.zero, _yBounds = new (-7.1f, 0.5f);
    public Vector2 XBounds { get => _xBounds; set => _xBounds = value; }

    [SerializeField] private Transform[] _items;
    [SerializeField] private float _gravityScale = 1.5f, _jumpForce = 300.0f, _speed = 10.0f, _maxWalkHeight = 0.5f;


    private GameObject _currentHeadphones, _currentShield;
    private Collider2D _currentHitCollider;
    private Vector2 _moveInput = Vector2.zero;
    private Vector3 _moveDirection = Vector3.zero;
    private int _comboHitCounter = 0, _maxHitCombo = 3;
    private float _comboTime = 1.0f, _comboTimer = 1.0f;
    private float _heightBeforeJumping;

    private bool _isFacingLeft = true, _isJumping = false;
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
            _animator.SetBool("IsJumping", true);
            // animation isJumping = isJumping
            Debug.Log("Player Jumped.");
        }
        else Debug.LogError("Jump action failed: Player is in the air.");
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_comboHitCounter != _maxHitCombo)
            {
                StartCoroutine(HandleHitCollider());
                _comboTimer = _comboTime;
                _comboHitCounter++;

                if (!(_animator.GetCurrentAnimatorStateInfo(0).IsName("Anim_Player_Punch") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f))
                {
                    _animator.SetBool("IsPunching", false);
                    _animator.SetBool("IsPunching", true);
                }
                _animator.SetBool("IsPunching", true);
            }

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

    public float horizontalBounds = 5.0f;
    public float verticalBounds = 3.0f;

    private void Start()
    {
        Initialize();
    }
    private void Update()
    {
        if (_player.Data.Health <= 0)
            Die();

        ClampPlayerToView();
        GetMoveDirection();
        ChainCombo();

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
    private void ClampPlayerToView()
    {
        _xBounds.x = CameraManager.Instance.XRange.x - 7.8f;
        _xBounds.y = CameraManager.Instance.XRange.y + 3.5f;

        if (transform.position.x < _xBounds.x)
            transform.position = new(_xBounds.x, transform.position.y, transform.position.z);

        if (transform.position.x > _xBounds.y)
            transform.position = new(_xBounds.y, transform.position.y, transform.position.z);

        if (transform.position.y < _yBounds.x)
            transform.position = new(transform.position.x, _yBounds.x, transform.position.z);

        if (transform.position.y > _yBounds.y)
            transform.position = new(transform.position.x, _yBounds.y, transform.position.z);
    }
    private void GetMoveDirection()
    {
        if (!_isJumping)
        {
            _moveDirection = new(_moveInput.x, _moveInput.y, 0.0f);
            _animator.SetFloat("Speed", Mathf.Abs(_moveInput.x != 0 ? _moveInput.x : _moveInput.y));
        }
        else
            _moveDirection = new(_moveInput.x, 0.0f, 0.0f);
    }
    private void FlipSprite()
    {
        Vector3 tempScale = transform.localScale;

        if (_moveInput.x < 0 && !_isFacingLeft || _moveInput.x > 0 && _isFacingLeft)
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
        _animator.SetBool("IsJumping", false);
    }
    private void Walk()
    {
        transform.position += _speed * Time.fixedDeltaTime * _moveDirection;
    }
    private void Jump()
    {
        _rb.AddForce(Vector2.up * _jumpForce);
    }
    private void ChainCombo()
    {
        if (_comboHitCounter > 0)
        {
            _comboTimer -= Time.deltaTime;

            if (_comboTimer <= 0)
            {
                _comboHitCounter = 0;
                _animator.SetBool("IsPunching", false);
            }
        }
    }
    public void TakeDamage(int damage)
    {
        UIManager.Instance.RemoveHeart();
        _player.Data.Health -= damage;
    }

    private IEnumerator HandleHitCollider()
    {
        _currentHitCollider = Instantiate(_player.Data.HitColliderGO, _player.Data.HitColliderTr).GetComponent<Collider2D>();
        EnemyDamager enemyDamager = _currentHitCollider.GetComponent<EnemyDamager>();
        enemyDamager.Damage = _player.Data.Power;
        yield return new WaitForSeconds(0.2f);

        Destroy(_currentHitCollider.gameObject);
    }

    private void Die()
    {
        _isAlive = false;

        if (_player.Data.Lives <= 0)
            Respawn();
    }
    private void Respawn()
    {
        transform.position = SpawnManager.Instance.PlayerSpawn.position;

        _isAlive = true;
    }
}
