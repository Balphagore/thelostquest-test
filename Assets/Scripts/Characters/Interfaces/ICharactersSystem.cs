using System;

namespace TheLostQuestTest.Characters
{
    public interface ICharactersSystem
    {
        Action DestroyCharactersAction { get; set; }

        Action<bool, int> SpawnCharacterAction { get; set; }

        Action<bool> EndTurnAction { get; set; }

        Action<CharacterModelDataModel> UpdateCharacterDataAction { get; set; }

        void CharacterAttack(bool isPlayerCharacter, int characterIndex);

        void CharacterBuff(bool isPlayerCharacter, int characterIndex);

        void StartNewGame();

        void UpdateCharacterData(CharacterModelDataModel characterModelData);
    }
}
