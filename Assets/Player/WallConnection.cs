using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallConnection : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigitbody;
    [SerializeField] private SpringJoint _joint;
    private Transform _playerPosition;

    private void Awake()
    {
        _playerPosition = FindObjectOfType<Movement>().transform;
        _joint.connectedBody = _playerPosition.GetComponent<Rigidbody>();
        //_joint.maxDistance = Vector3.Distance(_playerPosition.position, transform.position) - 0.2f;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, _playerPosition.position.y, transform.position.z);
    }

    public void Disconnect()
    {
        Destroy(gameObject);
    }
}
