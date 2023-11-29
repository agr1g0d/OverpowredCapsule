using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleSystems;
    [SerializeField] private Transform _player;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Movement _movement;
    [SerializeField] private FlipManager _jumpRotation;
    [SerializeField] private float _activationDistance;
    private Ray ray;

    private void Start()
    {
        for (int i = 0; i < _particleSystems.Length; i++)
        {
            _particleSystems[i].gameObject.SetActive(true);
        }
        ray = new Ray(_player.position - transform.up * 0.8f, Vector3.down);
    }
    private void Update()
    {
        ray.origin = _player.position - Vector3.up * 0.8f;
        if (_rigidbody.velocity.y < 0f)
        {
            ray.direction = Vector3.down;
        } else
        {
            ray.direction = -_movement.normal;
        }
        if (!Physics.Raycast(ray, _activationDistance) && _rigidbody.velocity.y < 0.5f && !_jumpRotation.IsFlipping)
        {
            for (int i = 0; i < _particleSystems.Length; i++)
            {
                _particleSystems[i].Play();
            }
        } else
        {
            for (int i = 0; i < _particleSystems.Length; i++)
            {
                _particleSystems[i].Stop();
            }
        }
    }
}
