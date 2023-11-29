using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Enemy _enemyInfo;

    private void Start()
    {
        _slider.minValue = 0f;
        UpdateHPValue();
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }

    public void UpdateHPValue()
    {
        _slider.maxValue = _enemyInfo.MaxHP;
        StartCoroutine(LerpSliderValue());
    }

    private IEnumerator LerpSliderValue()
    {
        for (float value = _slider.value; value != _enemyInfo.HP; value = Mathf.Lerp(value, _enemyInfo.HP, Time.deltaTime * 5f))
        {
            _slider.value = value;
            yield return null;
        }
    }
}
