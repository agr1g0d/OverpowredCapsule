using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private Aim _aim;
    [SerializeField] private FlipManager _flipManager;
    [SerializeField] private float _lerpCoeff;
    [SerializeField] private float _xAngle;
    [SerializeField] private float _minSpeed;


    private void Update()
    {
        if (!_flipManager.IsFlipping)
        {
            Vector3 toAimXZ = new Vector3(-_aim.ToAim.x, 0, Mathf.Abs(_aim.ToAim.y)); 
            _bodyTransform.transform.localRotation = Quaternion.Lerp(_bodyTransform.localRotation,
                Quaternion.LookRotation(toAimXZ, Vector3.up),
                Time.deltaTime * _lerpCoeff) ;
            int direction = 0;
            if (_rigidbody.velocity.x > _minSpeed)
            {
                direction = 1;
            } else if ( _rigidbody.velocity.x < -_minSpeed)
            {
                direction = -1;
            }
            _bodyTransform.localEulerAngles = new Vector3(Mathf.LerpAngle(_bodyTransform.localEulerAngles.x, _xAngle * direction, Time.deltaTime * _lerpCoeff), 
                _bodyTransform.localEulerAngles.y, 
                _bodyTransform.localEulerAngles.z);


        }
    }
}
