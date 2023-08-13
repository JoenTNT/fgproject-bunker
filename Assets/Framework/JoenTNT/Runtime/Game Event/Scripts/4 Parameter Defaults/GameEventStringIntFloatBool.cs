using System;
using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 4 parameters(string, int, float, bool).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Int Float Bool Event",
        menuName = "JT Framework/Game Events/4 Parameter/String Int Float Bool")]
    public class GameEventStringIntFloatBool : GameEvent<string, int, float, bool> { }
}
