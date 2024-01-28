using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public List<Weapon> Weapons = new List<Weapon>();
    public List<Consumable> Consumables = new List<Consumable>();
    public List<Potion> Potions = new List<Potion>();
    public List<GameResource> Resources = new List<GameResource>();
    public Preview Preview;
    public float ChangedCellScale = 0.8f;
    public ItemEvent OnItemPickedEvent = new ItemEvent();

    [SerializeField] private Player _player;
    [SerializeField] Transform _pocket; //parent for all consumables, potions and resources
    [SerializeField] RectTransform _weaponsParent;
    [SerializeField] RectTransform _consumablesParent;
    [SerializeField] RectTransform _potionsParent;
    [SerializeField] RectTransform _resourcesParent;
    [SerializeField] RectTransform _inventoryParent;
    [SerializeField] WeaponManager _weaponManager;

    public bool OpenInventory;

    private void Start()
    {
        foreach (var weapon in Weapons)
        {
            AddIcon(weapon);
        }
        foreach (var consumable in Consumables)
        {
            AddIcon(consumable);
        }
        foreach (var potion in Potions)
        {
            AddIcon(potion);
        }
        foreach (var resource in Resources)
        {
            AddIcon(resource);
        }

        OnItemPickedEvent.AddListener(ItemPicked);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!OpenInventory)
            {
                OpenInventory = true;
                _inventoryParent.gameObject.SetActive(true);
            }
            else
            {
                OpenInventory = false;
                _inventoryParent.gameObject.SetActive(false);
            }
        }
    }

    void ItemPicked(InventoryItem item)
    {
        if (AddItem(item))
        {
            item.Hold = true;
            AddIcon(item);
            item.InventoryIcon.SetItem(item);
            _player.Items.Remove(item);
        }
    }


    public bool AddItem(InventoryItem item)
    {
        if (item.gameObject.TryGetComponent(out Weapon w))
        {
            if (Weapons.Count < 3)
            {
                Weapons.Add(w);
                _weaponManager.AddWeapon(w);
                return true;
            }
        }
        else if (item.gameObject.TryGetComponent(out Consumable c))
        {
            if (Consumables.Count < 8)
            {
                Consumables.Add(c);
                c.PickUp(_pocket, true);
                return true;
            }
        }
        else if (item.gameObject.TryGetComponent(out Potion p))
        {
            if (Potions.Count < 8)
            {
                Potions.Add(p);
                p.PickUp(_pocket, true);
                return true;
            }
        }
        else if (item.gameObject.TryGetComponent(out GameResource r))
        {
            if (Resources.Count < 20)
            {
                Resources.Add(r);
                r.PickUp(_pocket, true);
                return true;
            }
        }
        return false;
    }

    public void AddIcon(InventoryItem item)
    {
        switch (item.TypeItem)
        {
            case TypeItem.weapon:
                item.InventoryIcon = Instantiate(item.InventoryIconPrefab, _weaponsParent);
                break;

            case TypeItem.consumable:
                item.InventoryIcon = Instantiate(item.InventoryIconPrefab, _consumablesParent);
                break;

            case TypeItem.potion:
                item.InventoryIcon = Instantiate(item.InventoryIconPrefab, _potionsParent);
                break;

            case TypeItem.resource:
                item.InventoryIcon = Instantiate(item.InventoryIconPrefab, _resourcesParent);
                break;
        }
    }

    public void RemoveIcon(InventoryItem item, int index) 
    {
        switch (item.TypeItem)
        {
            case TypeItem.weapon:
                Destroy(_weaponsParent.GetChild(index));
                break;

            case TypeItem.consumable:
                Destroy(_consumablesParent.GetChild(index));
                break;

            case TypeItem.potion:
                Destroy(_potionsParent.GetChild(index));
                break;

            case TypeItem.resource:
                Destroy(_resourcesParent.GetChild(index));
                break;
        }
    }



}

public class ItemEvent : UnityEvent<InventoryItem> { }

