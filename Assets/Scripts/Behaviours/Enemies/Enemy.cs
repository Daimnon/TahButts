using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected GameObject _target;
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected EnemyData _enemyData;

    protected float _distanceFromTarget;

    public abstract void LogicWhilePlayerInsight();
    public abstract void LogicWhilePlayerNotInsight();
    public void TakeDamage(int damage)
    {
        _enemyData.Health -= damage;
    }
}
