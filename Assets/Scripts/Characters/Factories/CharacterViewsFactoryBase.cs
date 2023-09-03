using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TheLostQuestTest.Characters
{
    public abstract class CharacterViewsFactoryBase : ICharacterViewsFactory
    {
        public virtual void Create(AssetReferenceGameObject characterView, Transform spawnPosition, Action<ICharacterView> callback)
        {
            characterView.InstantiateAsync(spawnPosition).Completed += operation => OnCompleted(operation, callback);
        }

        public virtual void OnCompleted(AsyncOperationHandle<GameObject> asyncOperation, Action<ICharacterView> callback)
        {
            callback?.Invoke(asyncOperation.Result.GetComponent<ICharacterView>());
        }
    }
}
