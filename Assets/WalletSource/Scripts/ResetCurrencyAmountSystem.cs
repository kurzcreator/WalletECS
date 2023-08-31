using Unity.Entities;
using Unity.Collections;

namespace GameCurrency
{
    public partial class ResetCurrencyAmountSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<UsedCurrencyList>();
        }

        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var entity = SystemAPI.GetSingletonEntity<UsedCurrencyList>();

            foreach (var (eventCurrency, eventCurrencyID, eventEntity) in SystemAPI.Query<RefRO<CurrencyResetEvent>, RefRO<CurrencyId>>().WithEntityAccess())
            {
                var currencies = SystemAPI.GetBuffer<CurrencyEntityReferenceBufferElement>(entity);

                //TODO: need to use hashmap
                for (var i = 0; i < currencies.Length; i++)
                {
                    var currency = currencies[i].Value;
                    var currencyID = SystemAPI.GetComponent<CurrencyId>(currency);

                    if (currencyID.Id == eventCurrencyID.ValueRO.Id)
                    {
                        var amountComponent = new CurrencyAmount { Amount = 0 };
                        ecb.SetComponent(currency, amountComponent);
                        ecb.AddComponent(currency, new CurrencyHasChanged());

                        break;
                    }
                }

                ecb.DestroyEntity(eventEntity);
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}