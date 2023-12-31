using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 4 parameters(string, int, Vector2, Vector3).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Int Vector2 Vector3 Event",
        menuName = "JT Framework/Game Events/4 Parameter/String Int Vector2 Vector3")]
    public class GameEventStringIntVector2Vector3 : GameEvent<string, int, Vector2, Vector3> { }
}
