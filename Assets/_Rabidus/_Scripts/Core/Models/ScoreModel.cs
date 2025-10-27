public class ScoreModel : IScoreModel
{
    public int Score { get; private set; }
    public int MaxScore { get; private set; }

    public void AddScore(int score)
    {
        Score += score;
    }

    public void LoadData(object data)
    {
        MaxScore = ((ScoreSaveData)data).MaxScore;
    }
}

public interface IScoreModel: ILoadable
{
    public int Score { get; }
    public int MaxScore { get; }
    public void AddScore(int score);
}