using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public Weapon WeaponPrefab;
    private Weapon _weapon;
    private bool _instantiatedRight;
    private bool _instantiatedLeft;

    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _aim;
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
        Vector3 toAim = _aim.position - transform.position;
        transform.rotation = Quaternion.LookRotation(toAim);
        if (WeaponPrefab.Type == "Pistol")
        {
            if (transform.localEulerAngles.y < 180f && !_flipManager.IsFlipping)
            {
                _instantiatedRight = false;
                if (!_instantiatedLeft)
                {
                    _weapon.DeleteWeapon();
                    _weapon = Instantiate(WeaponPrefab, _leftHand, false);
                    _instantiatedLeft = true;
                }
                float localAngleX = _leftArm.localEulerAngles.x;
                if (localAngleX > 180)
                {
                    localAngleX -= 360;
                    localAngleX *= -1;
                }
                _leftArm.LookAt(_aim);
                _leftArm.localPosition = new Vector3(_leftArm.localPosition.x, Mathf.Lerp(1.1f,1.6f, localAngleX / 90f), _leftArm.localPosition.z);
                _rightArm.localPosition = new Vector3(_rightArm.localPosition.x, 
                    Mathf.Lerp(_rightArm.localPosition.y, 1.1f, Time.deltaTime * _lerpCoeff),
                    _rightArm.localPosition.z);
                if (_leftArm.localEulerAngles.x > 60 && _leftArm.localEulerAngles.x < 270)
                {
                    _leftArm.localEulerAngles = new Vector3(60, _leftArm.localEulerAngles.y, _leftArm.localEulerAngles.z);
                }
                _rightArm.localRotation = Quaternion.Lerp(_rightArm.localRotation,Quaternion.Euler(-_defoultArmEulers), _lerpCoeff * Time.deltaTime);
            }
            else if (transform.localEulerAngles.y >= 180f && !_flipManager.IsFlipping)
            {
                _instantiatedLeft = false;
                if (!_instantiatedRight)
                {
                    _weapon.DeleteWeapon();
                    _weapon = Instantiate(WeaponPrefab, _rightHand, false);
                    _instantiatedRight = true;
                }
                float localAngleX = _rightArm.localEulerAngles.x;
                if (localAngleX > 180)
                {
                    localAngleX -= 360;
                    localAngleX *= -1;
                }
                _rightArm.LookAt(_aim);
                _rightArm.localPosition = new Vector3(_rightArm.localPosition.x, Mathf.Lerp(1.1f, 1.6f, localAngleX / 90f), _rightArm.localPosition.z);
                _leftArm.localPosition = new Vector3(_leftArm.localPosition.x,
                    Mathf.Lerp(_leftArm.localPosition.y, 1.1f, Time.deltaTime * _lerpCoeff),
                    _leftArm.localPosition.z);
                _leftArm.localRotation = Quaternion.Lerp(_leftArm.localRotation, Quaternion.Euler(_defoultArmEulers), _lerpCoeff * Time.deltaTime);
                if (_rightArm.localEulerAngles.x > 60 && _rightArm.localEulerAngles.x < 270)
                {
                    _rightArm.localEulerAngles = new Vector3(60, _rightArm.localEulerAngles.y, _rightArm.localEulerAngles.z);
                }
            }
        }
        

    }
}
