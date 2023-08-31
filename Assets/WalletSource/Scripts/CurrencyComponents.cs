using Unity.Entities;

namespace GameCurrency
{
    public struct CurrencyComponent : IComponentData
    {
        public int IdCurrency;
        public int Amount;
    }
}