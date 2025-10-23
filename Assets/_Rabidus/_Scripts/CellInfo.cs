[System.Serializable]
public class CellInfo
{
    public int Value;
    public bool IsOpened = false;

    public CellInfo(int value)
    {
        Value = value;
    }
}
