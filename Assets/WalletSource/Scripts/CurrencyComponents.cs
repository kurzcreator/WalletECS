using Unity.Entities;

namespace GameCurrency
{
    public struct Gold : IComponentData
    {
        public int Amount;
    }

    public struct Crystals : IComponentData
    {
        public int Amount;
    }
}