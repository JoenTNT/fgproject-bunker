using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, Vector2, Vector2).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Vector2 Vector2 Event",
        menuName = "JT Framework/Game Events/3 Parameter/String Vector2 Vector2")]
    public class GameEventStringTwoVector2 : GameEvent<string, Vector2, Vector2> { }
}
