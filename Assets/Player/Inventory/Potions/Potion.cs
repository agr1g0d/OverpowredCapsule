using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : InventoryItem
{
    public void PickUp(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
