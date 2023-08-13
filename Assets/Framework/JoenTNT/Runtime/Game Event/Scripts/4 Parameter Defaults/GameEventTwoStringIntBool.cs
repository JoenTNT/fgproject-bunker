using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 4 parameters(string, string, int, bool).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Game Event String String Int Bool",
        menuName = "JT Framework/Game Events/4 Parameter/String String Int Bool")]
    public class GameEventTwoStringIntBool : GameEvent<string, string, int, bool> { }
}
