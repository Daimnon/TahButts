using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

public class Ars : Enemy
{
    [SerializeField] private float _chaseSpeed = 2.0f, _inSightDistance = 3.0f, _interactionDistance = 1.0f, _yOffset = 0.0f, _attackSpeed = 1.0f;
    [SerializeField] private Animator _animator;
    
    private PlayerController _playerController;
    private IEnumerator _attackRoutine;
    private int _attackCounter = 0;
    private bool _isAwake = false, _isStandingUp = false, _isChasingPlayer = false, _isPunching = false;

    private void Awake()
    {
        EnemyState = Sleep;
        //_attackRoutine = Attack();
    }
    private void Update()
    {
        if (Data.Health <= 0)
            Die();

        DistanceFromTarget = Vector2.Distance(transform.position, Target.transform.position);
        EnemyState.Invoke();
    }

    private void Sleep()
    {
        if (DistanceFromTarget <= _inSightDistance * 2)
            EnemyState = StandUp;
    }
    private void StandUp()
    {
        if (_isAwake && DistanceFromTarget > _inSightDistance)
        {
            EnemyState = PlayerNotInsight;
            return;
        }

        if (!_isAwake && !_isStandingUp)
        {
            _animator.SetTrigger("HasAwoken");
            _isStandingUp = true;
            return;
        }
        else if (_isAwake)
        {
            _animator.SetBool("IsChasingPlayer", true);
            EnemyState = PlayerInsight;
            return;
        }

        AnimatorStateInfo standUpStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (standUpStateInfo.normalizedTime >= standUpStateInfo.length)
            _isAwake = true;
    }
    protected override void PlayerInsight() //chase after player
    {
        if (DistanceFromTarget <= _interactionDistance)
        {
            _animator.SetTrigger("HasPunched");
            //_hasPuncehdRecently = true;
            EnemyState = Interacting;
            return;
        }
        else if (DistanceFromTarget > _inSightDistance)
        {
            _animator.SetBool("IsChasingPlayer", false);
            EnemyState = PlayerNotInsight;
            return;
        }

        if (transform.position.x < Target.transform.position.x)
            Renderer.flipX = true;
        else
            Renderer.flipX = false;

        if (!_playerController)
            _playerController = Target.GetComponent<PlayerController>();

        Vector3 offset = new(0.0f, _yOffset, 0.0f);
        Vector2 lastMovePos = new(transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, Target.transform.position + offset, _chaseSpeed * Time.deltaTime);
    }
    private void ChasingPlayer()
    {

    }
    protected override void Interacting()
    {
        AnimatorStateInfo punchingStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (_attackCounter == 0)
        {
            //StartCoroutine(_attackRoutine);
            Attack2();
            _animator.SetBool("IsPunching", true);
            _attackCounter++;
        }

        if (punchingStateInfo.normalizedTime >= punchingStateInfo.length)
        {
            _animator.SetBool("IsPunching", false);
            _attackCounter = 0;
            EnemyState = PlayerInsight;
            return;
        }
    }
    protected override void PlayerNotInsight() //patrol
    {
        if (DistanceFromTarget < _inSightDistance)
        {
            EnemyState = PlayerInsight;
            return;
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }

    /*private IEnumerator HitPlayer(float attackSpeed)
    {
        StartCoroutine(Attack());
        _attackRoutine = null;
        _attackRoutine = HitPlayer(_attackSpeed);
        yield return new WaitForSeconds(attackSpeed);

        if (_playerController.IsAlive && DistanceFromTarget <= _stopDistance)
            StartCoroutine(_attackRoutine);
    }*/
    private void Attack2()
    {
        _isPunching = true;
        Vector3 newScale = Data.HitColliderTr.localScale;

        if (Renderer.flipX)
            newScale.x = 1;
        else
            newScale.x = -1;

        Data.HitColliderTr.localScale = newScale;

        GameObject playerDamager = Instantiate(Data.HitColliderGO, Data.HitColliderTr);
        CurrentHitCollider = playerDamager.GetComponent<Collider2D>();

        //PlayerDamagerKnockback playerDamager;

        //if (CurrentHitCollider.TryGetComponent(out playerDamager))
        //    playerDamager.Damage = Data.Power;
    }
    /*private IEnumerator Attack()
    {
        _isPunching = true;

        Vector3 newScale = Data.HitColliderTr.localScale;

        if (Renderer.flipX)
            newScale.x = 1;
        else
            newScale.x = -1;

        Data.HitColliderTr.localScale = newScale;
        CurrentHitCollider = Instantiate(Data.HitColliderGO, Data.HitColliderTr).GetComponent<Collider2D>();

        //PlayerDamagerKnockback playerDamager;

        //if (CurrentHitCollider.TryGetComponent(out playerDamager))
        //    playerDamager.Damage = Data.Power;
        
        yield return new WaitForSeconds(0.5f);

        Destroy(CurrentHitCollider.gameObject);

        _isPunching = false;
        _attackRoutine = null;
        _attackRoutine = Attack();
    }*/
    /*private IEnumerator ResetPunch(float time)
    {
        yield return new WaitForSeconds(time);
        _hasPuncehdRecently = false;
    }*/
}
