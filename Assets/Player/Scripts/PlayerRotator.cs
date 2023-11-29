using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private FlipManager _flipManager;
    [SerializeField] private float _lerpCoeff;
    [SerializeField] private float _xAngle;
    [SerializeField] private float _yAngle;
    [SerializeField] private float _minSpeed;


    private void Update()
    {
        if (!_flipManager.IsFlipping)
        {
            Vector3 eulers = _bodyTransform.localEulerAngles;
            float xMultiplier = 0;
            int _direction = 0;

            if (_rigidbody.velocity.magnitude > _minSpeed)
            {
                xMultiplier = 1;
            }    
            if (Mathf.Abs(_rigidbody.velocity.x) > Mathf.Abs(_rigidbody.velocity.y))
            {
                if (_rigidbody.velocity.x > 0.1f)
                {
                    _direction = 1;
                }
                else if (_rigidbody.velocity.x < -0.1f)
                {
                    _direction = -1;
                }
                if (Vector3.Angle(_bodyTransform.right, Vector3.right) < 90)
                {
                    _direction *= -1;
                }
            } else if (Mathf.Abs(_rigidbody.velocity.x) < Mathf.Abs(_rigidbody.velocity.y) && Mathf.Abs(_rigidbody.velocity.y) > 0.1f)
            {
                if (_rigidbody.velocity.y > 0.1f)
                {
                    _direction = -1;
                }
                else if (_rigidbody.velocity.y < -0.1f)
                {
                    _direction = 1;
                }
                
                xMultiplier = 4f;
                if (Vector3.Angle(_bodyTransform.right, Vector3.up) > 90)
                {
                    _direction *= -1;
                }
            }
            _bodyTransform.localEulerAngles = new Vector3(Mathf.LerpAngle(eulers.x, -_xAngle * xMultiplier, Time.deltaTime * _lerpCoeff), 
                Mathf.LerpAngle(eulers.y, _direction * _yAngle, Time.deltaTime * _lerpCoeff), 
                eulers.z);

        }
    }
}
