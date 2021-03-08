using Elysium.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Leaderboards
{
    public class ScoreSubmitter : MonoBehaviour
    {
        [SerializeField] private LongEventSO doSubmitScore = default;
        [SerializeField] private EventSO onFetchScore = default;

        public string PlayerName => null;
        public bool PlayerHasName => PlayerName != null && PlayerName.Length > 0;

        private void OnEnable() => doSubmitScore.OnRaise += SubmitScore;
        private void OnDisable() => doSubmitScore.OnRaise -= SubmitScore;

        private void SubmitScore(long _score)
        {
            string leaderboard = Leaderboards.LeaderboardID;
            Debug.Log($"Submitting leaderboard score of {_score} to leaderboard {leaderboard}.");

            if (!PlayerHasName)
            {
                Debug.LogError("Failed to submit score, user doesn't have a name");
                return;
            }

            void PostScoreCallback(bool _success) 
            {
                if (!_success) { return; }
                onFetchScore.Raise();
            };

            _ = Leaderboards.PostPlayerScore(leaderboard, PlayerName, _score, PostScoreCallback);
        }

        [ContextMenu("Submit Dummy Score")]
        public void SubmitDummyScore()
        {
            string playerName = "DummyPlayer5";
            string leaderboard = Leaderboards.LeaderboardID;
            long dummyScore = 2352351;

            void PostScoreCallback(bool _success) 
            {
                Action<bool, Leaderboards.GetScoreRequest> OnGetPlayerScore = (success, _score) =>
                {
                    Debug.Log($"Success: {success} | Rank: {_score.rank} | Score: {_score.score}");
                };
                
                Action<bool, Leaderboards.GetHighestScoresRequest> OnGetTopScores = (success, response) =>
                {
                    foreach (var score in response.scores)
                    {
                        Debug.Log($"ID: {score.playerID} | Score: {score.score}");
                    }
                };

                _ = Leaderboards.GetPlayerScore(leaderboard, playerName, OnGetPlayerScore);
                _ = Leaderboards.GetTopScores(leaderboard, OnGetTopScores);
            };

            _ = Leaderboards.PostPlayerScore(leaderboard, playerName, dummyScore, PostScoreCallback);
        }
    }
}
