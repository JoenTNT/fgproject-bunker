using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, Vector2).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Vector2 Event",
        menuName = "JT Framework/Game Events/2 Parameter/String Vector2")]
    public class GameEventStringVector2 : GameEvent<string, Vector2> { }
}
