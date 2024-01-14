using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public GameObject PreviewPrefab;
    public GameObject InventoryIconPrefab;
    public List<Property> properties;
    public string Name;
    public string Description;
    public TypeItem TypeItem;
    public bool Hold = false;

    [SerializeField] SurroundingSphere _surroundingSpherePrefab;

    private void Awake()
    {
        Player player = FindObjectOfType<Player>().GetComponent<Player>();
        if (player != null)
        {
            player.Items.Add(this);
        }
    }
    
    public void SetSurroundingSphere(bool set)
    {
        _surroundingSpherePrefab.gameObject.SetActive(set);
    }

    protected virtual void Update()
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
}

public enum TypeItem
{
    weapon,
    consumable,
    potion,
    resource
}