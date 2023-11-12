using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageScreen : MonoBehaviour
{
    [SerializeField] private Image image;
    private IEnumerator ScreenBlink()
    {
        image.enabled = true;
        for (float t = 1; t >= 0; t -= Time.deltaTime)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, t);
            yield return null;
        }
        image.enabled = false;
    }
    public void StartBlink()
    {
        StartCoroutine(ScreenBlink());
    }
}
