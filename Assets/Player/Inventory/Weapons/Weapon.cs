using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : InventoryItem
{
    public TypeWeapon TypeWeapon;
    public float ShotsPerSecond;
    public float Damage;
    public Vector3 RightArmEulerAngle = Vector3.zero;
    public Vector3 LeftArmEulerAngle = Vector3.up * 180;
    public Vector3 RightArmLocalPoz = Vector3.forward * 0.33f;
    public Vector3 LeftArmLocalPoz = Vector3.forward * -0.33f;
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
