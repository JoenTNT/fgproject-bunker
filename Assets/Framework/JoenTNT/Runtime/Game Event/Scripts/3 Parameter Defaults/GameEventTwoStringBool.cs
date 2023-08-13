using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, string, bool).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String String Bool Event",
        menuName = "JT Framework/Game Events/3 Parameter/String String Bool")]
    public class GameEventTwoStringBool : GameEvent<string, string, bool> { }
}
