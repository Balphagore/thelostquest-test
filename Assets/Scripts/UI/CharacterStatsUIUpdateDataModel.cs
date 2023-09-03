using System;
using System.Collections.Generic;
using System.Linq;

using TheLostQuestTest.Characters;

namespace TheLostQuestTest.UI
{
    public struct CharacterStatsUIUpdateDataModel
    {
        public List<string> BuffsStatuses;
        public float Health;
        public float MaximumHealth;
        public float Armor;
        public float MaximumArmor;
        public float Vampirism;
        public float MaximumVampirism;

        public CharacterStatsUIUpdateDataModel(CharacterModelDataModel characterModelData)
        {
            BuffsStatuses = characterModelData.CharacterBuffs
                .Select(buff => $"{buff.Id} ({buff.Duration})")
                .ToList();

            int dataIndex = Array.FindIndex(characterModelData.CharacterStats, x => x.Id == "Health");
            Health = characterModelData.CharacterStats[dataIndex].Value;
            MaximumHealth = characterModelData.CharacterStats[dataIndex].MaxValue;

            dataIndex = Array.FindIndex(characterModelData.CharacterStats, x => x.Id == "Armor");
            Armor = characterModelData.CharacterStats[dataIndex].Value;
            MaximumArmor = characterModelData.CharacterStats[dataIndex].MaxValue;

            dataIndex = Array.FindIndex(characterModelData.CharacterStats, x => x.Id == "Vampirism");
            Vampirism = characterModelData.CharacterStats[dataIndex].Value;
            MaximumVampirism = characterModelData.CharacterStats[dataIndex].MaxValue;
        }
    }
}
