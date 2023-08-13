using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 4 parameters(string, string, int, int).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String String Int Int Event",
        menuName = "JT Framework/Game Events/4 Parameter/String String Int Int")]
    public class GameEventTwoStringTwoInt : GameEvent<string, string, int, int> { }
}
