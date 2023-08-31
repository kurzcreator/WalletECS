using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace GameCurrency
{
    public partial class GetCurrencyAmountSystem : SystemBase
    {
        public event Action<Dictionary<int, int>> OnAmountGetRequestDone;
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<UsedCurrencyList>();
        }
        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var entity = SystemAPI.GetSingletonEntity<UsedCurrencyList>();

            foreach (var (getCurrencyEvent, eventEntity) in SystemAPI.Query<RefRW<GetCurrenciesAmountEvent>>().WithEntityAccess())
            {
                Dictionary<int, int> mapCurrencies = new Dictionary<int, int>();
                var currencies = SystemAPI.GetBuffer<CurrencyEntityReferenceBufferElement>(entity);
                 
                for (var i = 0; i < currencies.Length; i++)
                {
                    var currency = currencies[i].Value;
                    var currencyId = SystemAPI.GetComponent<CurrencyId>(currency);
                    var currencyAmount = SystemAPI.GetComponent<CurrencyAmount>(currency);

                    mapCurrencies.Add(currencyId.Id, currencyAmount.Amount);
                }

                OnAmountGetRequestDone?.Invoke(mapCurrencies);
                ecb.DestroyEntity(eventEntity);
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}