using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public Vector3 ToAim;
    public Weapon WeaponPrefab;

    private Weapon _weapon;
    
    [SerializeField] private Camera _camera;

    [SerializeField] private Transform _aim;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _rightArm;
    [SerializeField] private Transform _leftArm;
    [SerializeField] private Transform _rightHand;
    [SerializeField] private Transform _leftHand;

    [SerializeField] private Vector3 _defoultArmEulers;

    [SerializeField] private float _lerpCoeff;

    [SerializeField] private FlipManager _flipManager;

    private void Start()
    {
        _weapon = Instantiate(WeaponPrefab,_leftHand.position, Quaternion.Euler(Vector3.left * 90), _leftHand);

    }

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.back, Vector3.zero);
        float distance;
        plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance);
        _aim.position = point;
        ToAim = _aim.position - transform.position;
        transform.rotation = Quaternion.LookRotation(ToAim);
        if (!_flipManager.IsFlipping)
        {
            _head.localRotation = Quaternion.LookRotation(new Vector3(ToAim.x, 0, -Mathf.Abs(ToAim.y)));
        } else
        {
            _head.localRotation = Quaternion.Lerp(_head.localRotation, Quaternion.identity, Time.deltaTime * 10);
        }
        if (WeaponPrefab.Type == TypeWeapon.pistol)
        {
            
        } else if (WeaponPrefab.Type == TypeWeapon.knife)
        {
        }
    }

}
