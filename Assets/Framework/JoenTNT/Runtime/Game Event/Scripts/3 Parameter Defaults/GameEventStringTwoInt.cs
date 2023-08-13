using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, int, int).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Int Int Event",
        menuName = "JT Framework/Game Events/3 Parameter/String Int Int")]
    public class GameEventStringTwoInt : GameEvent<string, int, int> { }
}
