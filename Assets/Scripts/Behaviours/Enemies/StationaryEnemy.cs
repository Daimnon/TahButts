using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryEnemy : Enemy
{
    [SerializeField] private float _chaseDistance, _stopDistance, _yOffset;

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

    }
    public override void LogicWhilePlayerNotInsight() //patrol
    {

    }
}
