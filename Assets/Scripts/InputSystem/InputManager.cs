using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    public PlayerInput.OnFootActions _onFootActions;

    private PlayerLook _playerLook;

    private PlayerManager _playerManager;
    // Start is called before the first frame update
    void Awake()
    {
        _playerInput = new PlayerInput();
        _onFootActions = _playerInput.OnFoot;
        _playerManager = GetComponent<PlayerManager>();
        _playerLook = GetComponent<PlayerLook>();
        _onFootActions.Jump.performed += ctx =>
        {
            Debug.Log("Jump performed");
            _playerManager.Jump();
        };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _playerManager.MoveMec(_onFootActions.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        _playerLook.LookMec(_onFootActions.Look.ReadValue<Vector2>());
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
