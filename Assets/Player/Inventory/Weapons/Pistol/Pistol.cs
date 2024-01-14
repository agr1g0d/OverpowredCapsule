using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public float Power;
    [SerializeField] private Transform _bulletSpawner;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private GameObject _shot;
    [SerializeField] private InventoryManager _inventoryManager;
    private FlipManager _flipManager;
    private CameraFollow _camera;
    private float _timer = 999f;

    private void Start()
    {
        _flipManager = FindObjectOfType<FlipManager>();
        _camera = FindObjectOfType<CameraFollow>();
        _inventoryManager = FindObjectOfType<InventoryManager>();
    }

    protected override void Update()
    {
        base.Update();

        _timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) 
            && _timer >= 1f / ShotsPerSecond/*
          * && !_flipManager.IsFlipping*/ 
            && Hold
            && !_inventoryManager.OpenInventory)
        {
            _timer = 0;
            _effect.Play();
            _camera.Shake(Power / 1000);
            if (Physics.Raycast(_bulletSpawner.position, _bulletSpawner.forward, out RaycastHit info))
            {
                if (info.collider.attachedRigidbody != null)
                {
                    if (info.collider.attachedRigidbody.TryGetComponent(out Enemy enemy))
                    {
                        enemy.TakeDamage(Damage);
                    }
                    info.collider.attachedRigidbody.AddForceAtPosition(_bulletSpawner.forward * Power, info.point);
                }
                GameObject hit = Instantiate(_shot, _bulletSpawner.position, Quaternion.identity);
                StartCoroutine(SetPosAfterFrame(info.point, hit));
                Destroy(hit, 0.7f);
            }
        }
    }

    IEnumerator SetPosAfterFrame(Vector3 pos, GameObject gameObject)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();
        gameObject.transform.position = pos;
    }
}
