using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerController
{
    event Action<float> OnMove;
    event Action OnInteract;
    event Action<bool> OnJump;
    event Action<bool> OnCrouch;
    event Action<bool> OnRun;
    event Action<bool> OnOpenInventory;
}

public class PlayerController : MonoBehaviour, IPlayerController
{
    private PlayerInput _playerInput;
    public event Action<float> OnMove;
    public event Action OnInteract;
    public event Action<bool> OnJump;
    public event Action<bool> OnCrouch;
    public event Action<bool> OnRun;
    public event Action<bool> OnOpenInventory;

    //Actions

    private InputAction _moveAction;
    private InputAction _interactAction;
    private InputAction _jumpAction;
    private InputAction _crouchAction;
    private InputAction _runAction;
    private InputAction _openInventoryAction;

    private float _moveDir;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        var actions = _playerInput.actions;
        _moveAction = actions["Move"];
        _interactAction = actions["Interact"];
        _jumpAction = actions["Jump"];
        _crouchAction = actions["Crouch"];
        _runAction = actions["Run"];
        _openInventoryAction = actions["Inventory"];

        SubscribeToEvents();

        if (TryGetComponent(out PlayerModel model))
        {
            model.SubscribeToController(this);
        }
    }

    private void SubscribeToEvents()
    {
        _moveAction.performed += OnMoveAction;
        _interactAction.performed += OnInteractAction;
        _jumpAction.performed += OnJumpAction;
        _jumpAction.canceled += OnJumpAction;
        _crouchAction.performed += OnCrouchAction;
        _crouchAction.canceled += OnCrouchAction;
        _runAction.performed += OnRunAction;
        _runAction.canceled += OnRunAction;
        _openInventoryAction.performed += OnOpenInventoryAction;
    }


    private void OnDisable()
    {
        UnsubscribeToEvents();
    }

    private void Update()
    {
        OnMove?.Invoke(_moveDir);
    }

    private void UnsubscribeToEvents()
    {
        _moveAction.performed -= OnMoveAction;
        _interactAction.performed -= OnInteractAction;
        _jumpAction.performed -= OnJumpAction;
        _jumpAction.canceled -= OnJumpAction;
        _crouchAction.performed -= OnCrouchAction;
        _crouchAction.canceled -= OnCrouchAction;
        _runAction.performed -= OnRunAction;
        _runAction.canceled -= OnRunAction;
        _openInventoryAction.performed -= OnOpenInventoryAction;
    }

    private void OnMoveAction(InputAction.CallbackContext context)
    {
        _moveDir = context.ReadValue<float>();
    }

    private void OnJumpAction(InputAction.CallbackContext context)
    {
        OnJump?.Invoke(context.ReadValueAsButton());
    }

    private void OnInteractAction(InputAction.CallbackContext context)
    {
        OnInteract?.Invoke();
    }

    private void OnCrouchAction(InputAction.CallbackContext context)
    {
        OnCrouch?.Invoke(context.ReadValueAsButton());
    }

    private void OnRunAction(InputAction.CallbackContext context)
    {
        OnRun?.Invoke(context.ReadValueAsButton());
    }

    private void OnOpenInventoryAction(InputAction.CallbackContext context)
    {
        OnOpenInventory?.Invoke(context.ReadValueAsButton());
    }
}