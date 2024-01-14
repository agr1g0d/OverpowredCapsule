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

    private PlayerTakesDamageUnityEvent _onTakeDamagePlayer;
    private bool _invulnerable;

    private void Start()
    {
        _onTakeDamagePlayer = new PlayerTakesDamageUnityEvent();
        _onTakeDamagePlayer.AddListener(damage);
    }

    private void Update()
    {
        for (int i = 0;  i < Items.Count; i++)
        {
            if (Items[i].enabled)
            {
                if (Vector3.Distance(transform.position, Items[i].transform.position) < PickUpDistance)
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        _inventoryManager.OnItemPickedEvent.Invoke(Items[i]);
                    }
                }
            }
        }


        foreach (InventoryItem item in Items)
        {
            
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
