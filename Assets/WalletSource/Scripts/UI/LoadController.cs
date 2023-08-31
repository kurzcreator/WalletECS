using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace GameCurrency
{
    public class LoadController : MonoBehaviour
    {
        public LoaderType LoaderType;

        [SerializeField] private Button loadButton;
        [SerializeField] private Button saveButton;

        private ILoader loader;

        private EntityManager entityManager;
        private GetCurrencyAmountSystem getCurrencyAmountSystem;
        private SetCurrencyAmountSystem setCurrenciesAmountSystem;

        private void Awake()
        {
            switch(LoaderType)
            {
                case LoaderType.PlayerPrefs:
                    loader = new PlayerPrefsLoader();
                    break;

                case LoaderType.File:
                    loader = new FileLoader();
                    break;
            }
        }

        private void Start()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        private void OnEnable()
        {
            loadButton.onClick.AddListener(() => LoadCurrenciesAmount());
            saveButton.onClick.AddListener(() => OnClickSaveButton());
            getCurrencyAmountSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GetCurrencyAmountSystem>();
            getCurrencyAmountSystem.OnAmountGetRequestDone += SaveCurrenciesAmount;
            setCurrenciesAmountSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SetCurrencyAmountSystem>();
        }

        private void OnDisable()
        {
            loadButton.onClick?.RemoveAllListeners();
            saveButton.onClick?.RemoveAllListeners();
        }

        private void OnClickSaveButton()
        {
            var currencySaveEventEntity = entityManager.CreateEntity();
            entityManager.AddComponentData(currencySaveEventEntity, new GetCurrenciesAmountEvent());
        }

        private void SaveCurrenciesAmount(Dictionary<int, int> hashMap)
        {
            Dictionary<CurrencyType, int> currenciesDictionary = hashMap.ToDictionary(c => (CurrencyType)c.Key, c => c.Value);
        }

        private void LoadCurrenciesAmount()
        {
            void LoadingDoneCallback(Dictionary<CurrencyType, int> currenciesDictionary)
            {
                if (currenciesDictionary == null)
                {
                    Debug.LogError("Currencies loading error");
                    return;
                }

                NativeHashMap<int, int> currencyHashMap = new NativeHashMap<int, int>();

                foreach (var item in currenciesDictionary)
                {
                    currencyHashMap.Add((int)item.Key, item.Value);
                }
                
                setCurrenciesAmountSystem.currenciesHashMap = currencyHashMap;
                var currencyLoadEventEntity = entityManager.CreateEntity();
                entityManager.AddComponentData  (currencyLoadEventEntity, new SetCurrenciesAmountEvent());
            }

            loader.Load(LoadingDoneCallback);
        }
    }

    public enum LoaderType
    {
        PlayerPrefs,
        File
    }
}