using Elysium.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Leaderboards
{
    [CreateAssetMenu(fileName = "ScoreEventSO", menuName = "Scriptable Objects/Event/Score", order = 1)]
    public class ScoreEventSO : GenericEventSO<string, long>
    {        

    }
}
