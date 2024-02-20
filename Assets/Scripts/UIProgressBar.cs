using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
    [SerializeField] private Image fillableBar;
    [SerializeField] private Color barColour = Color.red;

    [Space]
    [SerializeField] private TMP_Text text;

    [Space]
    [SerializeField] private float currentValue;
    [SerializeField] private float startValue = 0.5f;
    [SerializeField] private float maxValue = 1f;

    [Space]
    [SerializeField] private bool showText = true;
    [SerializeField] private bool showTextAsPercentage = true;

    private void Awake()
    {
        OnValidate();
    }
    private void OnValidate()
    {
        if (fillableBar)
        {
            currentValue = startValue;
            fillableBar.fillAmount = currentValue / maxValue;
            fillableBar.color = barColour;
        }
        if (text)
        {
            UpdateText();
        }
    }
    public void SetBarAmount(float value)
    {
        StopAllCoroutines();
        StartCoroutine(FillBar(value / maxValue));
    }
    public void SetBarInstant(float value)
    {
        StopAllCoroutines();
        fillableBar.fillAmount = value;
        currentValue = value;
        UpdateText();
    }

    private IEnumerator FillBar(float target)
    {
        float t = 0f;
        float start = fillableBar.fillAmount;

        if (Mathf.Approximately(start, target)) { yield break; }

        while (t < 1f)
        {
            t += Time.deltaTime / ProgressTracker.UITime;
            fillableBar.fillAmount = currentValue = Mathf.Lerp(start, target, t);
            UpdateText();

            yield return null;
        }

        fillableBar.fillAmount = currentValue = target;
    }
    private void UpdateText()
    {
        if (showText)
        {
            if (showTextAsPercentage)
            {
                int percent = Mathf.RoundToInt((currentValue / maxValue) * 100f);
                text.text = $"{percent} %";
            }
            else
            {
                text.text = $"{Mathf.RoundToInt(currentValue)} / {Mathf.RoundToInt(maxValue)}";
            }
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }
}
