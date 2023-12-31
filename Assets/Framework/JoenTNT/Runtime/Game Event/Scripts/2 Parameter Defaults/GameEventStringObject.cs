using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, object).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Object Event",
        menuName = "JT Framework/Game Events/2 Parameter/String Object")]
    public class GameEventStringObject : GameEvent<string, object> { }
}
