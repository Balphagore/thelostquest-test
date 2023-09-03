using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TheLostQuestTest.Characters
{
    public abstract class CharacterControllerBase : ICharacterController
    {
        private ICharactersSystem characters;

        public Action DestroyModelAction { get; set; }
        public Action DestroyViewAction { get; set; }
        public Func<string, float> GetCharacterStatValueFunc { get; set; }
        public Func<float, float> TakeDamageFunc { get; set; }
        public Action TakeStrikeAction { get; set; }
        public Func<CharacterModelDataModel> GetCgaracterModelDataFunc { get; set; }
        public Func<int> GetActiveBuffsCountFunc { get; set; }
        public Func<List<string>> GetActiveBuffsListFunc { get; set; }
        public Action<CharacterBuffData> ActivateBuffAction { get; set; }
        public Action<CharacterBuffData> ActivateDebuffAction { get; set; }
        public Action<float> HealCharacterAction { get; set; }
        public Action UpdateBuffDurationsAction { get; set; }

        public virtual void Initialize(
                ICharacterViewsFactory characterViewsFactory,
                ICharacterModelsFactory characterModelsFactory,
                ICharactersSystem characters,
                AssetReferenceGameObject characterView,
                CharacterModelDataModel characterModelData,
                Transform spawnPosition,
                Action callback
            )
        {
            this.characters = characters;

            characterViewsFactory.Create(characterView, spawnPosition, value => OnCallback(value, callback));
            characterModelsFactory.Create(this, characterModelData);

            characters.DestroyCharactersAction += OnDestroyCharactersAction;
        }

        public virtual void AttackCharacter(ICharacterController targetCharacter)
        {
            float vampirismValue = GetCharacterStatValueFunc.Invoke("Vampirism");
            float damage = GetCharacterStatValueFunc.Invoke("Attack");
            targetCharacter.TakeDamage(damage);
            float healedDamage = damage * (vampirismValue / 100);
            HealCharacterAction?.Invoke(healedDamage);
        }

        public virtual float TakeDamage(float damage)
        {
            float takenDamage = 0;
            if (damage > 0)
            {
                takenDamage = TakeDamageFunc.Invoke(damage);
                TakeStrikeAction?.Invoke();
            }
            return takenDamage;
        }

        public virtual void KillCharacter()
        {
            characters.StartNewGame();
        }

        public virtual void UpdateCharacterModelData(CharacterModelDataModel characterModelData)
        {
            characters.UpdateCharacterData(characterModelData);
        }

        public virtual void ActivateBuff(CharacterBuffData characterBuff)
        {
            ActivateBuffAction?.Invoke(characterBuff);
        }

        public virtual void ActivateDebuff(CharacterBuffData characterDebuff)
        {
            ActivateDebuffAction?.Invoke(characterDebuff);
        }

        public virtual int GetActiveBuffsCount()
        {
            return GetActiveBuffsCountFunc.Invoke();
        }

        public virtual List<string> GetActiveBuffsList()
        {
            return GetActiveBuffsListFunc?.Invoke();
        }

        public virtual void UpdateBuffDurations()
        {
            UpdateBuffDurationsAction?.Invoke();
        }

        protected virtual void OnCallback(ICharacterView characterView, Action callback)
        {
            characterView.Initialize(this);
            callback?.Invoke();
        }

        protected virtual void OnDestroyCharactersAction()
        {
            DestroyModelAction?.Invoke();
            DestroyViewAction?.Invoke();
            characters.DestroyCharactersAction -= OnDestroyCharactersAction;
            characters = null;
        }
    }
}
