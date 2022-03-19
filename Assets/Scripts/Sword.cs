using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Animator swordAnim;
    private int damageAmount;

    private void Awake()
    {
        swordAnim = GetComponent<Animator>();
    }

    public void SetSwordDetails(int _damageAmount)
    {
        damageAmount = _damageAmount;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<NPC>())
        {
            Debug.Log("I hit someone!");
            col.gameObject.GetComponent<NPC>().enemyHealth.GetDamage(damageAmount);
        }
        Debug.Log("I hit another stuff");
    }

    public void SwipeSword()
    {
        //swipe sword
        swordAnim.SetTrigger("Stop");
        swordAnim.SetBool("inverse", System.Convert.ToBoolean(Random.Range(0, 2)));
        swordAnim.SetTrigger("Swipe");
    }
}
