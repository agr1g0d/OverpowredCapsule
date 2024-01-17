using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Icon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool _isWeaponCell;
    private InventoryManager _inventoryManager;
    private Preview _preview;
    private InventoryItem _item;

    private void Start()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();
        _preview = FindObjectOfType<Preview>();
    }

    public void SetItem(InventoryItem item)
    {
        _item = item;
        print(_item);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _preview.ChangeObject(_item);
        
    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        transform.localScale /= _inventoryManager.ChangedCellScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale *= _inventoryManager.ChangedCellScale;
    }
}
