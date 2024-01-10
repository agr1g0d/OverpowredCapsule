using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pear : Enemy
{
    [SerializeField] EnemyHealthBar _healthBar;
    override public void Die()
    {
        HP = MaxHP;
        _healthBar.UpdateHPValue();
    }
}
