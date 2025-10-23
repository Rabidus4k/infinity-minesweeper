using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private ZoomConfig _zoomConfig;
    [SerializeField] private GameConfig _gameConfig;

    public override void InstallBindings()
    {
        Container.Bind<IZoomConfig>().FromInstance(_zoomConfig).AsSingle();
        Container.Bind<IGameConfig>().FromInstance(_gameConfig).AsSingle();

        Container.Bind<IInputModel>().To<InputModel>().AsSingle();
        Container.Bind<IInputViewModel>().To<InputViewModel>().AsSingle();

        Container.Bind<IGameModel>().To<GameModel>().AsSingle();
        Container.Bind<IGameViewModel>().To<GameViewModel>().AsSingle();
    }
}
