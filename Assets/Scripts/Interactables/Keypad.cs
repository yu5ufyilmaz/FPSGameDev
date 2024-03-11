using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    [SerializeField]
    private GameObject _doors;

    private bool _doorOpen;
    
    
    // ReSharper disable Unity.PerformanceAnalysis
    protected override void Interact()
    {
        Debug.Log("Door Open");
        _doorOpen = !_doorOpen;
        _doors.GetComponent<Animator>().SetBool("IsOpen",_doorOpen);
    }
}
