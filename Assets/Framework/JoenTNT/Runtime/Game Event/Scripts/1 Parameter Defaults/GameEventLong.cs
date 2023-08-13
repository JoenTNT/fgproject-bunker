using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (long).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Long Event",
        menuName = "JT Framework/Game Events/1 Parameter/Long")]
    public class GameEventLong : GameEvent<long> { }
}
