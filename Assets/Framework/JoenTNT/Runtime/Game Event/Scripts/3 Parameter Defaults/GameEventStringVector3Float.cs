using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, Vector3, float).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Vector3 Float Event",
        menuName = "JT Framework/Game Events/3 Parameter/String Vector3 Float")]
    public class GameEventStringVector3Float : GameEvent<string, Vector3, float> { }
}
