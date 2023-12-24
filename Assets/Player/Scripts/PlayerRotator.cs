using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private FlipManager _flipManager;
    [SerializeField] private Aim _aim;
    [SerializeField] private float _lerpCoeff;
    [SerializeField] private float _xAngle;
    [SerializeField] private float _yAngle;
    [SerializeField] private float _minSpeed;


    private void Update()
    {
        if (!_flipManager.IsFlipping)
        {
            Quaternion rotation = transform.localRotation;
            transform.localEulerAngles = Vector3.up * _aim.transform.localEulerAngles.x;
        }
    }
}
