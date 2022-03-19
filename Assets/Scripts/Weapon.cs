using System.Collections;

public class Weapon
{
    public string Name { get; set; }
    public int DamageAmount { get; set; }
    public float NextTimeToFire { get; set; }
    public float ReloadTime { get; set; }

    public Weapon(string name, int damageAmount, float nextTimeToFire, float reloadTime)
    {
        Name = name;
        DamageAmount = damageAmount;    
        NextTimeToFire = nextTimeToFire;
        ReloadTime = reloadTime;
    }
}
