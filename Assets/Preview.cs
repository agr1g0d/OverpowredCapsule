using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Preview : MonoBehaviour
{
    public GameObject ObjectPrefab;
    public Vector3 Rotation;

    [SerializeField] float _rotationPerSecond;
    [SerializeField] Text _textName;
    [SerializeField] Text _textDescription;
    [SerializeField] RectTransform _buttonsParent;
    [SerializeField] RectTransform _propertiesParent;

    private GameObject _object;

    private void Start()
    {
        _object = Instantiate(ObjectPrefab, transform);
    }

    void Update()
    {
        ObjectPrefab.transform.eulerAngles += _rotationPerSecond * Time.deltaTime * Vector3.up;
    }

    public void ChangeObject(InventoryItem item) 
    {
        _object = Instantiate(item.PreviewPrefab, transform);

        foreach (Transform child in _buttonsParent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < item.Buttons.Count; i++)
        {
            Instantiate(item.Buttons[i], _buttonsParent);
        }

        foreach (Transform child in _propertiesParent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < item.Properties.Count; i++)
        {
            Instantiate(item.Properties[i], _propertiesParent);
        }

        _textName.text = item.Name;
        _textDescription.text = item.Description;
    }
}
