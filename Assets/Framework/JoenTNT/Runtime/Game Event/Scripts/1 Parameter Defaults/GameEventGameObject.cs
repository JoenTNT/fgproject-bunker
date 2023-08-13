using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter(GameObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New GameObject Event",
        menuName = "JT Framework/Game Events/1 Parameter/GameObject")]
    public class GameEventGameObject : GameEvent<GameObject> { }
}
