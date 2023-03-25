using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    public PlayerInput Input => _input;

    [SerializeField] private PlayerData _data;
    public PlayerData Data => _data;

    [SerializeField] private PlayerController _controller;
    public PlayerController Controller => _controller;

    private PlayerControls _controls;

    private void Awake()
    {
        _controls = new PlayerControls();
        _input.onActionTriggered += OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext obj)
    {
        if (!_controller.IsAlive || _controller.IsStunned)
            return;

        if (obj.action.name == _controls.Player.Movement.name)
        {
            _controller.OnMove(obj);
        }

        if (obj.action.name == _controls.Player.Jump.name)
        {
            _controller.OnJump(obj);
        }

        if (obj.action.name == _controls.Player.Attack.name)
        {
            _controller.OnAttack(obj);
        }

        if (obj.action.name == _controls.Player.InteractOne.name)
        {
            _controller.OnInteractOne(obj);
        }

        if (obj.action.name == _controls.Player.InteractTwo.name)
        {
            _controller.OnInteractTwo(obj);
        }
    }

    private void OnDestroy()
    {
        _input.onActionTriggered -= OnActionTriggered;
    }
}
