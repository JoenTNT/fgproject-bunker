using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (int).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Event",
        menuName = "JT Framework/Game Events/1 Parameter/Int")]
    public class GameEventInt : GameEvent<int> { }
}
