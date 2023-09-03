using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TheLostQuestTest.Characters
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Game Data/Character Data")]
    public class CharacterData : ScriptableObject
    {
        public AssetReferenceGameObject CharacterView;
        public CharacterStatsDataModel CharacterStatsData;
        public StatTypes CharacterStatTypes;
    }
}
