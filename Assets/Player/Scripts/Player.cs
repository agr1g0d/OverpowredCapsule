using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public float PlayerHP;
    public float MaxPlayerHP;
    public float PickUpDistance = 2f;
    public List<InventoryItem> Items;

    [SerializeField] DamageScreen _damageScreen;
    [SerializeField] ParticleSystem _electroEffect;
    [SerializeField] CountHP _countHP;
    [SerializeField] InventoryManager _inventoryManager;
    [SerializeField] LineRenderer _itemPointerLine;

    private PlayerTakesDamageUnityEvent _onTakeDamagePlayer;
    private bool _invulnerable;
    private bool _needTurnOnItemPointer;
    private bool _turnOnItemPointer;

    private void Start()
    {
        _onTakeDamagePlayer = new PlayerTakesDamageUnityEvent();
        _onTakeDamagePlayer.AddListener(damage);
    }

    private void Update()
    {
        if (Items.Count != 0)
        {
            InventoryItem nearestItem = Items[0];
            for (int i = 0; i < Items.Count; i++)
            {
                if (Vector3.Distance(transform.position, Items[i].transform.position) < PickUpDistance)
                {
                    _needTurnOnItemPointer = true;
                    if (!_turnOnItemPointer)
                    {
                        _needTurnOnItemPointer = true;
                        _itemPointerLine.gameObject.SetActive(true);
                    }
                    if (Vector3.Distance(transform.position, Items[i].transform.position) < Vector3.Distance(transform.position, nearestItem.transform.position))
                    {
                        nearestItem = Items[i];
                    }
                }
            }

            if (Vector3.Distance(transform.position, nearestItem.transform.position) < PickUpDistance)
            {
                if (Input.GetKeyDown(KeyCode.E) && nearestItem != null)
                {
                    _inventoryManager.OnItemPickedEvent.Invoke(nearestItem);
                }
                _itemPointerLine.SetPosition(0, transform.position);
                _itemPointerLine.SetPosition(1, nearestItem.transform.position);
            }

            if (!_needTurnOnItemPointer)
            {
                _turnOnItemPointer = false;
                _itemPointerLine.gameObject.SetActive(false);
            }

            _needTurnOnItemPointer = false;
        } else if (Items.Count == 0)
        {
            _turnOnItemPointer = false;
            _itemPointerLine.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent(out InventoryItem item) ) 
        {
            if (item == null)
            {
                print(null);
                return;
            }
            _inventoryManager.OnItemPickedEvent.Invoke(item);
        }
    }

    private void damage(float cooldown)
    {
        _damageScreen.StartBlink();
        _countHP.UpdateHPValue();
        StartCoroutine(InvulnerableForNSeconds(cooldown));
    }

    public void Die()
    {
        Debug.Log("Looooseeer");
    }

    public void TakeDamage(EnemyDamage damage, float cooldown)
    {
        if (!_invulnerable)
        {
            PlayerHP -= damage.Damage;
            if (damage.Type == TypeDamage.electrical)
            {
                _electroEffect.Play();
            }
            _onTakeDamagePlayer.Invoke(cooldown);
            if (PlayerHP <= 0)
            {
                Die();
            }
        }
    } 

    private IEnumerator InvulnerableForNSeconds(float n)
    {
        _invulnerable = true;
        yield return new WaitForSeconds(n);
        _invulnerable = false;
    }
}

public class PlayerTakesDamageUnityEvent : UnityEvent<float>
{
}
