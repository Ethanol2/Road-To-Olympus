using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStat : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text text;

    [Space]
    [SerializeField] private bool wholeNumbersOnly = true;

    [Space]
    [SerializeField] private float currentValue = 0f;
    [SerializeField] private float startValue = 0f;

    public float Value => currentValue;

    private bool showSigns = false;
    private string toStringMod;

    private void Start()
    {
        toStringMod = wholeNumbersOnly ? "0" : "0.#";
    }
    public void SetValue(float value, bool showMinusPositive = false, bool toggleActiveBasedOnValue = false)
    {
        showSigns = showMinusPositive;

        if (toggleActiveBasedOnValue)
        {
            this.gameObject.SetActive(!Mathf.Approximately(value, 0f));
        }

        if (!this.gameObject.activeInHierarchy)
        {
            SetValueInstant(value);
            return;
        }

        StopAllCoroutines();
        StartCoroutine(FillValue(value));
    }
    public void SetValueInstant(float value)
    {
        StopAllCoroutines();
        currentValue = value;
        UpdateText();
    }
    private void OnValidate()
    {
        currentValue = startValue;
        if (text) UpdateText();

    }
    private void UpdateText()
    {
        string value = currentValue.ToString(toStringMod);
        
        if (showSigns)
        {
            value = currentValue > 0f ? "+" + value : value;
        }

        text.text = value;
    }
    private IEnumerator FillValue(float target)
    {
        float t = 0f;
        float start = currentValue;

        if (Mathf.Approximately(startValue, target)) { yield break; }

        while (t < 1f)
        {
            t += Time.deltaTime / ProgressTracker.UITime;
            currentValue = Mathf.Lerp(start, target, t);
            UpdateText();

            yield return null;
        }
        currentValue = target;
        UpdateText();
    }
}
