using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _player;
    [SerializeField] private float _speed = 10.0f;

    private Vector2 _moveInput = Vector2.zero;
    private Vector3 _moveDirection = Vector3.zero;
    private bool _isFacingLeft = false, _isCrouching = false;
    private bool _isAlive = true, _isStunned = false;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (_isStunned) return;
        _moveInput = context.ReadValue<Vector2>(); // read move input on moving mapped key (left stick)
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (_isStunned) return;

        if (context.started)
        {
            Debug.Log("Player Attacked.");
        }
        else Debug.LogError("Attack action failed: No logic.");
    }

    private void Update()
    {
        if (_player.Data.Health <= 0)
            Die();

        GetMoveDirection();
    }
    private void FixedUpdate()
    {
        FlipSprite();
        Walk();
    }

    private void GetMoveDirection()
    {
        _moveDirection = new(_moveInput.x, _moveInput.y, 0.0f);
    }
    private void UseGravity(bool isGrounded)
    {

    }

    private void FlipSprite()
    {
        Vector3 tempScale = transform.localScale;
        
        if (_moveInput.x < 0 && _isFacingLeft || _moveInput.x > 0 && !_isFacingLeft)
        {
            _isFacingLeft = !_isFacingLeft;
            tempScale.x *= -1;

            transform.localScale = tempScale;
        }
    }
    private void Walk()
    {
        transform.position += _speed * Time.fixedDeltaTime * _moveDirection;
    }
    private void TakeDamage(int damage)
    {
        _player.Data.Health -= damage;
    }

    private void CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector2.down, out hit, transform.lossyScale.y / 2))
        {
            //_rb.useGravity = false;

            //if (_isDebugMessagesOn) Debug.Log($"{name} is grounded");
        }
        else
        {
            //if (_isDebugMessagesOn) Debug.Log($"{name} is not grounded");
        }
    }
    private void Die()
    {
        _isAlive = false;

        if (_player.Data.Lives >= 0)
            Respawn();
    }
    private void Respawn()
    {
        transform.position = GameManager.Instance.Spawn.position;

        _isAlive = true;
    }
}
