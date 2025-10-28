using System;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private ZoomConfig _zoomConfig;
    [SerializeField] private GameConfig _gameConfig;

    [SerializeField] private GridView _gridView;
    [SerializeField] private UICoordsButton _coordsButton;
    [SerializeField] private UINotificationManager _notificationManager;

    public override void InstallBindings()
    {
        BindConfigs();
        
        BindModels();
        BindServices();

        BindViewModels();
        BindFactories();

        Container.Bind<UINotificationManager>().FromInstance(_notificationManager).AsSingle();
    }

    private void BindServices()
    {
        Container.Bind<ISaveService>().To<SaveService>().AsSingle().NonLazy();
    }

    private void BindFactories()
    {
        Container.BindFactory<GridView, GridView.Factory>().FromComponentInNewPrefab(_gridView).UnderTransformGroup("Grids");
        Container.BindFactory<UICoordsButton, UICoordsButton.Factory>().FromComponentInNewPrefab(_coordsButton);
    }

    private void BindViewModels()
    {
        Container.Bind<IInputViewModel>().To<InputViewModel>().AsSingle();
        Container.Bind<IGameViewModel>().To<GameViewModel>().AsSingle();
        Container.Bind<IScoreViewModel>().To<ScoreViewModel>().AsSingle();
        Container.Bind<ICurrencyViewModel>().To<CurrencyViewModel>().AsSingle();
        Container.Bind<ICoordsViewModel>().To<CoordsViewModel>().AsSingle();
    }

    private void BindModels()
    {
        Container.Bind<IInputModel>().To<InputModel>().AsSingle();
        Container.Bind<IGameModel>().To<GameModel>().AsSingle();
        Container.Bind<IScoreModel>().To<ScoreModel>().AsSingle();
        Container.Bind<ICurrencyModel>().To<CurrencyModel>().AsSingle();
        Container.Bind<ICoordsModel>().To<CoordsModel>().AsSingle();
    }

    private void BindConfigs()
    {
        Container.Bind<IZoomConfig>().FromInstance(_zoomConfig).AsSingle();
        Container.Bind<IGameConfig>().FromInstance(_gameConfig).AsSingle();
    }
}