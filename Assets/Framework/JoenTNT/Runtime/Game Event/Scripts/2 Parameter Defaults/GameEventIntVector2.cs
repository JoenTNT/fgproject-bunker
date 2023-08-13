using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(int, Vector2).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Vector2 Event",
        menuName = "JT Framework/Game Events/2 Parameter/Int Vector2")]
    public class GameEventIntVector2 : GameEvent<int, Vector2> { }
}
