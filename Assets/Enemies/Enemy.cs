using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour
{
    public UnityEvent OnTakeDamageEnemy;
    public float MaxHP;
    public float HP;
    public bool ImmortalForNow;
    public bool CanFly;
    [SerializeField] protected Rigidbody _rigidbody;

    public abstract void Die();

}
