using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagerKnockback : Damager
{
    [SerializeField] private float _knockbackHeight = 3.0f, _knockbackPower = 3.0f, _knockbackDuration = 1.0f;
    protected const string _playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_playerTag))
        {
            Debug.Log("Hit Player");
            PlayerInputHandler player = other.GetComponent<PlayerInputHandler>();
            player.Controller.TakeDamage(_damage);
            //Knockback(player);
        }
    }

    private void Knockback(PlayerInputHandler player)
    {
        Debug.Log("Knocking Back");
        player.Controller.IsStunned = true;
        StartCoroutine(LerpKnockback(player, _knockbackDuration));
    }
    private IEnumerator LerpKnockback(PlayerInputHandler player, float duration)
    {
        float time = 0;
        Vector3 startPosition = player.transform.position;
        Vector3 targetPosition = player.transform.position;

        if (player.Controller.IsFacingLeft)
            targetPosition.x -= _knockbackPower;
        else if (!player.Controller.IsFacingLeft)
            targetPosition.x += _knockbackPower;

        Vector3 targetOriginalY = targetPosition;
        targetOriginalY.y = startPosition.y;

        targetPosition.y += _knockbackHeight;

        while (time < duration)
        {
            if (time > duration / 2)
                targetPosition = Vector3.Lerp(targetPosition, targetOriginalY, time / duration);

            player.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        //player.transform.position = targetPosition;
        player.Controller.IsStunned = false;
    }
}
