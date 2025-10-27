using System;
using UnityEngine;
using Zenject;

public class PopupInstaller : MonoInstaller
{
    [SerializeField] private PopupConfig _config;
    [SerializeField] private Popup _popup;

    public override void InstallBindings()
    {
        BindConfigs();
        BindServices();
        BindFactories();
    }

    private void BindConfigs()
    {
        Container.Bind<IPopupConfig>().FromInstance(_config).AsSingle();
    }

    private void BindServices()
    {
        Container.Bind<IPopupService>().To<PopupService>().AsSingle().NonLazy();
    }

    private void BindFactories()
    {
        Container.Bind<Popup>().FromInstance(_popup);
        //Container.BindFactory<Popup, Popup.Factory>().FromComponentInNewPrefab(_popup).UnderTransformGroup("Popups"); ;
    }
}
