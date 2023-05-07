using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : Damager
{
    protected const string _enemyTag = "Enemy";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_enemyTag))
        {
            Debug.Log($"Hit {other.name}");
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(_damage);
        }
    }
}
