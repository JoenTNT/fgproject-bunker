using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(Transform, GameObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Transform GameObject Event",
        menuName = "JT Framework/Game Events/2 Parameter/Transform GameObject")]
    public class GameEventTransformGameObject : GameEvent<Transform, GameObject> { }
}
