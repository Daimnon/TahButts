using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : Damager
{
    protected const string PlayerTag = "Player";
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
        if (other.CompareTag(PlayerTag))
        {
            Debug.Log($"Hit {other.name}");
            PlayerInputHandler player = other.GetComponent<PlayerInputHandler>();
            player.Controller.TakeDamage(_damage);
        }
    }
}
