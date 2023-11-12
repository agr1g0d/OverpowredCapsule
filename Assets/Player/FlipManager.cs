using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FlipManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigitbody;
    public float RayDistance;
    public float yRotation;
    public float zRotationPerSecond;
    [NonSerialized] public bool IsFlipping;

    private int _direction;
    private bool directed;

    void FixedUpdate()
    {
        if (IsFlipping)
        {
            if (!directed)
            {
                if (_rigitbody.velocity.x > 0)
                {
                    directed = true;
                    _direction = 1;
                }
                else
                {
                    directed = true;
                    _direction = -1;
                }
            }
            _rigitbody.freezeRotation = false;
            
            _rigitbody.AddTorque(transform.up * yRotation * -_direction, ForceMode.VelocityChange);
            _rigitbody.AddTorque(Vector3.forward * zRotationPerSecond * -_direction, ForceMode.VelocityChange);
        } else
        {
            directed = false;
        }
    }
}
