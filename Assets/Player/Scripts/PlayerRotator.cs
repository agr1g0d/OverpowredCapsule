using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private Transform _headTransform;
    [SerializeField] private WeaponManager _aim;
    [SerializeField] private FlipManager _flipManager;
    [SerializeField] private Movement _movement;
    [SerializeField] private float _lerpCoeff;
    [SerializeField] private float _xAngle;
    [SerializeField] private float _minSpeed;


    private void Update()
    {
        if (!_flipManager.IsFlipping)
        {
            int direction = 0;
            Vector3 toAimInverted;

            if (Vector3.Angle(_movement.normal, Vector3.up) < 40)
            {
                toAimInverted = new Vector3(-_aim.ToAim.x, 0, Mathf.Abs(_aim.ToAim.y));
                if (_rigidbody.velocity.x > _minSpeed)
                {
                    direction = 1;
                }
                else if (_rigidbody.velocity.x < -_minSpeed)
                {
                    direction = -1;
                }
            }
            else
            {
                toAimInverted = new Vector3(-_aim.ToAim.x, 0, _aim.ToAim.y);

                if (_rigidbody.velocity.y > _minSpeed)
                {
                    direction = 1;
                }
                else if (_rigidbody.velocity.y < -_minSpeed)
                {
                    direction = -1;
                }
            }

            _bodyTransform.localRotation = Quaternion.Lerp(_bodyTransform.localRotation,
                    Quaternion.LookRotation(toAimInverted, Vector3.up),
                    Time.deltaTime * _lerpCoeff);
            if (_rigidbody.velocity.x > _minSpeed)
            {
                direction = 1;
            }
            else if (_rigidbody.velocity.x < -_minSpeed)
            {
                direction = -1;
            }
            _bodyTransform.localEulerAngles = new Vector3(Mathf.LerpAngle(_bodyTransform.localEulerAngles.x, _xAngle * direction, Time.deltaTime * _lerpCoeff),
                _bodyTransform.localEulerAngles.y,
                _bodyTransform.localEulerAngles.z);

            _headTransform.localRotation = Quaternion.LookRotation(new Vector3(_aim.ToAim.x, 0, -Mathf.Abs(_aim.ToAim.y)));
            _headTransform.eulerAngles = new Vector3(_headTransform.eulerAngles.x, Mathf.Clamp(_headTransform.eulerAngles.y, 90, 270), _headTransform.eulerAngles.z);
           
        } else
        {
            _headTransform.localRotation = Quaternion.Lerp(_headTransform.localRotation, Quaternion.identity, Time.deltaTime * 10);
        }
    }
}
