using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : InventoryItem
{
    public TypeWeapon TypeWeapon;
    public float ShotsPerSecond;
    public float Damage;
    
}
public enum TypeWeapon
{
    pistol,
    knife,
    rifle,
    sword,
    shield,
    spell
}
