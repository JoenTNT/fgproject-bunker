using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(Vector2, float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Vector2 Float Event",
        menuName = "JT Framework/Game Events/2 Parameter/Vector2 Float")]
    public class GameEventVector2Float : GameEvent<Vector2, float> { }
}
