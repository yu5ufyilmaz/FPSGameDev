using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [FormerlySerializedAs("shootingSoundPistol")] public AudioSource ShootingChannel;
    public AudioClip PistolShot;
    public AudioClip MP5Shot;
    
    public AudioSource reloadingSoundPistol;
    public AudioSource reloadingSoundMP5;
    
    public AudioSource emptySoundPistol;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol:
                ShootingChannel.PlayOneShot(PistolShot);
                break;
            case Weapon.WeaponModel.MP5:
                ShootingChannel.PlayOneShot(MP5Shot);
            break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol:
                reloadingSoundPistol.Play();
                break;
            case Weapon.WeaponModel.MP5:
                reloadingSoundMP5.Play();
                break;
        }
    }

}
