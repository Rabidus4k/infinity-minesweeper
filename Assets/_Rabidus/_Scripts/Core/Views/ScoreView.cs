using UnityEngine;
using Zenject;

public class ScoreView : MonoBehaviour
{
    [SerializeField] protected TMPro.TextMeshProUGUI _scoreText;

    protected IScoreViewModel _viewModel;

    [Inject]
    private void Contsruct(IScoreViewModel viewModel)
    {
        _viewModel = viewModel;

        _viewModel.Score.OnChanged += SetScore;
    }

    private void OnDisable()
    {
        _viewModel.Score.OnChanged -= SetScore;
    }

    protected void SetScore(int score)
    {
        _scoreText.SetText(score.ToString());
    }
}
