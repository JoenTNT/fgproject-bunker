using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(UnityObject, UnityObject, UnityObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New UnityObject UnityObject UnityObject Event",
        menuName = "JT Framework/Game Events/3 Parameter/UnityObject UnityObject UnityObject")]
    public class GameEventThreeUnityObject : GameEvent<Object, Object, Object> { }
}
