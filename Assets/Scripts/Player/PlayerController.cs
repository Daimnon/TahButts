using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _player;
    [SerializeField] private Rigidbody2D _rb;
    public Rigidbody2D Rb => _rb;

    [SerializeField] private Animator _animator;
    public Animator Animator => _animator;

    [SerializeField] private Vector2 _xBounds = new(129.95f, 166.0f), _yBounds = new(-6.45f, -1.5f);
    public Vector2 XBounds { get => _xBounds; set => _xBounds = value; }

    [SerializeField] private Transform[] _items;
    [SerializeField] private float _gravityScale = 1.5f, _jumpForce = 300.0f, _speed = 10.0f;
    [SerializeField] private float _xMoveOffset = -7.8f, _yMoveOffset = 3.5f;
    [Range(0.2f, 2.0f)] [SerializeField] private float _comboTime = 1.0f;
    [SerializeField] private bool _isTesting;

    private GameObject _currentHeadphones, _currentShield;
    private Collider2D _currentHitCollider;
    private Vector2 _moveInput = Vector2.zero;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _lastPositionBeforeDeath = Vector3.zero;
    private int _comboHitCounter = 0, _maxHitCombo = 3;
    private float _comboTimer = 1.0f;
    private float _heightBeforeJumping;

    private bool _isFacingLeft = true, _isJumping = false;
    public bool IsFacingLeft => _isFacingLeft;

    private bool _isUsingHeadphones = false, _isUsingMask = false;
    public bool IsUsingHeadphones => _isUsingHeadphones;
    public bool IsUsingMask => _isUsingMask;

    private bool _isAlive = true, _isStunned = false, _isHurt;
    public bool IsAlive => _isAlive;
    public bool IsStunned { get => _isStunned; set => _isStunned = value; }
    public bool IsHurt { get => _isHurt; set => _isHurt = value; }

    public AudioSource _audioSource;
    public AudioClip[] _audioClip;       //שורות של עומר 
    private int currentIndex = 0; // האינדקס של הקליפ הנוכחי

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.IsLevelPlaying && !_isTesting)
            return;

        _moveInput = context.ReadValue<Vector2>();
        // animation speed = Mathf.Abs(_input.x != 0 ? _input.x : _inpt.y)

        _audioSource.PlayOneShot(_audioClip[4]); //תוספת של עומר - יכול להעביר את זה לקלאס Move


    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.IsLevelPlaying && !_isTesting)
            return;

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



            _audioSource.PlayOneShot(_audioClip[3]); //תוספת של עומר, סאונד קפיצה, אפשר להעביר אותו לקלאס Jump


        }
        else Debug.LogError("Jump action failed: Player is in the air.");
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.IsLevelPlaying && !_isTesting)
            return;

        if (context.started)
        {
            if (_comboHitCounter != _maxHitCombo)
            {
                StartCoroutine(HandleHitCollider());
                _comboTimer = _comboTime;
                _comboHitCounter++;

                /*if (!(_animator.GetCurrentAnimatorStateInfo(0).IsName("Anim_Player_Punch") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f))
                {
                    _animator.SetBool("IsPunching", false);
                    _animator.SetBool("IsPunching", true);
                }*/
                _animator.SetBool("IsPunching", true);

                _audioSource.PlayOneShot(_audioClip[4]); //תוספת של עומר, סאונד התקפה
                Debug.Log(_audioClip + "is playing right now !"); //וידוא סאונד מתקפה מופעל - עומר

            }

            Debug.Log("Player Attacked.");
        }
        else Debug.LogError("Attack action failed: No logic.");
    }
    public void OnInteractOne(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.IsLevelPlaying && !_isTesting)
            return;

        bool isPressed = context.ReadValue<float>() == 0 ? false : true;

        if (context.started && isPressed && !_isUsingHeadphones)
        {
            _currentHeadphones = Instantiate(_player.Data.HeadPhonesPrefab, _items[0]);
            _isUsingHeadphones = true;


           // _audioSource.PlayOneShot(_audioClip[6]); - טסט לסאונד כשהדמות מרכיבה אוזניות


            // other logic
            Debug.Log("Player Used Headphones.");
        }
        else if (context.started && isPressed && _isUsingHeadphones)
        {
            Destroy(_currentHeadphones);
            _isUsingHeadphones = false;
            // other logic

            // _audioSource.PlayOneShot(_audioClip[6]); - טסט לסאונד כשהדמות מורידה אוזניות


            Debug.Log("Player Unused Headphones.");
        }
        else Debug.LogError("InteractOne action failed: Player don't have headphones.");
    }
    public void OnInteractTwo(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.IsLevelPlaying && !_isTesting)
            return;

        if (context.started && !_isUsingMask)
        {
            _currentShield = Instantiate(_player.Data.ShieldPrefab, _items[1]);
            _isUsingMask = true;



            //it's Omer - here i think we can add an if statement for cases when the Player isHurt with a mask on - use a low pass filter or smth
            // _audioSource.PlayOneShot(_audioClip[6]); - טסט לסאונד בלי מסיכה ונפגעת ומשמיעה אנחת כאב


            // other logic
            Debug.Log("Player Used Shield.");
        }
        else if (context.started && _isUsingMask)
        {
            Destroy(_currentShield);
            _isUsingMask = false;


            //it's Omer - here i think we can add an if statement for cases when the Player isHurt with a mask on - use a low pass filter or smth
            // _audioSource.PlayOneShot(_audioClip[6]); - טסט לסאונד כשהדמות בלי מסיכה ונפגעת ומשמיעה אנחת כאב





            // other logic
            Debug.Log("Player Unused Shield.");
        }
        else Debug.LogError("InteractTwo action failed: Player don't have shield.");
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started && !UIManager.Instance.PauseMenu.activeInHierarchy)
        {
            Time.timeScale = 0;
            UIManager.Instance.PauseMenu.SetActive(true);
            Debug.Log("Game Paused.");



            //_audioSource.PlayOneShot(_audioClip[6]); - טסט לסאונד כשהמשתמש רוצה לעצור את המשחק



        }
        else if (context.started && UIManager.Instance.PauseMenu.activeInHierarchy)
        {
            Time.timeScale = 1;
            UIManager.Instance.PauseMenu.SetActive(false);
            Debug.Log("Game Unpaused.");



            //_audioSource.PlayOneShot(_audioClip[6]); - טסט לסאונד כשהמשתמש מחליט לחזור לשחק


        }
        else Debug.LogError("Pause action failed.");
    }

    //public float horizontalBounds = 5.0f;
    //public float verticalBounds = 3.0f;

    private void Start()
    {
        Initialize();
    }
    private void Update()
    {
        if (!GameManager.Instance.IsLevelPlaying && !_isTesting)
            return;

        if (!_isAlive)
        {
            transform.position = _lastPositionBeforeDeath;
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsTag("Death") && stateInfo.normalizedTime > 0.9)
            {
                UIManager.Instance.EndPopUp.SetActive(true);
                UIManager.Instance.EndLose.SetActive(true);


                //_audioSource.PlayOneShot(_audioClip[6]); טסט להשמעת טראק של הפסד במשחק









            }
            return;
        }

        _lastPositionBeforeDeath = transform.position;

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
        if (transform.position.x < _xBounds.x)
            transform.position = new Vector3(_xBounds.x, transform.position.y, transform.position.z);

        if (transform.position.x > _xBounds.y)
            transform.position = new Vector3(_xBounds.y, transform.position.y, transform.position.z);

        if (transform.position.y < _yBounds.x)
            transform.position = new Vector3(transform.position.x, _yBounds.x, transform.position.z);

        if (transform.position.y > _yBounds.y)
            transform.position = new Vector3(transform.position.x, _yBounds.y, transform.position.z);
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

        // _audioSource.PlayOneShot(_audioClip[6]); - טסט לסאונד כשהדמות נוחתת חזרה אל הקרקע

    }
    private void Walk()
    {
        transform.position += _speed * Time.fixedDeltaTime * _moveDirection;


        // _audioSource.PlayOneShot(_audioClip[6]); - טסט לסאונד כשהדמות הולכת


    }
    private void Jump()
    {
        _rb.AddForce(Vector2.up * _jumpForce);

        // _audioSource.PlayOneShot(_audioClip[6]); - טסט לסאונד כשהדמות קופצת


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
        if (!_isAlive)
            return;

        StartCoroutine(Hurt());
        UIManager.Instance.RemoveHeart();
        _player.Data.Health -= damage;

        _audioSource.PlayOneShot(_audioClip[1]); //תוספת של עומר, הסאונד פגיעה בשחקנית


    }
    public void HoldConversation()
    {
        StartCoroutine(Converse());
    }

    private IEnumerator HandleHitCollider()
    {
        yield return new WaitForSeconds(0.4f);
        _currentHitCollider = Instantiate(_player.Data.HitColliderGO, _player.Data.HitColliderTr).GetComponent<Collider2D>();
        EnemyDamager enemyDamager = _currentHitCollider.GetComponent<EnemyDamager>();
        enemyDamager.Damage = _player.Data.Power;
        yield return new WaitForSeconds(0.2f);

        Destroy(enemyDamager.gameObject);
    }


    private IEnumerator Hurt()
    {
        if (_player.Data.Health == 1)
            yield break;

        _isHurt = true;
        _animator.SetTrigger("WasHurt");


        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsTag("Hurt"))
        {
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        while (stateInfo.IsTag("Hurt") && stateInfo.normalizedTime < 1)
        {
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        _animator.ResetTrigger("WasHurt");
        _isHurt = false;

        //if (!_isAlive)
        //    _animator.SetTrigger("HasDied");
    }
    private IEnumerator Converse()
    {
        _animator.SetBool("IsConversing", true);

        yield return new WaitUntil(() => !_isStunned);
        _animator.SetBool("IsConversing", false);
    }


    private void Die()
    {
        if (!_isAlive)
            return;

        _isAlive = false;
        _animator.SetTrigger("HasDied");

        // _audioSource.PlayOneShot(_audioClip[8]); - טסט לסאונד כשהדמות גוססת למוות


        //if (_player.Data.Lives <= 0)
        //    Respawn();
    }
    private void Respawn()
    {
        transform.position = SpawnManager.Instance.PlayerSpawn.position;

        _isAlive = true;
    }
}
