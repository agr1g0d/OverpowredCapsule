using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private float _impulseEnoughToExplode;
    [SerializeField] private bool _drawGizmos;
    public float Force;
    public float Radius;
    [SerializeField] private bool _active;
    private bool _done;

    private void Update()
    {
        if (_active)
            Explode();
    }

    public void ExplodeWithDelay()
    {
        Invoke("Explode", 0.2f);
    }

    private void Explode()
    {
        if (_done)
            return;
        _done = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(Force, transform.position, Radius);
                Explosive explosive = rb.GetComponent<Explosive>();
                if (explosive)
                {
                    if (Vector3.Distance(transform.position, explosive.transform.position) < Radius / 2f)
                    {
                        explosive.ExplodeWithDelay();
                    }
                }
            }
        }
        Destroy(gameObject);
        Instantiate(_effect, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        if (!_drawGizmos)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius / 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude >= _impulseEnoughToExplode)
        {
            ExplodeWithDelay();
        }
    }
}
