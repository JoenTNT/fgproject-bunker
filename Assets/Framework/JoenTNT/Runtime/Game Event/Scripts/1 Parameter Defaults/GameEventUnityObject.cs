using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (Object).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New UnityObject Event",
        menuName = "JT Framework/Game Events/1 Parameter/UnityObject")]
    public class GameEventUnityObject : GameEvent<Object> { }
}
