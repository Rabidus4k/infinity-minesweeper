public class CurrencyViewModel : ICurrencyViewModel
{
    public ReactiveProperty<bool> IsLoaded { get; private set; } = new ReactiveProperty<bool>();
    public ReactiveProperty<int> Gems { get; protected set; } = new ReactiveProperty<int>();

    protected ICurrencyModel _model;

    public CurrencyViewModel(ICurrencyModel model) 
    {
        _model = model;

        Gems.Value = _model.Gems;
    }

    public void AddGems(int value)
    {
        _model.AddGems(value);

        Gems.Value = _model.Gems;
    }

    public void SpendGems(int value)
    {
        if (_model.TrySpendGems(value))
        {
            Gems.Value = _model.Gems;
        }
    }

    public void LoadData(object data)
    {
        _model.LoadData(data);
        IsLoaded.Value = _model.IsLoaded;
    }
}

public interface ICurrencyViewModel : ILoadableViewModel
{
    public ReactiveProperty<int> Gems { get; }
    void AddGems(int value);
    void SpendGems(int value);
}

