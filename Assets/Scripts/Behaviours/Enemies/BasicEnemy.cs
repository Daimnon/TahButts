using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    [SerializeField] private float _chaseSpeed, _chaseDistance, _stopDistance;

    private void Update()
    {
        _distanceFromTarget = Vector2.Distance(transform.position, _target.transform.position);

        if (_distanceFromTarget < _chaseDistance && _distanceFromTarget > _stopDistance)
            LogicWhilePlayerInsight();
        else
            LogicWhilePlayerNotInsight();
    }

    public override void LogicWhilePlayerInsight() //chase after player
    {
        if (transform.position.x < _target.transform.position.x)
            _spriteRenderer.flipX = false;
        else
            _spriteRenderer.flipX = true;

        transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _chaseSpeed * Time.deltaTime);
    }
    public override void LogicWhilePlayerNotInsight() //patrol
    {

    }
}
