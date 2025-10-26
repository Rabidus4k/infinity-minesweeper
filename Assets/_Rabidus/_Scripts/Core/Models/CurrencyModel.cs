public class CurrencyModel : ICurrencyModel
{
    public int Gems {get; private set;}

    public void AddGems(int value)
    {
        Gems += value;
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

public interface ICurrencyModel
{
    public int Gems { get; }

    void AddGems(int value);
    void SpendGems(int value);
    bool TrySpendGems(int value);
}