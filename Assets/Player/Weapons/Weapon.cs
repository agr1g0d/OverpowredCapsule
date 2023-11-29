using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public TypeWeapon Type;
    public float ShotsPerSecond;
    public void DeleteWeapon()
    {
        Destroy(gameObject);
    }
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
