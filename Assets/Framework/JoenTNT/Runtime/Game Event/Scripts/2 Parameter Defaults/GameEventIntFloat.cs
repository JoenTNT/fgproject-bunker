using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(int, float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Float Event",
        menuName = "JT Framework/Game Events/2 Parameter/Int Float")]
    public class GameEventIntFloat : GameEvent<int, float> { }
}
