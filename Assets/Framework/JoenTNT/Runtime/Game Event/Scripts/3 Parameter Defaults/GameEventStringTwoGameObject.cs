using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, GameObject, GameObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String GameObject GameObject Event",
        menuName = "JT Framework/Game Events/3 Parameter/String GameObject GameObject")]
    public class GameEventStringTwoGameObject : GameEvent<string, GameObject, GameObject> { }
}
