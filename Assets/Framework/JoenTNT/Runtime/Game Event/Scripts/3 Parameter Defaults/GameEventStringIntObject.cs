using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, int, object).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Int Object Event",
        menuName = "JT Framework/Game Events/3 Parameter/String Int Object")]
    public class GameEventStringIntObject : GameEvent<string, int, object> { }
}
