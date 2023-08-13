using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(int, int).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Int Event",
        menuName = "JT Framework/Game Events/2 Parameter/Int Int")]
    public class GameEventTwoInt : GameEvent<int, int> { }
}
