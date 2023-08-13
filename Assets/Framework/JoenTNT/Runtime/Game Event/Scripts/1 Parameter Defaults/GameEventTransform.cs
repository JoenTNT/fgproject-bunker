using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (Transform).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Transform Event",
        menuName = "JT Framework/Game Events/1 Parameter/Transform")]
    public class GameEventTransform : GameEvent<Transform> { }
}
