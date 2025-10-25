using UnityEngine;

public class ScoreModel : IScoreModel
{
    public int Score { get; private set; }

    public void AddScore(int score)
    {
        Score += score;
    }
}

public interface IScoreModel 
{
    public int Score { get; }
    public void AddScore(int score);
}