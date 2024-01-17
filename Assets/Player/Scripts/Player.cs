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
    private bool _turnOnItemPointer;

    private void Start()
    {
        _onTakeDamagePlayer = new PlayerTakesDamageUnityEvent();
        _onTakeDamagePlayer.AddListener(damage);
    }

    private void Update()
    {
        Vector3 nearestItemPosition = Vector3.one * (PickUpDistance + 1);
        for (int i = 0;  i < Items.Count; i++)
        {
            if (Vector3.Distance(transform.position, Items[i].transform.position) < PickUpDistance)
            {
                if (Vector3.Distance(transform.position, Items[i].transform.position) < Vector3.Distance(transform.position, nearestItemPosition))
                {
                    nearestItemPosition = Items[i].transform.position;
                }
                if (!_turnOnItemPointer)
                {
                    _turnOnItemPointer = true;
                    _itemPointerLine.gameObject.SetActive(true);
                }
                
                if (Input.GetKey(KeyCode.E) && Items[i] != null)
                {
                    _inventoryManager.OnItemPickedEvent.Invoke(Items[i]);
                }
            } else
            {
                if (_turnOnItemPointer)
                {
                    _turnOnItemPointer = false;
                    _itemPointerLine.gameObject.SetActive(false);
                }
            }
        }
        if (Items.Count == 0)
        {
            _turnOnItemPointer = false;
            _itemPointerLine.gameObject.SetActive(false);
        } else
        {
            _itemPointerLine.SetPosition(0, transform.position);
            _itemPointerLine.SetPosition(1, nearestItemPosition);
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
