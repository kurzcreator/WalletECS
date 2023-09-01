using System.Diagnostics;
using Unity.Collections;
using Unity.Entities;

namespace GameCurrency
{
    public partial class SetCurrencyAmountSystem : SystemBase
    {
        public NativeHashMap<int, int> currenciesHashMap;

        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<UsedCurrencyList>();
        }
        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var entity = SystemAPI.GetSingletonEntity<UsedCurrencyList>();

            foreach (var (setCurrencyEvent, eventEntity) in SystemAPI.Query<RefRW<SetCurrenciesAmountEvent>>().WithEntityAccess())
            {
                ecb.DestroyEntity(eventEntity);
                
                var currencies = SystemAPI.GetBuffer<CurrencyEntityReferenceBufferElement>(entity);
                
                if (currenciesHashMap.IsEmpty)
                {
                    break;
                }

                foreach (var item in currenciesHashMap)
                {
                    for (int i = 0; i < currencies.Length; i++)
                    {
                        var currency = currencies[i].Value;
                        var currencyID = SystemAPI.GetComponent<CurrencyId>(currency);

                        if (currencyID.Id == item.Key)
                        {
                            SystemAPI.SetComponent(currency, new CurrencyAmount { Amount = item.Value });
                            ecb.AddComponent(currency, new CurrencyHasChangedTag());
                        }
                    }
                }

            }

            ecb.Playback(EntityManager);
            ecb.Dispose();

            if (currenciesHashMap.IsCreated)
            {
                currenciesHashMap.Dispose();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            currenciesHashMap.Dispose();
        }
    }
}