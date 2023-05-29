using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected GameObject Target;
    [SerializeField] protected Animator AnimController;
    [SerializeField] protected SpriteRenderer Renderer;
    [SerializeField] protected EnemyData Data;
    protected Collider2D CurrentHitCollider;
    protected float DistanceFromTarget;
    protected bool IsInteracting = false;
    
    protected delegate void State();
    protected State EnemyState;

    private void Start()
    {
        SpawnManager.Instance.SpawnedEnemyList.Add(this);
    }
    private void OnDestroy()
    {
        SpawnManager.Instance.SpawnedEnemyList.Remove(this);
    }

    protected abstract void PlayerInsight();
    protected abstract void PlayerNotInsight();
    protected abstract void Interacting();

    public void TakeDamage(int damage)
    {
        Data.Health -= damage;
    }

}
