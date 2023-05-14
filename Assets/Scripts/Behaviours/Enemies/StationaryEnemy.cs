using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryEnemy : Enemy
{
    [SerializeField] private float _chaseDistance, _stopDistance, _yOffset;

    private void Update()
    {
        DistanceFromTarget = Vector2.Distance(transform.position, Target.transform.position);

        if (DistanceFromTarget < _chaseDistance && DistanceFromTarget > _stopDistance)
            PlayerInsight();
        else
            PlayerNotInsight();
    }

    protected override void PlayerInsight() //chase after player
    {
        if (transform.position.x < Target.transform.position.x)
            Renderer.flipX = true;
        else
            Renderer.flipX = false;

    }
    protected override void PlayerNotInsight() //patrol
    {

    }

    protected override void Interacting()
    {
        throw new NotImplementedException();
    }
}
