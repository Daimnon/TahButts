using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grandma : Enemy
{
    [SerializeField] private float _aggroDistance;

    private void Update()
    {
        _distanceFromTarget = Vector2.Distance(transform.position, _target.transform.position);

        if (_distanceFromTarget < _aggroDistance)
            LogicWhilePlayerInsight();
        else
            LogicWhilePlayerNotInsight();
    }

    public override void LogicWhilePlayerInsight() //stun after player
    {
        if (transform.position.x < _target.transform.position.x)
            _spriteRenderer.flipX = false;
        else
            _spriteRenderer.flipX = true;

        // stun player
    }
    public override void LogicWhilePlayerNotInsight() //idle
    {

    }
}
