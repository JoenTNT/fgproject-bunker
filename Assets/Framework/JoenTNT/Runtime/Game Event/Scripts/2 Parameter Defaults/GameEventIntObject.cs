using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(int, object).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Game Event Int Object",
        menuName = "JT Framework/Game Events/2 Parameter/Int Object")]
    public class GameEventIntObject : GameEvent<int, object> { }
}
