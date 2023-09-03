using System.Collections.Generic;

namespace TheLostQuestTest.Characters
{
    public class CharacterModelDataModel
    {
        public bool IsPlayerCharacter;
        public int CharacterIndex;
        public CharacterStat[] CharacterStats;
        public List<CharacterBuff> CharacterBuffs;
        public List<CharacterBuff> CharacterDebuffs;

        public CharacterModelDataModel(bool isPlayerCharacter, int characterIndex, CharacterStat[] characterStats, List<CharacterBuff> characterBuffs, List<CharacterBuff> characterDebuffs)
        {
            IsPlayerCharacter = isPlayerCharacter;
            CharacterIndex = characterIndex;
            CharacterStats = characterStats;
            CharacterBuffs = characterBuffs;
            CharacterDebuffs = characterDebuffs;
        }

        public class CharacterStat
        {
            public string Id;
            public float Value;
            public float RealValue;
            public float MaxValue;
            public float Multiplier;
        }

        public class CharacterBuff
        {
            public string Id;
            public bool IsTargetSelf;
            public int Duration;
            public CharacterBuffData.StatMultiplier[] StatMultipliers;

            public CharacterBuff(string id, bool isTargetSelf, int duration, CharacterBuffData.StatMultiplier[] statMultipliers)
            {
                Id = id;
                IsTargetSelf = isTargetSelf;
                Duration = duration;
                StatMultipliers = statMultipliers;
            }
        }
    }
}
