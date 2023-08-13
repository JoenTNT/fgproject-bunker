using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter(Component).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Component Event",
        menuName = "JT Framework/Game Events/1 Parameter/Component")]
    public class GameEventComponent : GameEvent<Component> { }
}
