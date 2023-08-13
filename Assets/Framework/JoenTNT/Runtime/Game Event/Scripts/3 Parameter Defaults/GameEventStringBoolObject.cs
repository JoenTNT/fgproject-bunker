using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, bool, object).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Bool Object Event",
        menuName = "JT Framework/Game Events/3 Parameter/String Bool Object")]
    public class GameEventStringBoolObject : GameEvent<string, bool, object> { }
}
