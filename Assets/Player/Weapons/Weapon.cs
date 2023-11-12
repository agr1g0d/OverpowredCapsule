using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string Type;
    public float ShotsPerSecond;
    public void DeleteWeapon()
    {
        Destroy(gameObject);
    }
}
