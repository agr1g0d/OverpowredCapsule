using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Hedgehog : MonoBehaviour
{
    public EnemyDamage ElectricityDamage;
    [SerializeField] private ParticleSystem _electricity;
    [SerializeField] private ParticleSystem _electricityHittingRay;
    [SerializeField] private ParticleSystem _jetpackFlame;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _forwardRayOrigin;
    [SerializeField] private Transform _ray;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private ContactEnemy _enemy;
    [SerializeField] private float _height;
    [SerializeField] private float _jetpackPower;
    [SerializeField] private float _electroHitDistance;
    private Ray _downRay;
    private Ray _forwardRay;
    private Transform _playerTransform;
    private bool _rayPlays;

    private void Start()
    {
        _playerTransform = FindAnyObjectByType<Player>().transform;
        _electricity.Play();
        _downRay = new Ray(transform.position, Vector3.down);
        _forwardRay = new Ray(_forwardRayOrigin.position, Vector3.right);
        _jetpackFlame.Play();
    }

    private void FixedUpdate()
    {
        _downRay.origin = transform.position;
        _forwardRay.origin = _forwardRayOrigin.position;
        if (_rigidbody.velocity.x > 0)
        {
            _forwardRay.direction = Vector3.right;
        } else if (_rigidbody.velocity.x < 0)
        {
            _forwardRay.direction = Vector3.left;
        }

        if (Physics.Raycast(_forwardRay, 2f, _layerMask) && _enemy.Triggered)
        {
            _rigidbody.AddForce(Vector3.up * _jetpackPower* 1.5f * Time.fixedDeltaTime, ForceMode.VelocityChange);
        } else if (Physics.Raycast(_downRay, _height))
        {
            _rigidbody.AddForce(Vector3.up * _jetpackPower * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        if (Physics.Raycast(_ray.position, _playerTransform.position - _ray.position, out RaycastHit hit, _electroHitDistance)
            && hit.collider.attachedRigidbody != null
            && hit.collider.attachedRigidbody.TryGetComponent(out Player p))
        {
            _ray.rotation = Quaternion.LookRotation(_playerTransform.position - _ray.position);
            _electricityHittingRay.startSpeed = 15 * (Vector3.Distance(_ray.position, _playerTransform.position) / _electroHitDistance);
            p.TakeDamage(ElectricityDamage, 0.25f);
            if (!_rayPlays)
            {
                _electricityHittingRay.Play();
                _rayPlays = true;
            }
        } else
        {
            _rayPlays = false;
            _electricityHittingRay.Stop();
        }
    }
}
