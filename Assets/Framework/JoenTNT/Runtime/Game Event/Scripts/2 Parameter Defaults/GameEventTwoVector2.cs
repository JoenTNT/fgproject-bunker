using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(Vector2, Vector2).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Vector2 Vector2 Event",
        menuName = "JT Framework/Game Events/2 Parameter/Vector2 Vector2")]
    public class GameEventTwoVector2 : GameEvent<Vector2, Vector2> { }
}
