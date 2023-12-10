using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(GameObject, UnityObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New GameObject UnityObject Event",
        menuName = "JT Framework/Game Events/2 Parameter/GameObject UnityObject")]
    public class GameEventGameObjectUnityObject : GameEvent<GameObject, Object> { }
}
