using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerInput.OnFootActions _onFootActions;

    private PlayerManager _playerManager;
    // Start is called before the first frame update
    void Awake()
    {
        _playerInput = new PlayerInput();
        _onFootActions = _playerInput.OnFoot;
        _playerManager = GetComponent<PlayerManager>();
        _onFootActions.Jump.performed += ctx => _playerManager.Jump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _playerManager.MoveMec(_onFootActions.Movement.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        _onFootActions.Enable();
    }

    private void OnDisable()
    {
        _onFootActions.Disable();
    }
}
