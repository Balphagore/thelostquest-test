using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TheLostQuestTest.Characters
{
    public interface ICharacterController
    {
        void Initialize(
            ICharacterViewsFactory characterViewsFactory,
            ICharacterModelsFactory characterModelsFactory,
            ICharactersSystem characters,
            AssetReferenceGameObject characterView,
            CharacterModelDataModel characterModelData,
            Transform spawnPosition,
            Action callback
            );

        Action DestroyModelAction { get; set; }

        Action DestroyViewAction { get; set; }

        Func<string, float> GetCharacterStatValueFunc { get; set; }

        Func<float, float> TakeDamageFunc { get; set; }

        Action TakeStrikeAction { get; set; }

        Func<CharacterModelDataModel> GetCgaracterModelDataFunc { get; set; }

        Func<int> GetActiveBuffsCountFunc { get; set; }

        Func<List<string>> GetActiveBuffsListFunc { get; set; }

        Action<CharacterBuffData> ActivateBuffAction { get; set; }

        Action<CharacterBuffData> ActivateDebuffAction { get; set; }

        Action<float> HealCharacterAction { get; set; }

        Action UpdateBuffDurationsAction { get; set; }

        void AttackCharacter(ICharacterController targetCharacter);

        float TakeDamage(float damage);

        void KillCharacter();

        void UpdateCharacterModelData(CharacterModelDataModel characterModelData);

        int GetActiveBuffsCount();

        List<string> GetActiveBuffsList();

        void ActivateBuff(CharacterBuffData characterBuff);

        void ActivateDebuff(CharacterBuffData characterDebuff);

        void UpdateBuffDurations();
    }
}
