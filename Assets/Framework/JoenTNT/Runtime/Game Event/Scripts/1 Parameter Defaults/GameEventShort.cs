using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (short).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Short Event",
        menuName = "JT Framework/Game Events/1 Parameter/Short")]
    public class GameEventShort : GameEvent<short> { }
}
