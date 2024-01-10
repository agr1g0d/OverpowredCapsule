using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Vector3 ToAim;

    private List<Weapon> _weapons = new List<Weapon>();
    private int _currentWeapon = 0;
    
    [SerializeField] Camera _camera;
    [SerializeField] Transform _aim;
    [SerializeField] Transform _playerTransform;
    [SerializeField] Transform _rightArm;
    [SerializeField] Transform _leftArm;
    [SerializeField] Transform _rightHand;
    [SerializeField] Transform _leftHand;
    [SerializeField] Vector3 _defoultArmEulers;
    [SerializeField] float _lerpCoeff;
    [SerializeField] FlipManager _flipManager;
    [SerializeField] Inventory _inventory;

    private void Start()
    {
        UpdateWeapons();
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
            _rightArm.rotation = Quaternion.LookRotation(ToAim);
        } else
        {
            _rightArm.eulerAngles = _playerTransform.eulerAngles;
        }
        if (_weapons.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) && _currentWeapon != 0)
            {
                ChangeWeapon(0);
            } else if (Input.GetKeyDown(KeyCode.Keypad2) && _currentWeapon != 1)
            {
                ChangeWeapon(1);
            } else if (Input.GetKeyDown(KeyCode.Keypad3) && _currentWeapon != 2)
            {
                ChangeWeapon(2);
            }

            if (_weapons[_currentWeapon].TypeWeapon == TypeWeapon.pistol)
            {
            }
            else if (_weapons[_currentWeapon].TypeWeapon == TypeWeapon.knife)
            {
            }
        }
    }

    public void UpdateWeapons()
    {
        foreach(var weapon in _weapons)
        {
            Destroy(weapon.gameObject);
        }
        for (int i = 0; i < _inventory.Weapons.Count; i++)
        {
            _weapons.Add(Instantiate(_inventory.Weapons[i], _rightHand));
            if (i != _currentWeapon)
            {
                _weapons[i].gameObject.SetActive(false);
            }
        }
    }

    public void ChangeWeapon(int index)
    {
        if (index > _weapons.Count - 1)
        {
            return;
        }
        foreach (var weapon in _weapons)
        {
            weapon.gameObject.SetActive(false);
        }
        _currentWeapon = index;
        _weapons[_currentWeapon].gameObject.SetActive(true);
    }

}
