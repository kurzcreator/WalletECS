using GameCurrency;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

public class WalletUIController : MonoBehaviour
{
    [SerializeField] private CurrencyWidget[] currencyWidgets;

    private EntityManager entityManager;

    private SendUIWalletChangesSystem sendChangesSystem;

    private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void OnEnable()
    {
        foreach (var currencyWidget in currencyWidgets)
        {
            currencyWidget.OnClickIncrementCurrencyCount += IncrementCurrency;
            currencyWidget.OnClickResetCurrencyCount += ResetCurrency;
        }

        sendChangesSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SendUIWalletChangesSystem>();
        sendChangesSystem.OnAmountCurrencyChanged += SetWidgetAmountCurrency;
    }

    private void OnDisable()
    {
        foreach (var currencyWidget in currencyWidgets)
        {
            currencyWidget.OnClickIncrementCurrencyCount += IncrementCurrency;
            currencyWidget.OnClickResetCurrencyCount += ResetCurrency;
        }

        if (sendChangesSystem == null)
        {
            return;
        }

        sendChangesSystem.OnAmountCurrencyChanged -= SetWidgetAmountCurrency;
    }

    private void IncrementCurrency(int value, CurrencyType currencyType)
    {
        var currencyChangeEventEntity = entityManager.CreateEntity();
        entityManager.AddComponentData(currencyChangeEventEntity, new CurrencyId { Id = (int)currencyType });
        entityManager.AddComponentData(currencyChangeEventEntity, new CurrencyChangeEvent { Value = value });
    }

    private void ResetCurrency(CurrencyType currencyType)
    {
        var currencyResetEventEntity = entityManager.CreateEntity();
        entityManager.AddComponentData(currencyResetEventEntity, new CurrencyId { Id = (int)currencyType });
        entityManager.AddComponentData(currencyResetEventEntity, new CurrencyResetEvent());
    }

    private void SetWidgetAmountCurrency(CurrencyType currencyType, int amount)
    {
        var widget = currencyWidgets.First(w => w.CurrencyType == currencyType);
        widget.SetCurrencyAmount(amount);
    }
}
