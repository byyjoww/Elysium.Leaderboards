using Elysium.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Leaderboards
{
    [CreateAssetMenu(fileName = "ScoreValueSO", menuName = "Scriptable Objects/Value/Score", order = 1)]
    public class ScoreValueSO : GenericValueSO<string, long>
    {
        
    }
}
