using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int weaponDamage;
    
    public bool isShooting, readyToShoot;
    private bool allowReset = true;
    public float shootingDelay = 2f;

    public int bulletsPerBurst = 3;
    [FormerlySerializedAs("currentBurst")] public int burstBulletsLeft;

    [Header("Spread")]
    public float spreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;
    internal Animator animator;

    [Header("Reloading")]

    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    

    public enum WeaponModel
    {
        Pistol,
        MP5
    }

    private bool isADS;

    public WeaponModel currentWeaponModel;
    
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;
    private static readonly int Recoil = Animator.StringToHash("Recoil");
    private static readonly int Reload1 = Animator.StringToHash("Reload");
    private static readonly int IsADS = Animator.StringToHash("isADS");
    private static readonly int ExitAds = Animator.StringToHash("exitADS");
    private static readonly int EnterAds = Animator.StringToHash("enterADS");
    private static readonly int RecoilAds = Animator.StringToHash("RecoilADS");

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;

        spreadIntensity = hipSpreadIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveWeapon)
        {
            if (Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }

            if (Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }
            
            
            
            GetComponent<Outline>().enabled = false;
            
            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.instance.emptySoundPistol.Play();
            }
        
            if (currentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }


            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading && WeaponManager.instance.CheckAmmoLeftFor(currentWeaponModel) >0)
            {
                Reload();
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }
        }
        
    }

    private void EnterADS()
    {
            animator.SetTrigger(EnterAds);
            isADS = true;
            HUDManager.instance.middleDot.SetActive(false);
            spreadIntensity = adsSpreadIntensity;
    }

    private void ExitADS()
    {
        
            animator.SetTrigger(ExitAds);
            isADS = false;
            HUDManager.instance.middleDot.SetActive(true);
            spreadIntensity = hipSpreadIntensity;

    }

    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        if (isADS)
        {
            animator.SetTrigger(RecoilAds);
        }
        else
        {
            animator.SetTrigger(Recoil);
        }
        SoundManager.instance.PlayShootingSound(currentWeaponModel);
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        SoundManager.instance.PlayReloadSound(currentWeaponModel);
        animator.SetTrigger(Reload1);
        
        isReloading = true;
        Invoke(nameof(ReloadCompleted), reloadTime);
        
    }

    private void ReloadCompleted()
    {
        if (WeaponManager.instance.CheckAmmoLeftFor(currentWeaponModel) > magazineSize)
        {
            bulletsLeft = magazineSize;
            WeaponManager.instance.DecreaseTotalAmmo(bulletsLeft, currentWeaponModel);
        }
        else
        {
            bulletsLeft = WeaponManager.instance.CheckAmmoLeftFor(currentWeaponModel);
            WeaponManager.instance.DecreaseTotalAmmo(bulletsLeft, currentWeaponModel);
        }
        isReloading = false;
    }

    void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        return direction + new Vector3(0, y, z);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}