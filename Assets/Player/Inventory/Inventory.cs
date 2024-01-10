using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public List<Weapon> Weapons = new List<Weapon>();
    public List<Consumable> Consumables = new List<Consumable>();
    public List<Potion> Potions = new List<Potion>();
    public List<GameResource> Resources = new List<GameResource>();
    public Preview Preview;
    public ItemEvent OnItemPickedEvent = new ItemEvent();

    [SerializeField] RectTransform _weaponsParent;
    [SerializeField] RectTransform _consumablesParent;
    [SerializeField] RectTransform _potionsParent;
    [SerializeField] RectTransform _resourcesParent;
    [SerializeField] WeaponManager _weaponManager;

    private void Start()
    {
        /*UpdateItems(TypeItem.weapon);
        UpdateItems(TypeItem.consumable);
        UpdateItems(TypeItem.potion);
        UpdateItems(TypeItem.resource);*/
        OnItemPickedEvent.AddListener(ItemPicked);

    }

    void ItemPicked(InventoryItem item)
    {
        AddItem(item);
        _weaponManager.UpdateWeapons();
        item.Hold = true;
        UpdateItems(item.TypeItem);
    }


    public void AddItem(InventoryItem item)
    {
        if (item.gameObject.TryGetComponent(out Weapon w))
        {
            if (Weapons.Count < 3)
            {
                Weapons.Add(w);
            }
        }
        else if (item.gameObject.TryGetComponent(out Consumable c))
        {
            if (Weapons.Count < 3)
            {
                Consumables.Add(c);
            }
        }
        else if (item.gameObject.TryGetComponent(out Potion p))
        {
            if (Weapons.Count < 3)
            {
                Potions.Add(p);
            }
        }
        else if (item.gameObject.TryGetComponent(out GameResource r))
        {
            if (Weapons.Count < 3)
            {
                Resources.Add(r);
            }
        }
    }

    public void UpdateItems(TypeItem type)
    {
        switch (type)
        {
            case TypeItem.weapon:
                for (int i = 0; i < _weaponsParent.childCount; i++)
                {
                    Destroy(_weaponsParent.GetChild(i));
                }
                foreach (var item in Weapons)
                {
                    Instantiate(item.InventoryIconPrefab, _weaponsParent);
                }
                break;
            case TypeItem.consumable:
                for (int i = 0; i < _consumablesParent.childCount; i++)
                {
                    Destroy(_consumablesParent.GetChild(i));
                }
                foreach (var item in Consumables)
                {
                    Instantiate(item.InventoryIconPrefab, _consumablesParent);
                }
                break;
            case TypeItem.potion:
                for (int i = 0; i < _potionsParent.childCount; i++)
                {
                    Destroy(_potionsParent.GetChild(i));
                }
                foreach (var item in Potions)
                {
                    Instantiate(item.InventoryIconPrefab, _potionsParent);
                }
                break;
            case TypeItem.resource:
                for (int i = 0; i < _resourcesParent.childCount; i++)
                {
                    Destroy(_resourcesParent.GetChild(i));
                }
                foreach (var item in Resources)
                {
                    Instantiate(item.InventoryIconPrefab, _resourcesParent);
                }
                break;
        }
    }

}

public class ItemEvent : UnityEvent<InventoryItem> { }

