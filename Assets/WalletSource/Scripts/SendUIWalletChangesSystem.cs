using Unity.Entities;
using Unity.Collections;
using System;

namespace GameCurrency
{
    public partial class SendUIWalletChangesSystem : SystemBase
    {
        public event Action<CurrencyType, int> OnAmountCurrencyChanged;

        protected override void OnCreate()
        {
            base.OnCreate();
        }

        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (changed, currencyID, amount, entity) in SystemAPI.Query<RefRW<CurrencyHasChanged>, CurrencyId, CurrencyAmount>().WithEntityAccess())
            {
                OnAmountCurrencyChanged?.Invoke((CurrencyType)currencyID.Id, amount.Amount);

                ecb.RemoveComponent<CurrencyHasChanged>(entity);
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}