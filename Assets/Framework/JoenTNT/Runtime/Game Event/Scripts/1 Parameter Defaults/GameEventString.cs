using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (string).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Event",
        menuName = "JT Framework/Game Events/1 Parameter/String")]
    public class GameEventString : GameEvent<string> { }
}
