using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, string, string).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String String String Event",
        menuName = "JT Framework/Game Events/3 Parameter/String String String")]
    public class GameEventThreeString : GameEvent<string, string, string> { }
}
