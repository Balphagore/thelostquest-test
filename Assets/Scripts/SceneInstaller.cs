using Zenject;

using TheLostQuestTest.Characters;
using TheLostQuestTest.UI;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IUISystem>().To<UISystem>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ICharactersSystem>().To<CharactersSystem>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IGameManagerSystem>().To<GameManagerSystem>().FromComponentInHierarchy().AsSingle();

        Container.Bind<ICharacterModelsFactory>().WithId("Main").To<CharacterModelsFactory>().FromNew().AsSingle();

        Container.Bind<ICharacterViewsFactory>().WithId("Main").To<CharacterViewsFactory>().FromNew().AsSingle();

        Container.Bind<ICharacterControllersFactory>().WithId("Main").To<CharacterControllersFactory>().FromNew().AsSingle();
    }
}
