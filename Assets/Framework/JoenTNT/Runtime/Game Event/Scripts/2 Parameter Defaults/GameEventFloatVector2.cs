using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(float, Vector2).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Float Vector2 Event",
        menuName = "JT Framework/Game Events/2 Parameter/Float Vector2")]
    public class GameEventFloatVector2 : GameEvent<float, Vector2> { }
}
