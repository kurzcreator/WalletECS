using GameCurrency;
using Unity.Burst;
using Unity.Entities;

public partial struct TestSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var currency in SystemAPI.Query<CurrencyAmount>())
        {
        }
    }
}