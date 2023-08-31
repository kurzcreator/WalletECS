using GameCurrency;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct CurrencyInitSystem : ISystem
{
    private EntityArchetype currencyArchetype;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UsedCurrencyList>();
        currencyArchetype = state.EntityManager.CreateArchetype(typeof(CurrencyComponent));
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) 
    {
        state.Enabled = false;

        var currencyTypes = SystemAPI.GetSingleton<UsedCurrencyList>();
        ref var currencyList = ref currencyTypes.CurrencyList.Value.Value;

        var ecb = new EntityCommandBuffer(Allocator.Temp);


        for (int i = 0; i < currencyList.Length; i++)
        {
            var currencyEntity = ecb.CreateEntity(currencyArchetype);
            ecb.SetComponent(currencyEntity, new CurrencyComponent { IdCurrency = currencyList[i], Amount = 56});
        }

        ecb.Playback(state.EntityManager);
    }
}
