using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 4 parameters(string, int, float, Vector3).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Int Float Vector3 Event",
        menuName = "JT Framework/Game Events/4 Parameter/String Int Float Vector3")]
    public class GameEventStringIntFloatVector3 : GameEvent<string, int, float, Vector3> { }
}
