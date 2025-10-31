public class ScoreModel : IScoreModel
{
    public bool IsLoaded { get; private set; }
    public int Score { get; private set; }
    public int MaxScore { get; private set; }

    public void AddScore(int score)
    {
        Score += score;
    }

    public void LoadData(object data)
    {
        if (data != null)
            MaxScore = ((ScoreSaveData)data).MaxScore;
        IsLoaded = true;
    }

    public void SetMaxScore(int maxScore)
    {
        MaxScore = maxScore;
    }
}

public interface IScoreModel: ILoadableModel
{
    public int Score { get; }
    public int MaxScore { get; }
    public void AddScore(int score);
    public void SetMaxScore(int maxScore);
}