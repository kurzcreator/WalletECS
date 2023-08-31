using Unity.Entities;

namespace GameCurrency
{
    public struct UsedCurrencyList : IComponentData
    {
        public BlobAssetReference<CurrencyListBlob> CurrencyList;
    }

    public struct CurrencyListBlob
    {
        public BlobArray<int> Value;
    }

    [InternalBufferCapacity(8)]
    public struct CurrencyEntityReferenceBufferElement : IBufferElementData
    {
        public Entity Value;

        public static implicit operator CurrencyEntityReferenceBufferElement(Entity value)
        {
            return new CurrencyEntityReferenceBufferElement { Value = value };
        }
    }
}