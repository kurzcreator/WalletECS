using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace GameCurrency
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct WalletInitSystem : ISystem
    {
        private EntityArchetype currencyArchetype;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<UsedCurrencyList>();
            currencyArchetype = state.EntityManager.CreateArchetype(typeof(CurrencyId), typeof(CurrencyAmount));
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            var entity = SystemAPI.GetSingletonEntity<UsedCurrencyList>();
            var currencyTypes = state.EntityManager.GetComponentData<UsedCurrencyList>(entity);
            ref var currencyList = ref currencyTypes.CurrencyList.Value.Value;

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var currencyBuffer = ecb.AddBuffer<CurrencyEntityReferenceBufferElement>(entity);

            for (int i = 0; i < currencyList.Length; i++)
            {
                var currencyEntity = ecb.CreateEntity(currencyArchetype);
                ecb.SetComponent(currencyEntity, new CurrencyAmount { Amount = 0 });
                ecb.SetComponent(currencyEntity, new CurrencyId { Id = currencyList[i] });
                currencyBuffer.Add(currencyEntity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}