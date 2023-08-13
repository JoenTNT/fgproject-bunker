using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, string, float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String String Float Event",
        menuName = "JT Framework/Game Events/3 Parameter/String String Float")]
    public class GameEventTwoStringFloat : GameEvent<string, string, float> { }
}
