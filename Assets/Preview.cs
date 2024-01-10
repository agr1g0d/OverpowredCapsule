using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview : MonoBehaviour
{
    public GameObject ObjectPrefab;
    public Vector3 Rotation;
    [SerializeField] float _rotationPerSecond;

    void Update()
    {
        ObjectPrefab.transform.eulerAngles += _rotationPerSecond * Time.deltaTime * Vector3.up;
    }
}
