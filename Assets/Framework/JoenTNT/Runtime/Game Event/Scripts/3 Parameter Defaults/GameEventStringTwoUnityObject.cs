using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, UnityObject, UnityObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String UnityObject UnityObject Event",
        menuName = "JT Framework/Game Events/3 Parameter/String UnityObject UnityObject")]
    public class GameEventStringTwoUnityObject : GameEvent<string, Object, Object> { }
}
