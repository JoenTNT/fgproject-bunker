using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, Color).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Color Event",
        menuName = "JT Framework/Game Events/2 Parameter/String Color")]
    public class GameEventStringColor : GameEvent<string, Color> { }
}
