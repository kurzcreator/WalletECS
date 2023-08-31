using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GameCurrency
{
    public class CurrencyAutoring : MonoBehaviour
    {
        public CurrencyType[] currencyTypes;
        public class CurrencyBacker : Baker<CurrencyAutoring>
        {
            public override void Bake(CurrencyAutoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.None);

                var blobBuilder = new BlobBuilder(Allocator.Temp);
                ref var currencyList = ref blobBuilder.ConstructRoot<CurrencyListBlob>();
                var arrayBuilder = blobBuilder.Allocate(ref currencyList.Value, authoring.currencyTypes.Length);

                for (int i = 0; i < authoring.currencyTypes.Length; i++)
                {
                    arrayBuilder[i] = (int)authoring.currencyTypes[i];
                }

                var blobAssets = blobBuilder.CreateBlobAssetReference<CurrencyListBlob>(Allocator.Persistent);

                AddComponent(entity, new UsedCurrencyList
                {
                    CurrencyList = blobAssets
                });

                blobBuilder.Dispose();
            }
        }
    }
}