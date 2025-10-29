using MirraGames.SDK.Common;
using UnityEngine;
using Zenject;

public class UILeaderboardEntry : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _numText;
    [SerializeField] private TMPro.TextMeshProUGUI _nameText;
    [SerializeField] private TMPro.TextMeshProUGUI _scoreText;

    public void Initialize(PlayerScore playerScore)
    {
        _numText.SetText(playerScore.position.ToString());
        _nameText.SetText(playerScore.displayName.ToString());
        _scoreText.SetText(playerScore.score.ToString());
    }

    public class Factory : PlaceholderFactory<UILeaderboardEntry>
    {
    }
}
