using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, string, UnityEngine.Object).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String String UnityObject Event",
        menuName = "JT Framework/Game Events/3 Parameter/String String UnityObject")]
    public class GameEventTwoStringUnityObject : GameEvent<string, string, Object> { }
}
