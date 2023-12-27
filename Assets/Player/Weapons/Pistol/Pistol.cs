using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public float Power;
    [SerializeField] private Transform _bulletSpawner;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private FlipManager _flipManager;
    private float _timer = 999f;

    private void Start()
    {
        _flipManager = FindObjectOfType<FlipManager>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && _timer >= 1f / ShotsPerSecond/* && !_flipManager.IsFlipping*/)
        {
            _timer = 0;
            _effect.Play();
            if (Physics.Raycast(_bulletSpawner.position, _bulletSpawner.forward, out RaycastHit info) 
                && info.collider.attachedRigidbody != null 
                && info.collider.attachedRigidbody.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(Damage);
                info.collider.attachedRigidbody.AddForceAtPosition(_bulletSpawner.forward * Power, info.point);
            }
        }
    }
}
