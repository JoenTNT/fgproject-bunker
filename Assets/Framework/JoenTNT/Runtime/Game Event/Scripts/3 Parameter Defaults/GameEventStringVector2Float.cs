using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, Vector2, float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Vector2 Float Event",
        menuName = "JT Framework/Game Events/3 Parameter/String Vector2 Float")]
    public class GameEventStringVector2Float : GameEvent<string, Vector2, float> { }
}
