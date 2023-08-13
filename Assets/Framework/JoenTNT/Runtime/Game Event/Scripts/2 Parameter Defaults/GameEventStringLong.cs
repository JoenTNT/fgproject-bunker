using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, long).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Long Event",
        menuName = "JT Framework/Game Events/2 Parameter/String Long")]
    public class GameEventStringLong : GameEvent<string, long> { }
}
