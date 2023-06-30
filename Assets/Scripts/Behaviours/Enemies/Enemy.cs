using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer Renderer;
    [SerializeField] protected EnemyData Data;
    [SerializeField] protected int AreaIndex;
    [SerializeField] protected float WabbleTime;
    protected Collider2D CurrentHitCollider;
    protected float DistanceFromTarget;
    protected bool IsInteracting = false;
    protected bool IsHurt;

    [SerializeField] private Animator _animController;
    public Animator AnimController => _animController;

    protected GameObject Target;
    
    protected delegate void State();
    protected State EnemyState;

    private void Start()
    {
        if (!Target)
            Target = GameManager.Instance.Player.gameObject;
        
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
        if (Data.Health > 0)
        {
            StartCoroutine(Wabble(transform.position, WabbleTime));
            _animController.SetTrigger("GotPunched");
            IsHurt = true;
        }

        Data.Health -= damage;
    }

    private IEnumerator Wabble(Vector3 startPos, float duration)
    {
        float time = 0;
        Vector3 newPos = transform.position;
        
        while (time < duration)
        {
            if (Renderer.flipX)
                newPos.x = startPos.x - 1;
            else
                newPos.x = startPos.x + 1;

            transform.position = newPos;
            time += Time.deltaTime;

            yield return null;
        }
        transform.position = startPos;
    }
}
