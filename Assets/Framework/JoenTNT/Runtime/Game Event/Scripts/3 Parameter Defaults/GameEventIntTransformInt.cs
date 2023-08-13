using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(int, Transform, int).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Transform Int Event",
        menuName = "JT Framework/Game Events/3 Parameter/Int Transform Int")]
    public class GameEventIntTransformInt : GameEvent<int, Transform, int> { }
}
