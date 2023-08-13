using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (Vector3).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Vector3 Event",
        menuName = "JT Framework/Game Events/1 Parameter/Vector3")]
    public class GameEventVector3 : GameEvent<Vector3> { }
}
