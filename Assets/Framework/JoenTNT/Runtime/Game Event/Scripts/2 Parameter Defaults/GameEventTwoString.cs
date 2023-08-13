using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, string).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String String Event",
        menuName = "JT Framework/Game Events/2 Parameter/String String")]
    public class GameEventTwoString : GameEvent<string, string> { }
}
