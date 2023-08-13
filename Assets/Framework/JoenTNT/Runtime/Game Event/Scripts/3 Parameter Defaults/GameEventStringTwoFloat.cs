using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, float, float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Float Float Event",
        menuName = "JT Framework/Game Events/3 Parameter/String Float Float")]
    public class GameEventStringTwoFloat : GameEvent<string, float, float> { }
}