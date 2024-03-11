using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask;
    private PlayerUI _playerUI;
    private InputManager _inputManager;
    

    private void Start()
    {
        _inputManager = GetComponent<InputManager>();
        cam = GetComponent<PlayerLook>().cam;
        _playerUI = GetComponent<PlayerUI>();
    }

    private void Update()
    {
        _playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(cam.transform.position,cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Debug.Log("Tepkiver");
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                _playerUI.UpdateText(interactable.promptMessage);
                if (_inputManager._onFootActions.Interact.triggered)
                {
                    Debug.Log("Interacted");
                    interactable.BaseInteract();
                }
            }
        }
    }
}
