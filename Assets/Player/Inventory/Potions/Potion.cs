using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : InventoryItem
{
    [SerializeField] private float _height = 0.2f;
    [SerializeField] private float _friquency = 3;
    [SerializeField] private float _rotation = 0.6f;

    private Vector3 startPoz;

    private void Start()
    {
        startPoz = transform.position;
    }
    void Update()
    {
        transform.position = startPoz + Vector3.up * (Mathf.Sin(Time.time * _friquency) * 0.5f + 0.5f) * _height;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Time.time * _rotation, transform.eulerAngles.z);
    }
}
