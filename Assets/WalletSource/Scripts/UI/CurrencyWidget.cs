using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyWidget : MonoBehaviour
{
    [SerializeField] private Button incButton;
    [SerializeField] private Button decButton;
    [SerializeField] private TextMeshProUGUI currencyCount;

    public event Action<int> OnClickIncrementCurrency;

    private void OnEnable()
    {
        incButton.onClick.AddListener(() => OnClickIncrementCurrency?.Invoke(1));
        decButton.onClick.AddListener(() => OnClickIncrementCurrency?.Invoke(-1));
    }

    private void OnDisable()
    {
        incButton.onClick.RemoveAllListeners();
        decButton.onClick.RemoveAllListeners();
    }

    public void SetCurrencyCount(int amount)
    {
        currencyCount.text = amount.ToString();
    }
}
