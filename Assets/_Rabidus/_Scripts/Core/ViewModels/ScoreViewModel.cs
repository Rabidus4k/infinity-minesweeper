public class ScoreViewModel : IScoreViewModel
{
    public ReactiveProperty<bool> IsLoaded { get; private set; } = new ReactiveProperty<bool>();
    public ReactiveProperty<int> Score {get; private set;} = new ReactiveProperty<int>();
    public ReactiveProperty<int> MaxScore { get; private set;} = new ReactiveProperty<int>();

    protected IScoreModel _model;

    public ScoreViewModel(IScoreModel model)
    {
        _model = model;
    }

    public void AddScore(int score)
    {
        _model.AddScore(score);
        Score.Value = _model.Score;

        ComputeMaxScore();
    }

    private void ComputeMaxScore()
    {
        if (_model.Score > _model.MaxScore)
        {
            _model.SetMaxScore(_model.Score);
            MaxScore.Value = _model.MaxScore;
        }
    }

    public void LoadData(object data)
    {
        _model.LoadData(data);
        IsLoaded.Value = _model.IsLoaded;
    }
}

public interface IScoreViewModel : ILoadableViewModel
{
    public ReactiveProperty<int> Score { get; }
    public ReactiveProperty<int> MaxScore { get; }
    public void AddScore(int score);
}
