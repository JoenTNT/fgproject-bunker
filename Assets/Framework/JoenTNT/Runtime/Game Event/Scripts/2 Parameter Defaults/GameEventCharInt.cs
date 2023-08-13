using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(char, int).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Char Int Event",
        menuName = "JT Framework/Game Events/2 Parameter/Char Int")]
    public class GameEventCharInt : GameEvent<char, int> { }
}
