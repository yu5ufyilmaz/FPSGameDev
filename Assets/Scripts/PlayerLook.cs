using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;

    private float _xRotation = 0f;

    private float _xSensivity = 30f;
    private float _ySensivity = 30f;

    public void LookMec(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        _xRotation -= (mouseY * Time.deltaTime) * _ySensivity;
        _xRotation = Mathf.Clamp(_xRotation, -80, 80);
        
        cam.transform.localRotation = Quaternion.Euler(_xRotation,0,0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) *_xSensivity);
    }
}
