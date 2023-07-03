using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashLoot : MonoBehaviour
{
    [SerializeField] private Enemy _owner;
    private const string _playerTag = "Player";

    /*private delegate void State();
    private State _state;*/

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(_playerTag))
        {
            _owner.ShouldWakeUp = true;
            // make sound
            // do effect
        }
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_playerTag))
        {
            if (_owner is Homeless)
                (_owner as Homeless).ShouldWakeUp = true;
            else if (_owner is Grandma)
                (_owner as Grandma).StunPlayer();
            // make sound
            // do effect
        }
    }
}
