using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GameCurrency
{
    public class CurrencyMono : MonoBehaviour
    {
        public class CurrencyBacker : Baker<CurrencyMono>
        {
            public override void Bake(CurrencyMono authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.None);

                AddComponent(entity, new Crystals
                {
                    Amount = 12
                });

                AddComponent(entity, new Gold
                {
                    Amount = 22
                });
            }
        }
    }
}