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
    private EntityArchetype currencyChangeEvent;
    private EntityArchetype currencyResetEvent;

    private SendUIWalletChangesSystem sendChangesSystem;

    private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        currencyChangeEvent = entityManager.CreateArchetype(typeof(CurrencyId),typeof(CurrencyChangeEvent));
        currencyResetEvent = entityManager.CreateArchetype(typeof(CurrencyId), typeof(CurrencyResetEvent));
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
        Debug.Log($"Increment {value} {currencyType}");
        var currencyChangeEventEntity = entityManager.CreateEntity(currencyChangeEvent);
        entityManager.SetComponentData(currencyChangeEventEntity, new CurrencyId { Id = (int)currencyType });
        entityManager.SetComponentData(currencyChangeEventEntity, new CurrencyChangeEvent { Value = value });
    }

    private void ResetCurrency(CurrencyType currencyType)
    {
        Debug.Log($"Reset {currencyType}");
        var currencyResetEventEntity = entityManager.CreateEntity(currencyResetEvent);
        entityManager.SetComponentData(currencyResetEventEntity, new CurrencyId { Id = (int)currencyType });
        entityManager.SetComponentData(currencyResetEventEntity, new CurrencyResetEvent());
    }

    private void SetWidgetAmountCurrency(CurrencyType currencyType, int amount)
    {
        var widget = currencyWidgets.First(w => w.CurrencyType == currencyType);
        widget.SetCurrencyAmount(amount);
    }
}
