using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    [FormerlySerializedAs("weeaponSlots")] public List<GameObject> weaponSlots;
    public GameObject activeWeaponSlot;

    [Header("Ammo")] public int totalRifeAmmo = 0;
    public int totalPistolAmmo = 0;

    [Header("Throwables")] 
    public int grenades = 0;

    public float throwForce = 10f;
    public GameObject grenadePrefab;
    public GameObject throwableSpawn;
    public float forceMultiplier = 0f;
    public float forceMultiplierLimit = 2f;
    
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

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

        if (Input.GetKey(KeyCode.G))
        {
            forceMultiplier += Time.deltaTime;

            if (forceMultiplier > forceMultiplierLimit)
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            if (grenades >0)
            {
                ThrowLethal();
            }
            forceMultiplier = 0;
        }

        
    }
    
    public void PickupWeapon(GameObject pickedupWeapon)
    {
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }

    void AddWeaponIntoActiveSlot(GameObject pickedupWeapon)
    {
        DropCurrentWeapon(pickedupWeapon);
        
        pickedupWeapon.transform.SetParent(activeWeaponSlot.transform,false);

        Weapon weapon = pickedupWeapon.GetComponent<Weapon>();

        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y,weapon.spawnPosition.z);
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y,weapon.spawnRotation.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;
        
            weaponToDrop.transform.SetParent(pickedupWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedupWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedupWeapon.transform.localRotation;
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount >0 )
        {

            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount >0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }

    }

    public void PickupAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.RifleAmmo:
                totalRifeAmmo += ammo.ammoAmount;
                break;
        }
    }

    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModel currentWeaponModel)
    {
        switch (currentWeaponModel)
        {
            case Weapon.WeaponModel.MP5:
                totalRifeAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.Pistol:
                totalPistolAmmo -= bulletsToDecrease;
                break;
        }
    }
    
    public int CheckAmmoLeftFor(Weapon.WeaponModel currentWeaponModel)
    {
        switch (currentWeaponModel)
        {
            case Weapon.WeaponModel.MP5:
                return totalRifeAmmo;
            
            case Weapon.WeaponModel.Pistol:
                return totalPistolAmmo;
            default:
                return 0;
        }
    }

    public void PickupThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                PickupGrenade();
                break;
        }
    }

    private void PickupGrenade()
    {
        grenades += 1;
        HUDManager.instance.UpdateThrowables(Throwable.ThrowableType.Grenade);
    }
    
    private void ThrowLethal()
    {
        GameObject lethalPrefab = grenadePrefab;

        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        
        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier),ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        grenades -= 1;
        HUDManager.instance.UpdateThrowables(Throwable.ThrowableType.Grenade);
    }
}
