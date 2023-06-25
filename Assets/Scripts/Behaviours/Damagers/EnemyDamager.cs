using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : Damager
{
    protected const string EnemyTag = "Enemy";
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.collider.CompareTag(_enemyTag))
    //    {
    //        Debug.Log($"Hit {collision.collider.name}");
    //        Enemy enemy = collision.collider.GetComponent<Enemy>();
    //        enemy.TakeDamage(_damage);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(EnemyTag))
        {
            Debug.Log($"Hit {other.name}");
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.AnimController.SetTrigger("GotPunched");
            enemy.TakeDamage(_damage);
        }
    }
}
