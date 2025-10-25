public class ScoreViewModel : IScoreViewModel
{
    public ReactiveProperty<int> Score {get; private set;} = new ReactiveProperty<int>();

    protected IScoreModel _model;

    public ScoreViewModel(IScoreModel model)
    {
        _model = model;
    }

    public void AddScore(int score)
    {
        _model.AddScore(score);
        Score.Value = _model.Score;
    }
}

public interface IScoreViewModel 
{
    public ReactiveProperty<int> Score { get; }
    public void AddScore(int score);
}
