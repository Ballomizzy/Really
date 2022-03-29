using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField]
    private WeaponManager.WeaponType weaponType;
    [SerializeField]
    private GameObject pickupVFX;

    private bool picked;
    [SerializeField]private bool isPickable;
    private AudioManager audioManager;
    private MapManager mapManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (isPickable && other.gameObject.CompareTag("Player") && !picked)
        {
            picked = true;
            other.GetComponentInChildren<WeaponManager>().SwitchWeapon(weaponType);
            //Play sound here
            audioManager.PlaySFX("GunPickup");
            //Display particle here
            Destroy(Instantiate(pickupVFX, transform.position, Quaternion.identity), 1f);
            Destroy(gameObject);
        }
    }
}
