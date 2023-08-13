using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 4 parameters(string, string, string, float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String String String Float Event",
        menuName = "JT Framework/Game Events/4 Parameter/String String String Float")]
    public class GameEventThreeStringFloat : GameEvent<string, string, string, float> { }
}
