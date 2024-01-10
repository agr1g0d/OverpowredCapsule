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

    [SerializeField] GameObject SurroundingSpherePrefab;

    private void Awake()
    {
        Player player = FindObjectOfType<Player>().GetComponent<Player>();
        if (player != null)
        {
            player.Items.Add(this);
        }
    }

    private void Update()
    {
        SurroundingSpherePrefab.SetActive(!Hold);
    }
}

public enum TypeItem
{
    weapon,
    consumable,
    potion,
    resource
}