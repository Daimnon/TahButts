using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Grandma : Enemy
{
    [SerializeField] private float _lookDistance, _interactionDistance, _yOffset, _stunDuration, _stunCooldown;
    private bool _isPlayerStunned = false;
    private IEnumerator _stunRoutine;

    private void Awake()
    {
        EnemyState = PlayerNotInsight;
        _stunRoutine = StunPlayer(_stunCooldown);
    }
    private void Update()
    {
        if (Data.Health <= 0)
            Die();

        DistanceFromTarget = Vector2.Distance(transform.position, Target.transform.position);

        if (DistanceFromTarget < _lookDistance && DistanceFromTarget > _interactionDistance)
            EnemyState = PlayerInsight;
        else if (DistanceFromTarget <= _interactionDistance)
            EnemyState = Interacting;
        else
            EnemyState = PlayerNotInsight;

        EnemyState.Invoke();
    }

    protected override void PlayerInsight() //chase after player
    {
        if (IsInteracting)
            IsInteracting = false;

        if (transform.position.x < Target.transform.position.x)
            Renderer.flipX = true;
        else
            Renderer.flipX = false;

        Debug.Log("Grandma sees you");
        // feedback for seeing player
    }
    protected override void PlayerNotInsight() //patrol
    {

    }
    protected override void Interacting()
    {
        if (!IsInteracting)
        {
            StartCoroutine(_stunRoutine);
            AnimController.SetBool("IsTalking", true);
            IsInteracting = true;
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator StunPlayer(float stunCooldown)
    {
        StartCoroutine(HandleStun());
        _stunRoutine = null;
        _stunRoutine = StunPlayer(stunCooldown);
        yield return new WaitForSeconds(stunCooldown);
        Debug.Log("StunPlayer");

        if (DistanceFromTarget <= _interactionDistance)
        {
            StartCoroutine(_stunRoutine);
            Debug.Log("Grandma is talking");
        }
        else
        {
            AnimController.SetBool("IsTalking", false);
            IsInteracting = false;
            Debug.Log("Grandma is confused");
        }
    }
    private IEnumerator HandleStun()
    {
        _isPlayerStunned = true;
        PlayerInputHandler player = Target.GetComponent<PlayerInputHandler>();
        player.Controller.Rb.velocity = Vector2.zero;
        player.Input.enabled = false;
        //player.Controller.IsStunned = true;
        yield return new WaitForSeconds(_stunDuration);

        Debug.Log("Free");
        //player.Controller.IsStunned = false;
        player.Input.enabled = true;
        _isPlayerStunned = false;
    }
}
