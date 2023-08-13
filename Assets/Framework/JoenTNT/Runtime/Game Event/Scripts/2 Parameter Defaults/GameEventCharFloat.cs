using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(char, float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Char Float Event",
        menuName = "JT Framework/Game Events/2 Parameter/Char Float")]
    public class GameEventCharFloat : GameEvent<char, float> { }
}
