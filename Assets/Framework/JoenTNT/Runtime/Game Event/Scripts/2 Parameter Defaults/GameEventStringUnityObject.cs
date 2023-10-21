using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, UnityObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String UnityObject Event",
        menuName = "JT Framework/Game Events/2 Parameter/String UnityObject")]
    public class GameEventStringUnityObject : GameEvent<string, Object> { }
}
