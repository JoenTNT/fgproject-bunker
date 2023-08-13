using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(int, double).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Double Event",
        menuName = "JT Framework/Game Events/2 Parameter/Int Double")]
    public class GameEventIntDouble : GameEvent<int, double> { }
}
