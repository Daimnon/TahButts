using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Ars : Enemy
{
    [SerializeField] private float _chaseSpeed, _chaseDistance, _stopDistance, _yOffset, _attackSpeed;
    private IEnumerator _attackRoutine;

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
        if (IsInteracting)
            IsInteracting = false;

        if (transform.position.x < Target.transform.position.x)
            Renderer.flipX = true;
        else
            Renderer.flipX = false;

        Vector3 offset = new(0.0f, _yOffset, 0.0f);
        transform.position = Vector2.MoveTowards(transform.position, Target.transform.position + offset, _chaseSpeed * Time.deltaTime);
    }
    protected override void PlayerNotInsight() //patrol
    {
        if (IsInteracting)
            IsInteracting = false;
    }
    protected override void Interacting()
    {
        if (!IsInteracting)
        {
            StartCoroutine(_attackRoutine);
            IsInteracting = true;
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator HitPlayer(float attackeSpeed)
    {
        StartCoroutine(HandleHitCollider());
        _attackRoutine = null;
        _attackRoutine = HitPlayer(_attackSpeed);
        yield return new WaitForSeconds(attackeSpeed);

        if (DistanceFromTarget <= _stopDistance)
            StartCoroutine(_attackRoutine);
    }
    private IEnumerator HandleHitCollider()
    {
        CurrentHitCollider = Instantiate(Data.HitColliderGO, Data.HitColliderTr).GetComponent<Collider2D>();
        PlayerDamagerKnockback playerDamager = CurrentHitCollider.GetComponent<PlayerDamagerKnockback>();
        playerDamager.Damage = Data.Power;
        yield return new WaitForSeconds(0.2f);

        Destroy(CurrentHitCollider.gameObject);
    }
}
