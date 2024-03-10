using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    [SerializeField]
    private GameObject _doors;

    private bool _doorOpen;
    
    
    protected override void Interact()
    {
        _doorOpen = !_doorOpen;
        _doors.GetComponent<Animator>().SetBool("IsOpen",_doorOpen);
    }
}
