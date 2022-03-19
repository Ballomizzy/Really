using System.Collections;

public class Health
{
    public string Name { get; set; }
    public float CurrentHealthAmount { get; set; }
    public float HealthMaxAmount { get; set; }

    public Health(string name, float healthMaxAmount)
    {
        Name = name;
        CurrentHealthAmount = healthMaxAmount;
        HealthMaxAmount = healthMaxAmount;
    }

    public void GetDamage(int damageAmount)
    {
        CurrentHealthAmount -= damageAmount;
    }

}
