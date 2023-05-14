using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected GameObject Target;
    [SerializeField] protected SpriteRenderer Renderer;
    [SerializeField] protected EnemyData Data;
    protected Collider2D CurrentHitCollider;
    protected float DistanceFromTarget;
    protected bool IsInteracting = false;
    
    protected delegate void State();
    protected State EnemyState;

    protected abstract void PlayerInsight();
    protected abstract void PlayerNotInsight();
    protected abstract void Interacting();

    public void TakeDamage(int damage)
    {
        Data.Health -= damage;
    }
}
