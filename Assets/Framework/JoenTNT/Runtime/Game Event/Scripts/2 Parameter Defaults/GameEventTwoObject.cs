using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(object, object).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Game Event Object Object",
        menuName = "JT Framework/Game Events/2 Parameter/Object Object")]
    public class GameEventTwoObject : GameEvent<object, object> { }
}
