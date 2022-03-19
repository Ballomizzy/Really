using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Health myHealth;
    [SerializeField]
    private float currentHealthAmount;

    private GameUIManager UIManager;

    private void Awake()
    {
        myHealth = new Health("Player", 300);
        UIManager = FindObjectOfType<GameUIManager>();
    }

    private void Update()
    {
        currentHealthAmount = myHealth.CurrentHealthAmount;
        UIManager.healthBarUI.fillAmount = myHealth.CurrentHealthAmount / myHealth.HealthMaxAmount;
        if (currentHealthAmount <= 0)
            Die();
    }

    public Health GetPlayerHealth()
    {
        return myHealth;
    }

    public void Die()
    {
        Debug.Log("You die die die die mf!");
    }
}
