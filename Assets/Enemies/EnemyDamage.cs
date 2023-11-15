using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public TypeDamage Type;
    public float Damage;
}
public enum TypeDamage
{
    common,
    electrical,
    fire,
    ice
}
