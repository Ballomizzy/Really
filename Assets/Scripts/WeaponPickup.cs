using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField]
    private WeaponManager.WeaponType weaponType;
    [SerializeField]
    private GameObject pickupVFX;

    private bool picked;
    public bool isPickable;

    private AudioManager audioManager;
    private MapManager mapManager;

    private float rotateSpeed, rotateMethod;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        mapManager = FindObjectOfType<MapManager>();
        rotateSpeed = Random.Range(15, 20);
        rotateMethod = Random.Range(-1, 1); 
        if(rotateMethod == 0) rotateMethod = 1;
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (isPickable && other.gameObject.CompareTag("Player") && !picked)
        {
            picked = true;
            WeaponManager weaponManager = other.GetComponentInChildren<WeaponManager>();
            DropWeapon(weaponManager.currentWeapon);
            weaponManager.SwitchWeapon(weaponType);
            //Play sound here
            audioManager.PlaySFX("GunPickup");
            //Display particle here
            Destroy(Instantiate(pickupVFX, transform.position, Quaternion.identity), 1f);
            mapManager.RemovePickUpWeapons(this);
            Destroy(gameObject);
        }
    }

    public void DropWeapon(GameObject weaponToDrop)
    {
        WeaponPickup weaponGO = Instantiate(weaponToDrop, transform.position + new Vector3(Random.Range(0f, 1.5f), 0, Random.Range(0f, 2f)), transform.rotation).GetComponent<WeaponPickup>();
        weaponGO.isPickable = true;
        mapManager.AddPickUpWeapons(weaponGO);
    }

    private void Update()
    {
        if (isPickable)
        {
            transform.Rotate(new Vector3 (0, rotateSpeed * rotateMethod, 0) * Time.deltaTime);
        }
    }
}
