using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountHP : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _text;
    [SerializeField] private Player _playerInfo;

    private void Start()
    {
        _slider.minValue = 0f;
        UpdateHPValue();
    }

    public void UpdateHPValue()
    {
        _slider.maxValue = _playerInfo.MaxPlayerHP;
        StartCoroutine(LerpSliderValue());
        _text.text = _playerInfo.PlayerHP.ToString() + "/" + _playerInfo.MaxPlayerHP.ToString();
    }

    private IEnumerator LerpSliderValue()
    {
        for (float value = _slider.value; value != _playerInfo.PlayerHP; value = Mathf.Lerp(value, _playerInfo.PlayerHP, Time.deltaTime * 2f))
        {
            _slider.value = value;
            yield return null;
        }
    }
}
