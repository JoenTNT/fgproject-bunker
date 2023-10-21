using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, GameObject, UnityObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String GameObject UnityObject Event",
        menuName = "JT Framework/Game Events/3 Parameter/String GameObject UnityObject")]
    public class GameEventStringGameObjectUnityObject : GameEvent<string, GameObject, Object> { }
}
