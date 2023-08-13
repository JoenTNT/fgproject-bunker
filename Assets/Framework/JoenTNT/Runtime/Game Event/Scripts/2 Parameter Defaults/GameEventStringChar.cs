using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, char).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Char Event",
        menuName = "JT Framework/Game Events/2 Parameter/String Char")]
    public class GameEventStringChar : GameEvent<string, char> { }
}
