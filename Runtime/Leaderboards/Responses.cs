using UnityEngine;

namespace Elysium.Leaderboards
{
    public partial class Leaderboards
    {
        [System.Serializable]
        public class BaseResponse
        {
            public string code;
            public string message;
        }

        [System.Serializable]
        public class GetScoreRequest : BaseResponse
        {
            public long score;
            public int rank;
        }

        [System.Serializable]
        public class GetHighestScoresRequest : BaseResponse
        {
            public Score[] scores;
        }

        [System.Serializable]
        public class Score
        {
            public string playerID;
            public long score;
        }
    }    
}
