using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashLoot : MonoBehaviour
{
    [SerializeField] private Homeless _owner;
    private const string _playerTag = "Player";

    /*private delegate void State();
    private State _state;*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(_playerTag))
        {
            _owner.ShouldWakeUp = true;
            // make sound
            // do effect
        }
    }
}
