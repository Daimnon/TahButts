using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public enum OldPeopleType { Grandma, Grandpa }

public class OldPeople : Enemy
{
    [SerializeField] private OldPeopleType _type;
    [SerializeField] private float _lookDistance, _interactionDistance, _yOffset, _stunDuration, _stunCooldown;
    [SerializeField] private float _playerPassDistance = 2.0f;
    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private AudioSource _audioSource;
    public AudioSource AudioSource => _audioSource;

    [SerializeField] private AudioClip[] _lineClips, _grandpaClips;
    public AudioClip[] LineClips => _lineClips;

    private bool _isPlayerStunned = false;
    private IEnumerator _stunRoutine;

    private void Awake()
    {
        EnemyState = PlayerNotInsight;
        _stunRoutine = DoStun(_stunCooldown);
    }
    private void Update()
    {
        /*if (Data.Health <= 0)
            Die();*/
        /*DistanceFromTarget = Vector2.Distance(transform.position, Target.transform.position);


        if (DistanceFromTarget < _lookDistance && DistanceFromTarget > _interactionDistance)
            EnemyState = PlayerInsight;
        else if (DistanceFromTarget <= _interactionDistance)
            EnemyState = Interacting;
        else
            EnemyState = PlayerNotInsight;

        EnemyState.Invoke();*/

        /*if (Target.transform.position.x < transform.position.x - _playerPassDistance)
            GameManager.Instance.InvokeEnemyPass(this);*/
    }

    protected override void PlayerInsight() //chase after player
    {
        IsInteracting = false;

        if (transform.position.x > Target.transform.position.x)
            Renderer.flipX = true;
        else
            Renderer.flipX = false;

        Debug.Log("Grandma sees you");
        // feedback for seeing player
    }
    protected override void PlayerNotInsight() //patrol
    {
        /*if (Target.transform.position.x < transform.position.x - _playerPassDistance)
            GameManager.Instance.InvokeEnemyPass(this);*/
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

    public OldPeopleType GetOldPeopleType()
    {
        return _type;
    }
    /*private void Die()
    {
        Destroy(gameObject);
    }*/
    public void StunPlayer()
    {
        AnimController.SetBool("IsTalking", true);
        IsInteracting = true;

        StartCoroutine(_stunRoutine);
    }
    private IEnumerator DoStun(float stunCooldown)
    {
        StartCoroutine(HandleStun());
        _stunRoutine = null;
        _stunRoutine = DoStun(stunCooldown);
        yield return new WaitForSeconds(stunCooldown);
        Debug.Log("StunPlayer");

        /*if (DistanceFromTarget <= _interactionDistance)
        {
            StartCoroutine(_stunRoutine);
            Debug.Log("Grandma is talking");
        }
        else
        {
            AnimController.SetBool("IsTalking", false);
            IsInteracting = false;
            Debug.Log("Grandma is confused");
        }*/
    }
    private IEnumerator HandleStun()
    {
        _isPlayerStunned = true;
        PlayerInputHandler player = Target.GetComponent<PlayerInputHandler>();
        player.Controller.Rb.velocity = Vector2.zero;
        player.Input.enabled = false;
        player.Controller.Animator.SetTrigger("WasStunned");
        _renderer.enabled = true;
        //player.Controller.IsStunned = true;
        yield return new WaitForSeconds(_stunDuration);

        Debug.Log("Free");
        //player.Controller.IsStunned = false;
        player.Input.enabled = true;
        player.Controller.Animator.ResetTrigger("WasStunned");
        _renderer.enabled = false;
        _isPlayerStunned = false;
        AnimController.SetBool("IsTalking", false);
        IsInteracting = false;
    }

    public void PlayRandomOldPeopleLine()
    {
        int clipIndex = 0;
        AudioClip clip;

        clipIndex = UnityEngine.Random.Range(0, _lineClips.Length);
        clip = _lineClips[clipIndex];

        _audioSource.PlayOneShot(clip);
    }
}
