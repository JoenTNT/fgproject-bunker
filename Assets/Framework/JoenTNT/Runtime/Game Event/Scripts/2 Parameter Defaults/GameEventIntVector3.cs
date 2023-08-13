using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(int, Vector3).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Vector3 Event",
        menuName = "JT Framework/Game Events/2 Parameter/Int Vector3")]
    public class GameEventIntVector3 : GameEvent<int, Vector3> { }
}
