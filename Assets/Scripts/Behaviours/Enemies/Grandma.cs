using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Grandma : Enemy
{
    [SerializeField] private float _detectionRadius, _lookDistance, _interactionDistance, _yOffset, _stunDuration, _stunCooldown;
    private bool _isPlayerStunned = false;
    private IEnumerator _stunRoutine;

    private void Awake()
    {
        EnemyState = PlayerNotInsight;
        _stunRoutine = StunPlayer();
    }
    private void Update()
    {
        if (Data.Health <= 0)
            Die();

        DistanceFromTarget = Vector2.Distance(transform.position, Target.transform.position);

        if (DistanceFromTarget < _lookDistance && DistanceFromTarget > _interactionDistance)
            EnemyState = PlayerInsight;
        else if (!_isPlayerStunned && DistanceFromTarget <= _detectionRadius)
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

        // feedback for seeing player
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
            StartCoroutine(_stunRoutine);
            IsInteracting = true;
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator StunPlayer()
    {
        _isPlayerStunned = true;
        PlayerInputHandler player = Target.GetComponent<PlayerInputHandler>();
        player.Controller.Rb.velocity = Vector2.zero;
        player.Controller.IsStunned = true;
        yield return new WaitForSeconds(_stunDuration);

        player.Controller.IsStunned = false;
        _isPlayerStunned = false;
        yield return new WaitForSeconds(_stunCooldown);
        IsInteracting = false;
    }
}
