using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(UnityObject, UnityObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New UnityObject UnityObject Event",
        menuName = "JT Framework/Game Events/2 Parameter/UnityObject UnityObject")]
    public class GameEventTwoUnityObject : GameEvent<Object, Object> { }
}
