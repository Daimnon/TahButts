using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

public class Ars : Enemy
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _wakingUpDistance = 9f, _inSightDistance = 7.0f, _chaseDistance = 6.0f, _interactionDistance = 1.0f;
    [SerializeField] private float _attackTime = 1.0f, _chaseSpeed = 2.0f, _yOffset = 0.0f;

    private GameObject _currentPlayerDamager;
    private PlayerController _playerController;
    private IEnumerator _attackRoutine;
    private int _attackCounter = 0, _attackResetCounter = 0;
    private bool _isAwake = false, _isWaiting = false;

    private void Awake()
    {
        EnemyState = Sleep;
        //_attackRoutine = Attack();
    }
    private void FixedUpdate()
    {
        if (Data.Health <= 0)
            Die();

        DistanceFromTarget = Vector2.Distance(transform.position, Target.transform.position);
        EnemyState.Invoke();

        Debug.Log(EnemyState.Method.Name);
    }

    private void Sleep()
    {
        if (DistanceFromTarget <= _inSightDistance * _wakingUpDistance)
        {
            _animator.SetTrigger("HasAwoken");
            EnemyState = StandUp;
        }
    }
    private void StandUp()
    {
        if (_isAwake)
        {
            _animator.SetTrigger("HasStoodUp");
            EnemyState = PlayerNotInsight;
            return;
        }

        AnimatorStateInfo standUpStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (standUpStateInfo.normalizedTime >= standUpStateInfo.length)
            _isAwake = true;
    }
    protected override void PlayerNotInsight() //patrol
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
            _animator.SetBool("IsChasingPlayer", false);
            //_animator.SetTrigger("HasPunched");
            EnemyState = Interacting;
            return;
        }
        else if (DistanceFromTarget <= _chaseDistance)
        {
            _animator.SetBool("IsChasingPlayer", true);
            EnemyState = ChasingPlayer;
            return;
        }
        else if (DistanceFromTarget > _inSightDistance)
        {
            _animator.SetBool("IsChasingPlayer", false);
            EnemyState = PlayerNotInsight;
            return;
        }
    }
    private void ChasingPlayer()
    {
        if (DistanceFromTarget <= _interactionDistance)
        {
            _animator.SetBool("IsChasingPlayer", false);
            //_animator.SetBool("HasPunched", true);
            EnemyState = Interacting;
            return;
        }
        else if (DistanceFromTarget > _chaseDistance)
        {
            _animator.SetBool("IsChasingPlayer", false);
            EnemyState = PlayerInsight;
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
    protected override void Interacting()
    {
        _attackResetCounter = 0;

        if (_currentPlayerDamager)
            Destroy(_currentPlayerDamager);

        if (_attackCounter == 0)
        {
            Attack2();
            _attackCounter++;
        }
        else if (_attackCounter > 0)
        {
            if (_currentPlayerDamager)
                Destroy(_currentPlayerDamager);
        }

        AnimatorStateInfo punchingStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (punchingStateInfo.normalizedTime >= _attackTime)
        {
            //_animator.SetBool("IsPunching", false);
            _attackCounter = 0;
            //_animator.SetBool("IsPunching", false);
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
            _attackResetCounter++;
            return;
        }

        if (DistanceFromTarget <= _interactionDistance)
        {
            _animator.SetBool("IsPunching", true);
            EnemyState = Interacting;
            return;
        }
        else if (DistanceFromTarget <= _chaseDistance)
        {
            _animator.SetBool("IsChasingPlayer", true);
            EnemyState = ChasingPlayer;
            return;
        }
        else if (DistanceFromTarget <= _inSightDistance)
        {
            EnemyState = PlayerInsight;
            return;
        }
        else
        {
            EnemyState = PlayerNotInsight;
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
        _animator.SetBool("HasPunched", true);
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
    private IEnumerator ResetWait(float time)
    {
        yield return new WaitForSeconds(time);
        _isWaiting = false;
    }
}
