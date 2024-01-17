using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public GameObject PreviewPrefab;
    public Icon InventoryIconPrefab;
    [NonSerialized] public Icon InventoryIcon;
    public List<Property> Properties;
    public List<Button> Buttons;
    public string Name;
    public string Description;
    public TypeItem TypeItem;
    public bool Hold = false;

    [SerializeField] SurroundingSphere _surroundingSpherePrefab;

    protected virtual void Start()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.Items.Add(this);
            print(gameObject);
        }
    }
    
    public void SetSurroundingSphere(bool set)
    {
        _surroundingSpherePrefab.gameObject.SetActive(set);
    }

    protected virtual void Update()
    {
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!Hold)
        {
            if (_surroundingSpherePrefab.isActiveAndEnabled)
            {
                if (_surroundingSpherePrefab.Rigidbody.velocity != Vector3.zero)
                {
                    _surroundingSpherePrefab.Rigidbody.isKinematic = true;
                    _surroundingSpherePrefab.Collider.isTrigger = true;
                }
            }
        }
    }

    public void PickUp(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}

public enum TypeItem
{
    weapon,
    consumable,
    potion,
    resource
}