using Elysium.Core;
using Elysium.Utils.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Leaderboards
{
    public class ScoreReceiver : MonoBehaviour
    {
        [Separator("Events", true)]
        [SerializeField] private EventSO doFetchScore = default;
        [SerializeField] private EventSO onFetchScore = default;

        [Separator("Cached Scores", true)]
        [SerializeField] private HighscoreValueSO playerScore = default;
        [SerializeField] private HighscoreArrayValueSO highscores = default;

        public string PlayerName => "DummyPlayer4";
        public bool PlayerHasName => PlayerName != null && PlayerName.Length > 0;

        private void OnEnable() => doFetchScore.OnRaise += FetchAllScores;
        private void OnDisable() => doFetchScore.OnRaise -= FetchAllScores;

        private void FetchAllScores()
        {
            FetchIndividualScores();
            FetchHighscores();
        }

        [ContextMenu("Fetch Individual Scores")]
        private void FetchIndividualScores()
        {
            string leaderboard = Leaderboards.LeaderboardID;
            Debug.Log($"Fetching leaderboard score for {PlayerName} on leaderboard {leaderboard}.");

            if (!PlayerHasName)
            {
                Debug.LogError("Failed to fetch score, user doesn't have a name");
                return;
            }

            Action<bool, Leaderboards.GetScoreRequest> OnGetPlayerScore = (success, _score) =>
            {
                if (!success) 
                {
                    Debug.Log("Failed to get individual leaderboard data");
                    return; 
                }

                Debug.Log($"Rank: {_score.rank} | Score: {_score.score}");
                playerScore.Value = new Highscore(PlayerName, _score.rank, _score.score, true);
            };

            _ = Leaderboards.GetPlayerScore(leaderboard, PlayerName, OnGetPlayerScore);            
        }

        [ContextMenu("Fetch Highscores Scores")]
        private void FetchHighscores()
        {
            string leaderboard = Leaderboards.LeaderboardID;
            Debug.Log($"Fetching top leaderboard scores on leaderboard {leaderboard}.");

            Action<bool, Leaderboards.GetHighestScoresRequest> OnGetTopScores = (success, response) =>
            {
                if (!success)
                {
                    Debug.Log("Failed to get highscore leaderboard data");
                    return;
                }
                
                highscores.Value = new Highscore[response.scores.Length];
                for (int i = 0; i < response.scores.Length; i++)
                {
                    int index = i;
                    var resp = response.scores[index];
                    Debug.Log($"ID: {resp.playerID} | Score: {resp.score}");
                    bool isPlayer = PlayerHasName && resp.playerID == PlayerName;
                    highscores.Value[index] = new Highscore(resp.playerID, index, resp.score, isPlayer);
                }
            };

            _ = Leaderboards.GetTopScores(leaderboard, OnGetTopScores);
        }
    }
}