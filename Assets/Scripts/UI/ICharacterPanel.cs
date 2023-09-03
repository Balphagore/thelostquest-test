namespace TheLostQuestTest.UI
{
    public interface ICharacterPanel
    {
        void Activate(bool isPlayerCharacter, int characterIndex);

        void Deactivate();

        void SetInteractible(bool isInteractable);

        void UpdateStatsUI(CharacterStatsUIUpdateDataModel characterStatsUIUpdateData);
    }
}
