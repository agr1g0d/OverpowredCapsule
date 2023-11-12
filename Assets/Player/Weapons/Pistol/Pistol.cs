using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] private Transform _bulletSpawner;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private ParticleSystem _effect;
    private float _timer = 999f;
    private FlipManager _flipManager;

    private void Start()
    {
        _flipManager = FindObjectOfType<FlipManager>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && _timer >= 1f / ShotsPerSecond && !_flipManager.IsFlipping)
        {
            _timer = 0;
            Instantiate(_bulletPrefab, _bulletSpawner.position, _bulletSpawner.rotation);
            _effect.Play();
        }
    }
}
