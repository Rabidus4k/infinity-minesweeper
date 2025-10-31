public interface ILoadableViewModel
{
    public ReactiveProperty<bool> IsLoaded { get; }
    void LoadData(object data);
}

public interface ILoadableModel 
{
    public bool IsLoaded { get; }
    void LoadData(object data);
}

