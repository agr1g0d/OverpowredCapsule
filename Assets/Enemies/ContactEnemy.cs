using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactEnemy : Enemy
{
    public EnemyDamage ContactDamage;
    public bool Triggered { get; private set; }

    private Transform _playerTransform;
    private Quaternion _defaultRotation;
    private Ray _checkingRay;
    private Vector3 _toTarget;
    [SerializeField] private float _speed;
    [SerializeField] private float _checkRadius;
    [SerializeField] private bool _keepCalm;
    [SerializeField] private ConfigurableJoint _joint;
    [SerializeField] private Transform _checkingRayOrigin;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private LayerMask _playerLayer;

    private void Start()
    {
        _playerTransform = FindObjectOfType<Player>().transform;
        _defaultRotation = transform.rotation;
        _checkingRay = new Ray(_checkingRayOrigin.position, Vector3.right);
        _toTarget = Vector3.right;
    }

    private void FixedUpdate()
    {
        _checkingRay.origin = _checkingRayOrigin.position;
        if (Physics.Raycast(transform.position, _playerTransform.position - transform.position,out RaycastHit hit,_checkRadius) 
            && hit.collider.attachedRigidbody != null 
            && hit.collider.attachedRigidbody.TryGetComponent(out Player p))
        {
            _toTarget = _playerTransform.position - transform.position;
            _toTarget = _toTarget.normalized;
            Triggered = true;
            if (_rigidbody.velocity.x > 0)
            {
                _checkingRay.direction = Vector3.right;
            }
            else if (_rigidbody.velocity.y < 0)
            {
                _checkingRay.direction = Vector3.left;
            }
            
        } else if (Physics.Raycast(_checkingRay, 2f, _layerMask))
        {
            Triggered = false;
            _checkingRay.direction *= -1;
            _toTarget = _checkingRay.direction;
        } else
        {
            Triggered = false;
        }

        if (!CanFly)
        {
            _toTarget = new Vector3(_toTarget.x, 0, _toTarget.z);
        }
        _joint.targetRotation = Quaternion.Inverse(Quaternion.LookRotation(new Vector3(_toTarget.x, 0, _toTarget.z))) * _defaultRotation;
        if (!_keepCalm)
        {
            _rigidbody.AddForce(_toTarget * _speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

    override public void Die()
    {
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            if (other.attachedRigidbody.TryGetComponent(out Player player))
            {
                player.TakeDamage(ContactDamage, 1f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (collision.rigidbody.TryGetComponent(out Player player))
            {
                player.TakeDamage(ContactDamage, 1f);
            }
        }

    }
}
