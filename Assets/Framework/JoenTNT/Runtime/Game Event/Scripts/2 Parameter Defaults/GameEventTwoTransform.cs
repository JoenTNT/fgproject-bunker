using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(Transform, Transform).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Game Event Transform Transform",
        menuName = "JT Framework/Game Events/2 Parameter/Transform Transform")]
    public class GameEventTwoTransform : GameEvent<Transform, Transform> { }
}
