using Cysharp.Threading.Tasks;
using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourcePrefab;
    [SerializeField] private int _sameSoundMaxCount = 5;

    private bool _canPlay = true;

    [field : SerializeField] public SerializedDictionary<string, Sound> DefaultSounds { get; private set; } = new SerializedDictionary<string, Sound>();

    [SerializeField] private Sound _debugSound;

    private Dictionary<Sound, int> _currentSounds = new Dictionary<Sound, int>();

    public void PlaySound(string sound)
    {
        if (!DefaultSounds.ContainsKey(sound))
        {
            Debug.LogWarning($"[PlaySound] No such sound: {sound}");
            return;
        }

        Sound selectedSound = DefaultSounds[sound];
        PlaySound(selectedSound);
    }

    public void PlaySound(Sound sound)
    {
        if (!_canPlay) return;

        if (_currentSounds.ContainsKey(sound) && _currentSounds[sound] > _sameSoundMaxCount)
            return;

        var audioSource = LeanPool.Spawn(_audioSourcePrefab);
        audioSource.clip = sound.Clip;
        audioSource.volume = sound.Volume;
        audioSource.loop = sound.Loop;
        audioSource.pitch = sound.Pitch;

        audioSource.Play();

        if (_currentSounds.ContainsKey(sound))
            _currentSounds[sound]++;
        else
            _currentSounds.Add(sound, 1);


        if (!sound.Loop)
            DespawnSoundWithDelay(audioSource, sound).Forget();
    }

    private async UniTask DespawnSoundWithDelay(AudioSource source, Sound sound)
    {
        await UniTask.WaitForSeconds(sound.LifeTime);

        if (_currentSounds.ContainsKey(sound))
            _currentSounds.Remove(sound);

        LeanPool.Despawn(source);
    }
    
    public void ToggleSound()
    {
        _canPlay = !_canPlay;
    }

    [Button]
    private void DebugPlay()
    {
        PlaySound(_debugSound);
    }
}
