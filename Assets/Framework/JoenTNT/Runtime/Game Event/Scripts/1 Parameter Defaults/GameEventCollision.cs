using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter(Collision).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Collision Event",
        menuName = "JT Framework/Game Events/1 Parameter/Collision")]
    public class GameEventCollision : GameEvent<Collision> { }
}
