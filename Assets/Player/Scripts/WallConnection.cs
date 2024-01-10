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
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, _playerPosition.position.y, 0);
    }

    public void Move(float distanse)
    {
        transform.Translate(0, distanse, 0);
    }

    public void Disconnect()
    {
        Destroy(gameObject);
    }
}
