public class CurrencyModel : ICurrencyModel
{
    public bool IsLoaded { get; private set; }
    public int Gems {get; private set;}

    public void AddGems(int value)
    {
        Gems += value;
    }

    public void LoadData(object data)
    {
        if (data != null)
            Gems = ((CurrencySaveData)data).Gems;

        IsLoaded = true;
    }

    public void SpendGems(int value)
    {
        Gems -= value;
    }

    public bool TrySpendGems(int value)
    {
        if (value > Gems)
            return false;
        
        SpendGems(value);

        return true;
    }
}

public interface ICurrencyModel : ILoadableModel
{
    public int Gems { get; }

    void AddGems(int value);
    void SpendGems(int value);
    bool TrySpendGems(int value);
}