namespace TheLostQuestTest.Characters
{
    public abstract class CharacterModelsFactoryBase : ICharacterModelsFactory
    {
        public void Create(ICharacterController characterController, CharacterModelDataModel characterModelData)
        {
            CharacterModel characterModel = new();
            characterModel.Initialize(characterController, characterModelData);
        }
    }
}
