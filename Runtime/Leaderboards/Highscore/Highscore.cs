using UnityEngine;

namespace Elysium.Leaderboards
{
    [System.Serializable]
    public class Highscore
    {
        [SerializeField] private string name = default;
        [SerializeField] private int rank = default;
        [SerializeField] private long score = default;
        [SerializeField] private bool isPlayer = default;

        public string Name => name;
        public int Rank => rank;
        public long Score => score;
        public bool IsPlayer => isPlayer;

        public Highscore(string _name, int _rank, long _score, bool _isPlayer = false)
        {
            this.name = _name;
            this.rank = _rank + 1; // => Add one to rank as the backend returns a value starting at 0
            this.score = _score;
            this.isPlayer = _isPlayer;
        }
    }
}