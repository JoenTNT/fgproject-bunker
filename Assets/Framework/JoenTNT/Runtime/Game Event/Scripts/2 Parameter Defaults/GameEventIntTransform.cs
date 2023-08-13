using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(int, GameObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Int Transform Event",
        menuName = "JT Framework/Game Events/2 Parameter/Int Transform")]
    public class GameEventIntTransform : GameEvent<int, Transform> { }
}
