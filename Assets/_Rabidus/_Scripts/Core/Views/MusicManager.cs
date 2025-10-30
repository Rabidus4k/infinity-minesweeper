using System;
using UnityEngine;
using Zenject;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource _source;

    private ISoundViewModel _soundViewModel;

    [Inject]
    private void Construct(ISoundViewModel soundViewModel)
    {
        _soundViewModel = soundViewModel;

        _soundViewModel.Music.OnChanged += SetMusic;
        SetMusic(_soundViewModel.Music.Value);
    }
    private void OnDisable()
    {
        _soundViewModel.Music.OnChanged -= SetMusic;
    }

    private void SetMusic(bool value)
    {
        _source.enabled = value;
    }
}
