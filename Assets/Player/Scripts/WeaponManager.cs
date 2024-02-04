using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Vector3 ToAim;
    public Transform Arms;
    public Transform RightArm;
    public Transform LeftArm;
    public Transform RightHand;
    public Transform LeftHand;

    private List<Weapon> _weapons = new List<Weapon>();
    private int _currentWeapon = 0;
    private bool _lookToPointer = true;

    
    [SerializeField] Camera _camera;
    [SerializeField] Transform _aim;
    [SerializeField] Transform _playerTransform;
    [SerializeField] Vector3 _defoultArmEulers;
    [SerializeField] float _lerpCoeff;
    [SerializeField] FlipManager _flipManager;
    [SerializeField] InventoryManager _inventoryManager;

    private void Start()
    {
    }

    private void Update()
    {
        if (!_inventoryManager.OpenInventory)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.back, Vector3.zero);
            float distance;
            plane.Raycast(ray, out distance);
            Vector3 point = ray.GetPoint(distance);
            _aim.position = point;
            ToAim = _aim.position - transform.position;
            transform.rotation = Quaternion.LookRotation(ToAim);
        }


        if (_weapons.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && _currentWeapon != 0)
            {
                ChangeWeapon(0);
            } else if (Input.GetKeyDown(KeyCode.Alpha2) && _currentWeapon != 1)
            {
                ChangeWeapon(1);
            } else if (Input.GetKeyDown(KeyCode.Alpha3) && _currentWeapon != 2)
            {
                ChangeWeapon(2);
            }
        }
        if (!_flipManager.IsFlipping)
        {
            if (_lookToPointer)
            {
                Arms.rotation = Quaternion.LookRotation(ToAim);
            } else
            {
                Arms.rotation = Quaternion.Lerp(Arms.rotation, Quaternion.LookRotation(-_playerTransform.forward, _playerTransform.up), Time.deltaTime * 10);
            }
        }
        else
        {
            Arms.eulerAngles = _playerTransform.eulerAngles;
        }
    }

    public void AddWeapon(Weapon weapon)
    {
        _weapons.Add(weapon);
        if (_weapons.IndexOf(weapon) != _currentWeapon)
        {
            weapon.PickUp(RightHand, true);
        } else
        {
            SetArmsProperties();
            weapon.PickUp(RightHand, false);
        }
    }

    private void SetArmsProperties()
    {
        RightArm.localEulerAngles = _weapons[_currentWeapon].RightArmEulerAngle;
        RightArm.localPosition = _weapons[_currentWeapon].RightArmLocalPoz;
        LeftArm.SetParent(Arms);
        LeftArm.localPosition = _weapons[_currentWeapon].LeftArmLocalPoz;
        LeftArm.localEulerAngles = _weapons[_currentWeapon].LeftArmEulerAngle;
        if (!_weapons[_currentWeapon].RotateLeftHand)
        {
            LeftArm.SetParent(_playerTransform);
            LeftArm.localPosition =  new Vector3(-_weapons[_currentWeapon].LeftArmLocalPoz.z, 
                _weapons[_currentWeapon].LeftArmLocalPoz.y, 
                -_weapons[_currentWeapon].LeftArmLocalPoz.x)  
                + Arms.localPosition;
            LeftArm.localEulerAngles = _weapons[_currentWeapon].LeftArmEulerAngle + Vector3.up * -90;
        } else
        {
            LeftArm.SetParent(Arms);
            LeftArm.localPosition = _weapons[_currentWeapon].LeftArmLocalPoz;
            LeftArm.localEulerAngles = _weapons[_currentWeapon].LeftArmEulerAngle;
        }
        _lookToPointer = _weapons[_currentWeapon].LookToPointer;
        print(_currentWeapon);
    }

    public void __UpdateWeapons()
    {
        foreach(var weapon in _weapons)
        {
            Destroy(weapon.gameObject);
        }
        for (int i = 0; i < _inventoryManager.Weapons.Count; i++)
        {
            _weapons.Add(_inventoryManager.Weapons[i]);
            _inventoryManager.Weapons[i].PickUp(RightHand, false);
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
        SetArmsProperties();
        
    }

}
