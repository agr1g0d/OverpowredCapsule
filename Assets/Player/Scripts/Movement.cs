using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed;
    public float MaxSpeed;
    public float SpeedMultiplier;
    public float JumpForce;
    public float horizontalFriction;
    public Vector3 normal {get; private set;}

    [SerializeField] private float _walkingRotation;
    [SerializeField] private float _lerpMultiplyer;
    [SerializeField] private float _maxAngularVelocity;
    [SerializeField] private float _wallrunStrenght;
    [SerializeField] private float _wallrunDistance;
    [SerializeField] private ConstantForce _constantForce;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private PhysicMaterial _phMaterial;
    [SerializeField] private Transform _crouchingBody;
    [SerializeField] private Transform _body;
    [SerializeField] private FlipManager _flipManager;
    [SerializeField] private WallConnection _wallConnectionPrefab;
    [SerializeField] private WeaponManager _aim;
    [SerializeField] private InventoryManager _inventoryManager;

    private Vector3 _stickContactPoint;
    private Vector3 _jumpDirection;
    private Vector3 _additionalForce;
    private Ray _downRay;
    private Ray _rightRay;
    private Ray _leftRay;
    private RaycastHit _raycastInfo;
    private float _lerpCoeff;
    private float _timer;
    private float _crouchTimer;
    private bool _touchsThng;
    private bool _grounded;
    private bool _wallRun;
    private bool _speedUp;
    private bool _firstRayCast;
    private bool _shouldNotStick;
    private WallConnection _connection;

    private void Start()
    {
        _additionalForce = _constantForce.force;
        _speedUp = false;
        _timer = 0;
        _rigidbody.maxAngularVelocity = _maxAngularVelocity;
        _downRay = new Ray(transform.position, Vector3.down);
        _rightRay = new Ray(transform.position, Vector3.right);
        _leftRay = new Ray(transform.position, Vector3.left);
        _flipManager.IsFlipping = false;
        _connection = null;
    }

    private void FixedUpdate()
    {
        _downRay.origin = transform.position;
        _rightRay.origin = transform.position;
        _leftRay.origin = transform.position;
       
        if (Physics.Raycast(_downRay, _flipManager.RayDistance) || Physics.Raycast(_rightRay, out _raycastInfo, _flipManager.RayDistance) || Physics.Raycast(_leftRay, out _raycastInfo, _flipManager.RayDistance))
        {
            _flipManager.IsFlipping = false;
            if (_firstRayCast)
            {
                _firstRayCast = false;
                _lerpCoeff = Mathf.Abs(_rigidbody.velocity.magnitude) * 1.7f;
                if (_lerpCoeff < 20)
                {
                    _lerpCoeff = 20f;
                }
            }
            Quaternion playerRotation = Quaternion.identity;
            if ((Physics.Raycast(_rightRay, _flipManager.RayDistance) || Physics.Raycast(_leftRay, _flipManager.RayDistance)) && !Physics.Raycast(_downRay, _flipManager.RayDistance))
            {
                playerRotation = Quaternion.LookRotation(Vector3.forward, _raycastInfo.normal);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, playerRotation, Time.fixedDeltaTime * _lerpCoeff);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, 0f), Time.fixedDeltaTime * 20);
        }
        else
        {
            _flipManager.IsFlipping = true;
            _firstRayCast = true;
        }

        //*****************************************************************************************
        float speedMultiplier = 1;

        if (!_grounded && !_touchsThng)
        {
            speedMultiplier = 0.15f;
        }

        if (!_grounded && _touchsThng)
        {
            speedMultiplier = 0;
        }
        if (_rigidbody.velocity.x > MaxSpeed && Input.GetAxis("Horizontal") > 0f)
        {
            speedMultiplier = 0;
        }
        if (_rigidbody.velocity.x < -MaxSpeed && Input.GetAxis("Horizontal") < 0f)
        {
            speedMultiplier = 0;
        }

        if (!_inventoryManager.OpenInventory)
        {
            _rigidbody.AddForce(transform.right * Speed * speedMultiplier * Input.GetAxis("Horizontal"), ForceMode.VelocityChange);

            if (_wallRun)
            {
                if (_connection != null)
                {
                    _rigidbody.AddForce(0, _wallrunStrenght * Time.fixedDeltaTime * Input.GetAxis("Vertical"), 0, ForceMode.VelocityChange);
                    _connection.Move(_wallrunStrenght * Time.fixedDeltaTime * Input.GetAxis("Vertical"));
                }
            }
        }
        if (_grounded)
        {
            _rigidbody.AddForce(-_rigidbody.velocity.x * horizontalFriction, 0, 0, ForceMode.VelocityChange);
        }
    }


    private void Update()
    {
        //wallrun checking
        if (Vector3.Distance(transform.position, _stickContactPoint) < _wallrunDistance && _touchsThng)
        {
            _wallRun = true;
        }
        else
        {
            _wallRun = false;
            _phMaterial.staticFriction = 1f;
            _phMaterial.dynamicFriction = 1f;
            _phMaterial.frictionCombine = PhysicMaterialCombine.Maximum;
        }
        //shift speed up
        if (Input.GetKey(KeyCode.LeftShift) && _grounded && !_speedUp)
        {
            _speedUp = true;
            Speed *= SpeedMultiplier;
            MaxSpeed *= SpeedMultiplier;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && _speedUp)
        {
            _speedUp = false;
            Speed /= SpeedMultiplier;
            MaxSpeed /= SpeedMultiplier;
        }
        //jump
        if (Input.GetKeyDown(KeyCode.Space) && _touchsThng && !_inventoryManager.OpenInventory)
        {
            if (_connection != null)
            {
                _connection.Disconnect();
                _connection = null;
            }
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(_jumpDirection.normalized * JumpForce, ForceMode.VelocityChange);
        }
        //crouch
        if (_rigidbody.velocity.y > 0 && !_touchsThng)
        {
            _crouchTimer = 0f;
            _crouchingBody.localScale = Vector3.Lerp(_crouchingBody.localScale, new Vector3(1, 0.6f, 1), Time.deltaTime * _lerpMultiplyer);
        } else if (!_grounded && (_touchsThng || _crouchTimer > 0))
        {
            _crouchTimer += 1;
            _crouchingBody.localScale = Vector3.Lerp(_crouchingBody.localScale, new Vector3(1, 0.7f, 1), Time.deltaTime * _lerpMultiplyer);
        } else
        {
            _crouchTimer = 0f;
            _crouchingBody.localScale = Vector3.Lerp(_crouchingBody.localScale, Vector3.one, Time.deltaTime * _lerpMultiplyer);
        }
        //idle
        if (_rigidbody.velocity == Vector3.zero)
        {
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.attachedRigidbody != null && collision.collider.attachedRigidbody.TryGetComponent(out Enemy e))
        {
            _shouldNotStick = true;
            return;
        }
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (Vector3.Angle(Vector3.up, collision.contacts[i].normal) < 40)
            {
                _grounded = true;
                _shouldNotStick = true;
                return;
            }
        }
        if (!_shouldNotStick)
        {
            if (collision.contacts[0].otherCollider.attachedRigidbody != null)
            {
                if (_connection == null)
                {
                    _connection = Instantiate(_wallConnectionPrefab, collision.contacts[0].point, Quaternion.identity);
                }
                _rigidbody.useGravity = false;
                _stickContactPoint = transform.position;
            } else
            {
                if (_connection == null)
                {
                    _connection = Instantiate(_wallConnectionPrefab, collision.contacts[0].point, Quaternion.identity);
                }
                _rigidbody.useGravity = false;
                _stickContactPoint = transform.position;
            }
            _wallRun = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        _rigidbody.freezeRotation = true;
        _flipManager.IsFlipping = false;
        _touchsThng = true;
        _jumpDirection = Vector3.up;
        normal = collision.contacts[0].normal;
        if (Vector3.Angle(normal, Vector3.up) < 40)
        {
            _grounded = true;
            _shouldNotStick = true;
            if (_connection != null)
            {
                _connection.Disconnect();
                _connection = null;
            }
        }
        if (!_shouldNotStick)
        {
            _timer += Time.deltaTime;
            _jumpDirection = collision.contacts[0].normal * 1.2f;
            if (Input.GetKey(KeyCode.W))
            {
                _jumpDirection = _jumpDirection + Vector3.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                _jumpDirection = _jumpDirection - Vector3.up;
            }
            if (_timer < 2f)
            {
                _constantForce.force = Vector3.zero;
                _phMaterial.frictionCombine = PhysicMaterialCombine.Maximum;
                _rigidbody.useGravity = false;
                _constantForce.force = -_jumpDirection * 3;
                /*if (Input.GetAxis("Vertical") == 0f)
                {
                    _phMaterial.staticFriction = 1f;
                    _phMaterial.dynamicFriction = 1f;
                    _phMaterial.frictionCombine = PhysicMaterialCombine.Maximum;
                } else
                {
                    _phMaterial.staticFriction = 0f;
                    _phMaterial.dynamicFriction = 0f;
                    _phMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
                }*/
                _phMaterial.staticFriction = 0f;
                _phMaterial.dynamicFriction = 0f;
                _phMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
            }
            else if (_timer < 4f)
            {
                if (_connection != null)
                {
                    _connection.Disconnect();
                    _connection = null;
                }
                _rigidbody.useGravity = true;
                _wallRun = false;
                _phMaterial.staticFriction = 0.1f;
                _phMaterial.dynamicFriction = 0.1f;
                _phMaterial.frictionCombine = PhysicMaterialCombine.Average;
            } else {
                _phMaterial.staticFriction = 0f;
                _phMaterial.dynamicFriction = 0f;
                _phMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
                _constantForce.force = _additionalForce;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        _shouldNotStick = false;
        _rigidbody.useGravity = true;
        _grounded = false;
        _touchsThng = false;
        _wallRun = false;
        _timer = 0f;
        _phMaterial.staticFriction = 0f;
        _phMaterial.dynamicFriction = 0f;
        _phMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        _constantForce.force = _additionalForce;
    }
}
