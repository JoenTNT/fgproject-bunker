using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, GameObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Transform Event",
        menuName = "JT Framework/Game Events/2 Parameter/String Transform")]
    public class GameEventStringTransform : GameEvent<string, Transform> { }
}
