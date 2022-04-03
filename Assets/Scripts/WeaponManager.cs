using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public enum WeaponType
    {
        gun,
        shakabula,
        bulletOf3gun,
        shotgun,
        sword
    }
    [Header("Weapon Details")]
    [SerializeField]
    private WeaponType weaponType;
    private Weapon weapon;
    private float myTime;

    [Space]
    //Space

    [SerializeField]
    private GameObject gunGO, gunBullet, 
                       shakabulaGO, shakabulaBullet, 
                       bulletOf3GunGO, bulletOf3, 
                       shotgunGO, shotgunBullet,
                       swordGO;
    
    private Sword sword;

    private GameManager gameManager;

    private class BulletClass
    {
        public GameObject BulletGO;
        public float BulletSpeed;
        public float BulletImpact;

        public BulletClass(GameObject bulletGO, float bulletSpeed, float bulletImpact)
        {
            BulletGO = bulletGO;
            BulletSpeed = bulletSpeed;
            BulletImpact = bulletImpact;
        }
    }
    BulletClass newBulletClassInstance;

    public GameObject currentWeapon { get; private set; }

    private AudioManager audioManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioManager = gameManager.transform.GetComponent<AudioManager>();
        InitWeapon();
        myTime = weapon.NextTimeToFire + 1;
    }


    public void InitWeapon()
    {
        if(currentWeapon != null)
            Destroy(currentWeapon);

        switch (weaponType)
        {
            case WeaponType.gun:
                weapon = new Weapon("gun", 15, 0.05f, 1f);
                currentWeapon = Instantiate(gunGO, transform);
                break;
            case WeaponType.shakabula:
                weapon = new Weapon("shakabula", 50, 1f, 2f);
                currentWeapon = Instantiate(shakabulaGO, transform);
                break;
            case WeaponType.bulletOf3gun:
                weapon = new Weapon("bulletOf3Gun", 20, 0.05f, 1f);
                currentWeapon = Instantiate(bulletOf3GunGO, transform);
                break;
            case WeaponType.shotgun:
                weapon = new Weapon("shotgun", 40, 1.5f, 1.4f);
                currentWeapon = Instantiate(shotgunGO, transform);
                break;
            case WeaponType.sword:
                weapon = new Weapon("sword", 20, 0.02f, 0);
                currentWeapon = Instantiate(swordGO, transform);
                currentWeapon.transform.position += new Vector3(5, 0, 0);
                sword = GetComponentInChildren<Sword>();
                break;
            default:
                break;
        }
    }

 
    private void Update()
    {
        if(gameManager.gameState == GameManager.GameState.Playing)
            CheckFired();
        SwitchWeapon();
        
    }

    private void SwitchWeapon()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            weaponType = WeaponType.gun;
            InitWeapon();
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            weaponType = WeaponType.shakabula;
            InitWeapon();
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            weaponType = WeaponType.bulletOf3gun;
            InitWeapon();
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            weaponType = WeaponType.shotgun;
            InitWeapon();
        }
    }

    public void SwitchWeapon(WeaponType newWeaponType)
    {
        if (newWeaponType == WeaponType.gun)
        {
            weaponType = WeaponType.gun;
            InitWeapon();
        }
        else if (newWeaponType == WeaponType.shakabula)
        {
            weaponType = WeaponType.shakabula;
            InitWeapon();
        }
        else if (newWeaponType == WeaponType.bulletOf3gun)
        {
            weaponType = WeaponType.bulletOf3gun;
            InitWeapon();
        }
        else if (newWeaponType == WeaponType.shotgun)
        {
            weaponType = WeaponType.shotgun;
            InitWeapon();
        }
    }

    public void CheckFired()
    {
        myTime += Time.deltaTime;
        if (Input.GetButton("Fire1") && weaponType == WeaponType.gun && myTime > weapon.NextTimeToFire)
        {
            myTime = 0;
            Use();
        }
        else if (Input.GetButton("Fire1") && weaponType == WeaponType.bulletOf3gun && myTime > weapon.NextTimeToFire)
        {
            myTime = 0;
            Use();
        }
        else if (Input.GetButton("Fire1") && weaponType == WeaponType.sword && myTime > weapon.NextTimeToFire)
        {
            myTime = 0;
            Use();
        }
        else if (Input.GetButton("Fire1") && weaponType == WeaponType.shakabula && myTime > weapon.NextTimeToFire)
        {
            myTime = 0;
            Use();
        }
        else if (Input.GetButton("Fire1") && weaponType == WeaponType.shotgun && myTime > weapon.NextTimeToFire)
        {
            myTime = 0;
            Use();
        }

    }

    private void Use()
    {
        switch (weaponType)
        {
            case WeaponType.gun:
                Bullet newBullet = Instantiate(gunBullet, transform.position + transform.forward, Quaternion.identity).GetComponent<Bullet>();
                newBullet.transform.forward = transform.forward;
                newBulletClassInstance = new BulletClass(newBullet.gameObject, 50f, 1.3f);
                newBullet.SetBulletDetails(weapon.DamageAmount, newBulletClassInstance.BulletSpeed, .5f, newBulletClassInstance.BulletImpact);
                audioManager.PlaySFX("LaserGun", transform.position);
                break;

            case WeaponType.shakabula:
                Bullet newShakabulaBullet = Instantiate(shakabulaBullet, transform.position + transform.forward, Quaternion.identity).GetComponent<Bullet>();
                newShakabulaBullet.transform.forward = transform.forward;
                newBulletClassInstance = new BulletClass(newShakabulaBullet.gameObject, 25f, 2f);
                newShakabulaBullet.SetBulletDetails(weapon.DamageAmount, newBulletClassInstance.BulletSpeed, 2f, newBulletClassInstance.BulletImpact);
                audioManager.PlaySFX("Rocket", transform.position);
                break;
            case WeaponType.shotgun:
                Bullet newShotgunBullet = Instantiate(shotgunBullet, transform.position + transform.forward, Quaternion.identity).GetComponent<Bullet>();
                newShotgunBullet.transform.forward = transform.forward;
                newBulletClassInstance = new BulletClass(newShotgunBullet.gameObject, 25f, 2f);
                newShotgunBullet.SetBulletDetails(weapon.DamageAmount, newBulletClassInstance.BulletSpeed, 2f, newBulletClassInstance.BulletImpact);
                audioManager.PlaySFX("Shotgun", transform.position);
                break;
            case WeaponType.bulletOf3gun:
                Bullet new3SideBullet = Instantiate(bulletOf3, transform.position + transform.forward, Quaternion.identity).GetComponent<Bullet>();
                Bullet new3SideBullet1 = Instantiate(bulletOf3, transform.position + transform.forward, Quaternion.identity).GetComponent<Bullet>();
                Bullet new3SideBullet2 = Instantiate(bulletOf3, transform.position + transform.forward, Quaternion.identity).GetComponent<Bullet>();


                new3SideBullet.transform.forward = transform.forward;
                newBulletClassInstance = new BulletClass(new3SideBullet.gameObject, 50f, 1.3f);
                new3SideBullet.SetBulletDetails(weapon.DamageAmount, newBulletClassInstance.BulletSpeed, .5f, newBulletClassInstance.BulletImpact);

                new3SideBullet1.transform.forward = transform.forward + new Vector3(0.25f, 0f, 0f);
                newBulletClassInstance = new BulletClass(new3SideBullet1.gameObject, 50f, 1.3f);
                new3SideBullet1.SetBulletDetails(weapon.DamageAmount, newBulletClassInstance.BulletSpeed, .5f, newBulletClassInstance.BulletImpact);

                new3SideBullet2.transform.forward = transform.forward - new Vector3(0.25f, 0f, 0f);
                newBulletClassInstance = new BulletClass(new3SideBullet2.gameObject, 50f, 1.3f);
                new3SideBullet2.SetBulletDetails(weapon.DamageAmount, newBulletClassInstance.BulletSpeed, .5f, newBulletClassInstance.BulletImpact);

                audioManager.PlaySFX("LaserGun1", transform.position);
                break;
            case WeaponType.sword:
                sword.SwipeSword();
                sword.SetSwordDetails(weapon.DamageAmount);
                break;
            default:
                Debug.Log("Nothing!");
                break;
        }
    }
}
