using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Float Event",
        menuName = "JT Framework/Game Events/2 Parameter/String Float")]
    public class GameEventStringFloat : GameEvent<string, float> { }
}
