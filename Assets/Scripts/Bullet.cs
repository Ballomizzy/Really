using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damageAmount;
    private float bulletSpeed;
    private float bulletTime = 2, bulletImpact;
    private bool notHitAnything = true;

    private AudioManager audioManager;

    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        Destroy(gameObject, bulletTime);
    }
    void Update()
    {
        if (notHitAnything)
            transform.position += transform.TransformDirection(0, 0, bulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        notHitAnything = false;
        if(col.gameObject.GetComponent<NPC>())
        {
            Debug.Log("I hit someone!");
            col.gameObject.GetComponent<NPC>().enemyHealth.GetDamage(damageAmount);
            col.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * bulletImpact * 0.1f, ForceMode.Impulse);
            audioManager.PlaySFX("Ouch", col.transform.position);
        }
        Debug.Log("I hit another stuff");
        Destroy(gameObject);
    }

    public void SetBulletDetails(int _damageAmount, float _bulletSpeed, float _bulletTime, float _bulletImpact)
    {
        damageAmount = _damageAmount;
        bulletSpeed = _bulletSpeed; 
        bulletTime = _bulletTime;
        bulletImpact = _bulletImpact;
    }
}
