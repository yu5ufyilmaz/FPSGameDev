using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private CharacterController _controller;
    private bool isGrounded;
    private Vector3 _playerVelocity;

    [SerializeField] private float speed = 5f;
    public float jumpHeight = 3f;
    private float gravity = -9.8f;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = _controller.isGrounded;
    }
    // It works with InputManager for character controller
    public void MoveMec(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        _controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        _playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && _playerVelocity.y <0)
        {
            _playerVelocity.y = -2f;
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }
}
