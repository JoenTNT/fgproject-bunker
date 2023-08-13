using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 4 parameters(string, int, int, bool).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Int Int Bool Event",
        menuName = "JT Framework/Game Events/4 Parameter/String Int Int Bool")]
    public class GameEventStringTwoIntBool : GameEvent<string, int, int, bool> { }
}
