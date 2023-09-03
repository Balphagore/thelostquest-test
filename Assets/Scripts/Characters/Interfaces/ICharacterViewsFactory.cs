using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TheLostQuestTest.Characters
{
    public interface ICharacterViewsFactory
    {
        void Create(AssetReferenceGameObject characterView, Transform spawnPosition, Action<ICharacterView> callback);
    }
}
