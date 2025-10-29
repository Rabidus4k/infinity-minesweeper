using MirraGames.SDK;
using MirraGames.SDK.Common;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LeaderboardView : MonoBehaviour
{
    private const string LeaderboardId = "score";

    private IScoreViewModel _scoreViewModel;
    private UILeaderboardEntry.Factory _factory;

    [SerializeField] private RectTransform _root;

    [Inject]
    private void Construct(IScoreViewModel scoreViewModel, UILeaderboardEntry.Factory factory)
    {
        _factory = factory;
        _scoreViewModel = scoreViewModel;
        _scoreViewModel.MaxScore.OnChanged += MaxScoreChanged;
    }

    private void Awake()
    {
        MirraSDK.WaitForProviders(() => {
            LoadLearboard();
        });
    }

    private void OnDisable()
    {
        _scoreViewModel.MaxScore.OnChanged -= MaxScoreChanged;
    }

    private int _currentMaxScore = 0;
    private Coroutine _coroutine;

    private void MaxScoreChanged(int value)
    {
        Debug.Log($"MAX SCORE: {value}");
        _currentMaxScore = value;

        if (_coroutine != null) return;

        _coroutine = StartCoroutine(AddToLeaderboardCoroutine());
    }

    private IEnumerator AddToLeaderboardCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        MirraSDK.Achievements.SetScore(LeaderboardId, _currentMaxScore);
        _coroutine = null;
    }

    private void LoadLearboard()
    {
        MirraSDK.Achievements.GetLeaderboard(LeaderboardId, (leaderboard) => 
        {
            if (leaderboard == null) return;

            Debug.Log($"получено '{leaderboard.players.Length}' игроков в лидерборде 'leaderboard_id'");

            var sortedPlayers = leaderboard.players.OrderBy((x) => x.position);

            foreach (PlayerScore player in sortedPlayers)
            {
                var entry = _factory.Create();
                entry.Initialize(player);

                entry.transform.SetParent(_root);
                entry.transform.localScale = Vector3.one;
                entry.transform.localPosition = Vector3.zero;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_root);
        });
    }
}
