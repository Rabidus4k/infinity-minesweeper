public class SoundModel : ISoundModel
{
    public bool IsLoaded {get; private set;}
    public bool Sound {get; private set;}
    public bool Music { get; private set; }

    public SoundModel()
    {
        Sound = true;
        Music = true;
    }

    public void ToggleMusic()
    {
        Music = !Music;
    }

    public void ToggleSound()
    {
        Sound = !Sound;
    }

    public void LoadData(object data)
    {
        if (data != null)
        {
            Sound = ((SoundSaveData)data).Sound;
            Music = ((SoundSaveData)data).Music;
        }

        IsLoaded = true;
    }
}

public interface ISoundModel : ILoadableModel
{
    public bool Sound { get; }
    public bool Music { get; }
    public void ToggleSound();
    public void ToggleMusic();
}
