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
}