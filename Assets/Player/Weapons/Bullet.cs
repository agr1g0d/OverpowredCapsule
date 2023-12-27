using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float Damage = 1;
    public float Power = 10;
    [SerializeField] private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody.AddForce(transform.forward * Power, ForceMode.VelocityChange);
        StartCoroutine(DestroiAfterSrcond());
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (!enemy.InvulnerableForNow)
            {
                enemy.HP -= Damage;
                enemy.OnTakeDamageEnemy.Invoke();
                if (enemy.HP <= 0)
                {
                    enemy.Die();
                }
            }
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (!enemy.InvulnerableForNow)
            {
                enemy.HP -= Damage;
                enemy.OnTakeDamageEnemy.Invoke();
                if (enemy.HP <= 0)
                {
                    enemy.Die();
                }
            }
        }
        Destroy(gameObject);
    }

    IEnumerator DestroiAfterSrcond()
    {
        yield return new WaitForSeconds(1);
        if (gameObject != null)
            Destroy(gameObject);
    }
}
