using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace TheLostQuestTest.Characters
{
    public abstract class CharacterControllersFactoryBase : ICharacterControllersFactory
    {
        [Inject(Id = "Main")]
        private ICharacterModelsFactory characterModelsFactory;
        [Inject(Id = "Main")]
        private ICharacterViewsFactory characterViewsFactory;
        [Inject]
        private ICharactersSystem characters;

        public virtual ICharacterController Create(AssetReferenceGameObject characterView, CharacterModelDataModel characterModelData, Transform spawnPosition, Action callback)
        {
            CharacterController characterController = new();
            characterController.Initialize(characterViewsFactory, characterModelsFactory, characters, characterView, characterModelData, spawnPosition, callback);
            return characterController;
        }
    }
}
