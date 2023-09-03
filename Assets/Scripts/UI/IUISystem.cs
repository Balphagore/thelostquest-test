namespace TheLostQuestTest.UI
{
    public interface IUISystem
    {
        void Attack(bool isPlayerCharacter, int characterIndex);

        void Buff(bool isPlayerCharacter, int characterIndex);
    }
}
