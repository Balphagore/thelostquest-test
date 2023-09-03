using System;
using UnityEngine;

namespace TheLostQuestTest.Characters
{
    [CreateAssetMenu(fileName = "CharacterBuffData", menuName = "Game Data/Character Buff Data")]
    public class CharacterBuffData : ScriptableObject
    {
        public string Id;
        public bool IsTargetSelf;
        public StatMultiplier[] statMultipliers;
        public int Duration;

        public StatTypes StatTypesData;

        [Serializable]
        public class StatMultiplier
        {
            [Dropdown("StatTypesData.Types")]
            public string Id;
            public float BonusMultiplier;
            public float BonusAdditive;
        }
    }
}
