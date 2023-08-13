using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 4 parameters(string, string, int, float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String String Int Float Event",
        menuName = "JT Framework/Game Events/4 Parameter/String String Int Float")]
    public class GameEventTwoStringIntFloat : GameEvent<string, string, int, float> { }
}
