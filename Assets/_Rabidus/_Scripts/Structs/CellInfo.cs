[System.Serializable]
public class CellInfo
{
    public int Value;
    public bool IsOpened = false;
    public bool IsFlagged = false;
    public CellInfo(int value)
    {
        Value = value;
    }
}
