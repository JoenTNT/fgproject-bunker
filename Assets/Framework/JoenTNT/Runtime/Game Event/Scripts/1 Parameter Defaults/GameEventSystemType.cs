using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 1 parameter (bool).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New System.Type Event",
        menuName = "JT Framework/Game Events/1 Parameter/System.Type")]
    public class GameEventSystemType : GameEvent<System.Type> { }
}
