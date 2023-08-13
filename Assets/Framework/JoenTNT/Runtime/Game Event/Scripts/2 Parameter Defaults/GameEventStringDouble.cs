using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, double).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Double Event",
        menuName = "JT Framework/Game Events/2 Parameter/String Double")]
    public class GameEventStringDouble : GameEvent<string, double> { }
}
