using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Ars : Enemy
{
    [SerializeField] private float _chaseSpeed, _chaseDistance, _stopDistance, _yOffset, _attackSpeed;
    [SerializeField] private Animator _animator;
    
    private PlayerController _playerController;
    private IEnumerator _attackRoutine;
    private float _moveSpeed = 0;
    private bool _isAwake;

    private void Awake()
    {
        EnemyState = PlayerNotInsight;
        _attackRoutine = HitPlayer(_attackSpeed);
    }
    private void Update()
    {
        if (Data.Health <= 0)
            Die();

        DistanceFromTarget = Vector2.Distance(transform.position, Target.transform.position);

        if (DistanceFromTarget < _chaseDistance && DistanceFromTarget > _stopDistance)
            EnemyState = PlayerInsight;
        else if (DistanceFromTarget <= _stopDistance)
            EnemyState = Interacting;
        else
            EnemyState = PlayerNotInsight;

        EnemyState.Invoke();
    }

    protected override void PlayerInsight() //chase after player
    {
        if (!_isAwake)
        {
            _animator.SetBool("IsAwoken", true);
            _isAwake = true;
        }

        if (IsInteracting)
        {
            _animator.SetBool("IsPunching", false);
            IsInteracting = false;
        }

        if (transform.position.x < Target.transform.position.x)
            Renderer.flipX = true;
        else
            Renderer.flipX = false;

        _playerController = Target.GetComponent<PlayerController>();

        Vector3 offset = new(0.0f, _yOffset, 0.0f);

        Vector2 lastMovePos = new(transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, Target.transform.position + offset, _chaseSpeed * Time.deltaTime);

        _moveSpeed = 1;
        _animator.SetFloat("Speed", _moveSpeed);
    }
    protected override void PlayerNotInsight() //patrol
    {
        if (IsInteracting)
        {
            IsInteracting = false;
            _moveSpeed = 0;
            _animator.SetFloat("Speed", _moveSpeed);
        }
    }
    protected override void Interacting()
    {
        if (!IsInteracting)
        {
            _animator.SetBool("IsPunching", true);
            StartCoroutine(_attackRoutine);
            IsInteracting = true;
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator HitPlayer(float attackSpeed)
    {
        StartCoroutine(HandleHitCollider());
        _attackRoutine = null;
        _attackRoutine = HitPlayer(_attackSpeed);
        yield return new WaitForSeconds(attackSpeed);

        if (_playerController.IsAlive && DistanceFromTarget <= _stopDistance)
            StartCoroutine(_attackRoutine);
    }
    private IEnumerator HandleHitCollider()
    {
        CurrentHitCollider = Instantiate(Data.HitColliderGO, Data.HitColliderTr).GetComponent<Collider2D>();

        PlayerDamagerKnockback playerDamager;

        if (CurrentHitCollider.TryGetComponent(out playerDamager))
            playerDamager.Damage = Data.Power;
        
        yield return new WaitForSeconds(0.2f);

        Destroy(CurrentHitCollider.gameObject);
    }
}
