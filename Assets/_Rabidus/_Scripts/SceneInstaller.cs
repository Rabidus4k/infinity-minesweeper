using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private ZoomConfig _zoomConfig;

    public override void InstallBindings()
    {
        Container.Bind<IZoomConfig>().FromInstance(_zoomConfig).AsSingle();

        Container.Bind<IInputModel>().To<InputModel>().AsSingle();
        Container.Bind<IInputViewModel>().To<InputViewModel>().AsSingle();
    }
}
