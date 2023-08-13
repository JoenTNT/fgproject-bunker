using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (object).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Object Event",
        menuName = "JT Framework/Game Events/1 Parameter/Object")]
    public class GameEventObject : GameEvent<object> { }
}
