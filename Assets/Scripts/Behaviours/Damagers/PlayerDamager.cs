using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : Damager
{
    protected const string PlayerTag = "Player";

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
