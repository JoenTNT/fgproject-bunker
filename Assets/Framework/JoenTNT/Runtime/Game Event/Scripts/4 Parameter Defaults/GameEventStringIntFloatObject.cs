using System;
using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 4 parameters(string, int, float, Object).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Int Float Object Event",
        menuName = "JT Framework/Game Events/4 Parameter/String Int Float object")]
    public class GameEventStringIntFloatObject : GameEvent<string, int, float, object> { }
}
