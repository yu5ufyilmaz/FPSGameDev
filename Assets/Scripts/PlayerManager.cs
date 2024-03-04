using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    private CharacterController _controller;
    private bool _isGrounded = true;
    public Vector3 playerVelocity;

    public float speed = 5f;
    public float jumpHeight = 3f;
    public float gravity = -9.8f;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = _controller.isGrounded;
    }
    // It works with InputManager for character controller
    public void MoveMec(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        _controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (_isGrounded && playerVelocity.y <0)
        {
            playerVelocity.y = -2f;
        }

        _controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }
}
