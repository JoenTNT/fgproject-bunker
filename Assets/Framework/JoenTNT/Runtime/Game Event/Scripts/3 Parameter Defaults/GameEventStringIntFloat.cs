using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, int, float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Int Float Event",
        menuName = "JT Framework/Game Events/3 Parameter/String Int Float")]
    public class GameEventStringIntFloat : GameEvent<string, int, float> { }
}
