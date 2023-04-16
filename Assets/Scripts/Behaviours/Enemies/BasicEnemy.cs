using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    [SerializeField] private float _chaseSpeed, _chaseDistance, _stopDistance, _yOffset;

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
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;

        Vector3 offset = new(0.0f, _yOffset, 0.0f);
        transform.position = Vector2.MoveTowards(transform.position, _target.transform.position + offset, _chaseSpeed * Time.deltaTime);
    }
    public override void LogicWhilePlayerNotInsight() //patrol
    {

    }
}
