using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCurrency
{
    public class CurrencyWidget : MonoBehaviour
    {
        public CurrencyType CurrencyType;
        [SerializeField] private Button incButton;
        [SerializeField] private Button decButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private TextMeshProUGUI currencyCount;

        public event Action<int, CurrencyType> OnClickIncrementCurrencyCount;
        public event Action<CurrencyType> OnClickResetCurrencyCount;

        private void OnEnable()
        {
            incButton.onClick.AddListener(() => OnClickIncrementCurrencyCount?.Invoke(1, CurrencyType));
            decButton.onClick.AddListener(() => OnClickIncrementCurrencyCount?.Invoke(-1, CurrencyType));
            resetButton.onClick.AddListener(() => OnClickResetCurrencyCount?.Invoke(CurrencyType));
        }

        private void OnDisable()
        {
            incButton.onClick.RemoveAllListeners();
            decButton.onClick.RemoveAllListeners();
            resetButton.onClick.RemoveAllListeners();
        }

        public void SetCurrencyAmount(int amount)
        {
            currencyCount.text = amount.ToString();
        }
    }
}