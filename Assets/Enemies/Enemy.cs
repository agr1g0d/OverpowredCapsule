using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour
{
    public UnityEvent OnTakeDamageEnemy;
    public float MaxHP;
    public float HP;
    public bool InvulnerableForNow;
    public bool CanFly;
    [SerializeField] protected Rigidbody _rigidbody;

    public void TakeDamage(float damage)
    {
        if (!InvulnerableForNow)
        {
            if (HP - damage > 0)
            {
                HP -= damage;
                OnTakeDamageEnemy.Invoke();
            } else
            {
                Die();
            }
        }
    }

    public abstract void Die();

}
