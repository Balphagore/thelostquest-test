using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TheLostQuestTest.Characters
{
    public interface ICharacterControllersFactory
    {
        ICharacterController Create(AssetReferenceGameObject characterView, CharacterModelDataModel characterModelData, Transform spawnPosition, Action callback);
    }
}
