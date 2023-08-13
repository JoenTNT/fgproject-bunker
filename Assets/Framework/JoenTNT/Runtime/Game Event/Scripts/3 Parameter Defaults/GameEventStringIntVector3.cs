using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, int, Vector3).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Int Vector3 Event",
        menuName = "JT Framework/Game Events/3 Parameter/String Int Vector3")]
    public class GameEventStringIntVector3 : GameEvent<string, int, Vector3> { }
}
