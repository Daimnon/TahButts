using System.Collections;
using UnityEngine;

public class Ars : Enemy
{
    [SerializeField] private float _wakingUpDistance = 9f, _inSightDistance = 7.0f, _chaseDistance = 6.0f, _interactionDistance = 1.0f;
    [SerializeField] private float _attackTime = 1.0f, _chaseSpeed = 2.0f, _yOffset = 0.0f;

    private GameObject _currentPlayerDamager;
    private PlayerController _playerController;
    private IEnumerator _attackRoutine;
    private string _punchAnimName;
    private int _attackCounter = 0, _attackResetCounter = 0;
    private bool _isAlive = true, _isAwake = false, _isWaiting = false;

    private AnimatorStateInfo _punchingStateInfo;

    private void Awake()
    {
        EnemyState = Sleep;
        //_punchingStateInfo = AnimController.Get
        //_attackRoutine = Attack();
    }
    private void Update()
    {
        Debug.Log(EnemyState.Method.Name);
        if (AnimController.GetCurrentAnimatorStateInfo(0).IsName("Anim_Ars_SitDown"))
        {
            // Debug
        }
    }
    private void FixedUpdate()
    {
        if (!_isAlive)
            return;

        if (Data.Health <= 0)
        {
            Die();
            return;
        }

        DistanceFromTarget = Vector2.Distance(transform.position, Target.transform.position);
        EnemyState.Invoke();   

        //Debug.Log(EnemyState.Method.Name);
    }

    private void Sleep()
    {
        if (DistanceFromTarget <= _wakingUpDistance)
        {
            AnimController.SetTrigger("HasAwoken");
            EnemyState = StandUp;
        }
    }
    private void StandUp()
    {
        if (_isAwake)
        {
            AnimController.SetTrigger("HasStoodUp");
            EnemyState = PlayerNotInsight;
            return;
        }

        AnimatorStateInfo standUpStateInfo = AnimController.GetCurrentAnimatorStateInfo(0);

        if (standUpStateInfo.normalizedTime >= standUpStateInfo.length)
            _isAwake = true;
    }
    protected override void PlayerNotInsight()
    {
        if (DistanceFromTarget <= _inSightDistance)
        {
            EnemyState = PlayerInsight;
            return;
        }
    }
    protected override void PlayerInsight()
    {
        if (DistanceFromTarget <= _interactionDistance)
        {
            AnimController.SetBool("IsChasingPlayer", false);
            //AnimController.SetTrigger("HasPunched");
            EnemyState = Interacting;
            return;
        }
        else if (DistanceFromTarget <= _chaseDistance)
        {
            AnimController.SetBool("IsChasingPlayer", true);
            EnemyState = ChasingPlayer;
            return;
        }
        else if (DistanceFromTarget > _inSightDistance)
        {
            AnimController.SetBool("IsChasingPlayer", false);
            EnemyState = PlayerNotInsight;
            return;
        }
    }
    private void ChasingPlayer()
    {
        if (DistanceFromTarget <= _interactionDistance)
        {
            //AnimController.SetBool("IsChasingPlayer", false);
            //AnimController.SetBool("HasPunched", true);
            EnemyState = Interacting;
            return;
        }
        else if (DistanceFromTarget > _chaseDistance)
        {
            AnimController.SetBool("IsChasingPlayer", false);
            EnemyState = PlayerInsight;
            return;
        }

        if (transform.position.x > Target.transform.position.x)
            Renderer.flipX = true;
        else
            Renderer.flipX = false;

        if (!_playerController)
            _playerController = Target.GetComponent<PlayerController>();

        Vector3 offset = new(0.0f, _yOffset, 0.0f);
        Vector2 lastMovePos = new(transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, Target.transform.position + offset, _chaseSpeed * Time.deltaTime);
    }
    protected override void Interacting()
    {
        _attackResetCounter = 0;
        AnimatorStateInfo stateInfo = AnimController.GetCurrentAnimatorStateInfo(0);

        if (_currentPlayerDamager)
            Destroy(_currentPlayerDamager);

        if (_attackCounter == 0)
        {
            Attack2();
            _attackCounter++;
            return;
        }
        else if (_attackCounter > 0 && stateInfo.IsTag("Punch") && stateInfo.normalizedTime >= _attackTime)
        {
            AnimController.ResetTrigger("HasPunched");
            _attackCounter = 0;
            _isWaiting = true;
            EnemyState = PostInteracting;
            return;
        }
    }
    private void PostInteracting()
    {
        if (_isWaiting && _attackResetCounter == 0)
        {
            StartCoroutine(ResetWait(_attackTime));
            AnimController.SetBool("IsStanding", true);
            _attackResetCounter++;
            return;
        }

        if (_isWaiting)
        {
            return;
        }
        if (DistanceFromTarget <= _interactionDistance)
        {
            AnimController.SetBool("IsStanding", false);
            EnemyState = Interacting;
            return;
        }
        else if (DistanceFromTarget <= _chaseDistance)
        {
            AnimController.SetBool("IsStanding", false);
            AnimController.SetBool("IsChasingPlayer", true);
            EnemyState = ChasingPlayer;
            return;
        }
        else if (DistanceFromTarget <= _inSightDistance)
        {
            EnemyState = PlayerInsight;
            return;
        }
        else if (!_isWaiting)
        {
            EnemyState = PlayerNotInsight;
            return;
        }
    }

    private void Die()
    {
        AnimController.SetTrigger("HasDied"); 
        AnimController.SetBool("IsAlive", false); 
        _isAlive = false;
        GameManager.Instance.InvokeEnemyDeath(this);
    }

    private void Attack2()
    {
        AnimController.SetTrigger("HasPunched");
        //StartCoroutine(GetPunchingAnimation());

        _punchingStateInfo = AnimController.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] clipInfo = AnimController.GetCurrentAnimatorClipInfo(0);
        _punchAnimName = clipInfo[0].clip.name;
        //clipInfo.no
        Debug.Log(_punchAnimName);

        Vector3 newScale = Data.HitColliderTr.localScale;

        if (Renderer.flipX)
            newScale.x = 1;
        else
            newScale.x = -1;

        Data.HitColliderTr.localScale = newScale;

        if (!_currentPlayerDamager)
        {
            _currentPlayerDamager = Instantiate(Data.HitColliderGO, Data.HitColliderTr);
            CurrentHitCollider = _currentPlayerDamager.GetComponent<Collider2D>();
        }

        //PlayerDamagerKnockback playerDamager;

        //if (CurrentHitCollider.TryGetComponent(out playerDamager))
        //    playerDamager.Damage = Data.Power;
    }
    private IEnumerator ResetWait(float time)
    {
        yield return new WaitForSeconds(time);
        _isWaiting = false;
    }
    private IEnumerator GetPunchingAnimation()
    {
        yield return null;

        while (_punchingStateInfo.normalizedTime < 1)
        {
            //_punchingStateInfo = AnimController.GetCurrentAnimatorStateInfo(0);
            AnimatorClipInfo[] clipInfo = AnimController.GetCurrentAnimatorClipInfo(0);
            _punchAnimName = clipInfo[0].clip.name;
            Debug.Log(_punchAnimName);
            yield return null;
        }
    }
}
