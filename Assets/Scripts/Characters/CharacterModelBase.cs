using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheLostQuestTest.Characters
{
    public abstract class CharacterModelBase : ICharacterModel
    {
        private ICharacterController characterController;
        private CharacterModelDataModel characterModelData;

        public virtual void Initialize(ICharacterController characterController, CharacterModelDataModel characterModelData)
        {
            this.characterController = characterController;
            this.characterModelData = characterModelData;
            characterController.GetCharacterStatValueFunc += OnGetCharacterStatValueFunc;
            characterController.TakeDamageFunc += OnTakeDamageAction;
            characterController.GetActiveBuffsCountFunc += OnGetActiveBuffsCountFunc;
            characterController.GetActiveBuffsListFunc += OnGetActiveBuffsListFunc;
            characterController.ActivateBuffAction += OnActivateBuffAction;
            characterController.ActivateDebuffAction += OnActivateDebuffAction;
            characterController.HealCharacterAction += OnHealCharacterAction;
            characterController.UpdateBuffDurationsAction += UpdateBuffDurationsAction;
            characterController.DestroyModelAction += OnDestroyModelAction;

            characterController.UpdateCharacterModelData(characterModelData);
        }

        protected virtual void OnDestroyModelAction()
        {
            characterController.GetCharacterStatValueFunc -= OnGetCharacterStatValueFunc;
            characterController.TakeDamageFunc -= OnTakeDamageAction;
            characterController.GetActiveBuffsCountFunc -= OnGetActiveBuffsCountFunc;
            characterController.GetActiveBuffsListFunc -= OnGetActiveBuffsListFunc;
            characterController.ActivateBuffAction -= OnActivateBuffAction;
            characterController.ActivateDebuffAction -= OnActivateDebuffAction;
            characterController.HealCharacterAction -= OnHealCharacterAction;
            characterController.UpdateBuffDurationsAction -= UpdateBuffDurationsAction;
            characterController.DestroyModelAction -= OnDestroyModelAction;
            characterController = null;
        }

        protected virtual void OnHealCharacterAction(float healValue)
        {
            int dataIndex = Array.FindIndex(characterModelData.CharacterStats, x => x.Id == "Health");
            characterModelData.CharacterStats[dataIndex].Value += healValue;
            characterModelData.CharacterStats[dataIndex].Value = Mathf.Clamp(
                characterModelData.CharacterStats[dataIndex].Value,
                0,
                characterModelData.CharacterStats[dataIndex].MaxValue
                );
            characterController.UpdateCharacterModelData(characterModelData);
        }

        protected virtual void OnActivateBuffAction(CharacterBuffData characterBuffData)
        {
            CharacterModelDataModel.CharacterBuff characterBuff = new CharacterModelDataModel.CharacterBuff(
                characterBuffData.Id,
                characterBuffData.IsTargetSelf,
                characterBuffData.Duration,
                characterBuffData.statMultipliers
                );
            characterModelData.CharacterBuffs.Add(characterBuff);
            if (characterBuffData.IsTargetSelf)
            {
                foreach (var statMultiplier in characterBuff.StatMultipliers)
                {
                    int dataIndex = Array.FindIndex(characterModelData.CharacterStats, x => x.Id == statMultiplier.Id);
                    characterModelData.CharacterStats[dataIndex].Multiplier += statMultiplier.BonusMultiplier;
                    characterModelData.CharacterStats[dataIndex].RealValue += statMultiplier.BonusAdditive;
                    characterModelData.CharacterStats[dataIndex].Value = Mathf.Clamp(
                        characterModelData.CharacterStats[dataIndex].RealValue,
                        0,
                        characterModelData.CharacterStats[dataIndex].MaxValue);
                }
            }
            characterController.UpdateCharacterModelData(characterModelData);
        }

        protected virtual void OnActivateDebuffAction(CharacterBuffData characterDebuffData)
        {
            CharacterModelDataModel.CharacterBuff characterDebuff = new CharacterModelDataModel.CharacterBuff(
                characterDebuffData.Id,
                characterDebuffData.IsTargetSelf,
                characterDebuffData.Duration,
                characterDebuffData.statMultipliers
                );
            characterModelData.CharacterDebuffs.Add(characterDebuff);

            foreach (var statMultiplier in characterDebuff.StatMultipliers)
            {
                int dataIndex = Array.FindIndex(characterModelData.CharacterStats, x => x.Id == statMultiplier.Id);
                characterModelData.CharacterStats[dataIndex].Multiplier += statMultiplier.BonusMultiplier;
                characterModelData.CharacterStats[dataIndex].RealValue += statMultiplier.BonusAdditive;
                characterModelData.CharacterStats[dataIndex].Value = Mathf.Clamp(
                    characterModelData.CharacterStats[dataIndex].RealValue,
                    0,
                    characterModelData.CharacterStats[dataIndex].MaxValue);
            }

            characterController.UpdateCharacterModelData(characterModelData);
        }

        protected virtual List<string> OnGetActiveBuffsListFunc()
        {
            return characterModelData.CharacterBuffs.Select(buff => buff.Id).ToList();
        }

        protected virtual int OnGetActiveBuffsCountFunc()
        {
            return characterModelData.CharacterBuffs.Count;
        }

        protected virtual float OnTakeDamageAction(float damage)
        {
            int healthIndex = Array.FindIndex(characterModelData.CharacterStats, x => x.Id == "Health");
            int armorIndex = Array.FindIndex(characterModelData.CharacterStats, x => x.Id == "Armor");
            float takenDamage = damage * (100 - characterModelData.CharacterStats[armorIndex].Value) / 100;
            characterModelData.CharacterStats[healthIndex].Value -= takenDamage;
            characterController.UpdateCharacterModelData(characterModelData);
            if (characterModelData.CharacterStats[healthIndex].Value < 0)
            {
                characterController.KillCharacter();
            }
            return takenDamage;
        }

        protected virtual float OnGetCharacterStatValueFunc(string statId)
        {
            int dataIndex = Array.FindIndex(characterModelData.CharacterStats, x => x.Id == statId);
            return characterModelData.CharacterStats[dataIndex].Value * characterModelData.CharacterStats[dataIndex].Multiplier;
        }

        protected virtual void UpdateBuffDurationsAction()
        {
            for (int i = characterModelData.CharacterBuffs.Count - 1; i >= 0; i--)
            {
                var buff = characterModelData.CharacterBuffs[i];
                buff.Duration--;
                if (buff.Duration == 0)
                {
                    if (buff.IsTargetSelf)
                    {
                        foreach (var statMultiplier in buff.StatMultipliers)
                        {
                            int dataIndex = Array.FindIndex(characterModelData.CharacterStats, x => x.Id == statMultiplier.Id);
                            characterModelData.CharacterStats[dataIndex].Multiplier -= statMultiplier.BonusMultiplier;
                            characterModelData.CharacterStats[dataIndex].RealValue -= statMultiplier.BonusAdditive;
                            characterModelData.CharacterStats[dataIndex].Value = Mathf.Clamp(
                                characterModelData.CharacterStats[dataIndex].RealValue,
                                0,
                                characterModelData.CharacterStats[dataIndex].MaxValue);
                        }
                    }
                    characterModelData.CharacterBuffs.RemoveAt(i);
                }
            }

            for (int i = characterModelData.CharacterDebuffs.Count - 1; i >= 0; i--)
            {
                var debuff = characterModelData.CharacterDebuffs[i];
                debuff.Duration--;
                if (debuff.Duration == 0)
                {
                    foreach (var statMultiplier in debuff.StatMultipliers)
                    {
                        int dataIndex = Array.FindIndex(characterModelData.CharacterStats, x => x.Id == statMultiplier.Id);
                        characterModelData.CharacterStats[dataIndex].Multiplier -= statMultiplier.BonusMultiplier;
                        characterModelData.CharacterStats[dataIndex].RealValue -= statMultiplier.BonusAdditive;
                        characterModelData.CharacterStats[dataIndex].Value = Mathf.Clamp(
                            characterModelData.CharacterStats[dataIndex].RealValue,
                            0,
                            characterModelData.CharacterStats[dataIndex].MaxValue);
                    }
                    characterModelData.CharacterDebuffs.RemoveAt(i);
                }
            }

            characterController.UpdateCharacterModelData(characterModelData);
        }
    }
}
