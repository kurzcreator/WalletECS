using Unity.Entities;

namespace GameCurrency
{
    public struct CurrencyId : IComponentData
    {
        public int Id;
    }

    public struct CurrencyAmount : IComponentData
    {
        public int Amount;
    }

    public struct CurrencyHasChangedTag : IComponentData {}
    public struct CurrencyChangeEvent : IComponentData
    {
        public int Value;
    }

    public struct CurrencyResetEvent : IComponentData {}

    public struct GetCurrenciesAmountEvent : IComponentData {}

    public struct SetCurrenciesAmountEvent : IComponentData {}
}