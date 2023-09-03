using System;

namespace TheLostQuestTest.Characters
{
    [Serializable]
    public class CharacterStatsDataModel
    {
        public CharacterStat[] CharacterStats;

        [Serializable]
        public class CharacterStat
        {
            [Dropdown("CharacterStatTypes.Types")]
            public string Id;
            public float Value;
            public float MaxValue;
        }

        public CharacterModelDataModel.CharacterStat[] Copy()
        {
            CharacterModelDataModel.CharacterStat[] characterStats = new CharacterModelDataModel.CharacterStat[CharacterStats.Length];

            for (int i = 0; i < CharacterStats.Length; i++)
            {
                characterStats[i] = new CharacterModelDataModel.CharacterStat();
                characterStats[i].Id = CharacterStats[i].Id;
                characterStats[i].Value = CharacterStats[i].Value;
                characterStats[i].RealValue = CharacterStats[i].Value;
                characterStats[i].MaxValue = CharacterStats[i].MaxValue;
                characterStats[i].Multiplier = 1;
            }

            return characterStats;
        }
    }
}
