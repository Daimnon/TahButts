using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneLady : Enemy
{
    [SerializeField] private float _playerPassDistance = 2.0f;
    //[SerializeField] private 

    private void Update()
    {
        if (Target.transform.position.x < transform.position.x - _playerPassDistance)
            GameManager.Instance.InvokeEnemyPass(this);
    }
    protected override void Interacting()
    {
        throw new System.NotImplementedException();
    }

    protected override void PlayerInsight()
    {
        throw new System.NotImplementedException();
    }

    protected override void PlayerNotInsight()
    {
        throw new System.NotImplementedException();
    }
}
