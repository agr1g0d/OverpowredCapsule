using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool _isWeaponCell;
    private InventoryManager _inventoryManager;

    private void Start()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print(gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        if (!_isWeaponCell)
        {
            transform.localScale = Vector3.one;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isWeaponCell)
        {
            transform.localScale = Vector3.one * _inventoryManager.ChangedCellScale;
        }
    }
}
