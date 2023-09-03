namespace TheLostQuestTest.Characters
{
    public interface ICharacterModelsFactory
    {
        void Create(ICharacterController characterController, CharacterModelDataModel characterModelData);
    }
}
