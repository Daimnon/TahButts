using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
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
            {
                (_owner as Homeless).ShouldWakeUp = true;
            }
            else if (_owner is OldPeople)
            {
                (_owner as OldPeople).PlayRandomOldPeopleLine();
                (_owner as OldPeople).StunPlayer();
            }
            else if (_owner is SmellyDude)
            {
                (_owner as SmellyDude).HarmPlayer();
            }
            else if (_owner is PhoneLady)
            {
                (_owner as PhoneLady).HarmPlayer();
            }
            // make sound
            // do effect
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(_playerTag))
        {
            if (_owner is SmellyDude)
                (_owner as SmellyDude).StopHarmingPlayer();
            else if (_owner is PhoneLady)
            {
                (_owner as PhoneLady).StopHarmingPlayer();
                //AudioManager.Instance.PlayFadeOutOneShot((_owner as PhoneLady).AudioSource, (_owner as PhoneLady).AudioClip) - work in progress, might discard
            }
            // make sound
            // do effect
        }
    }
}
