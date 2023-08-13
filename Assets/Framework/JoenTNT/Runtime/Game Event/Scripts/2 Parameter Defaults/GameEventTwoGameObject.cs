using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(GameObject, GameObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Game Event GameObject GameObject",
        menuName = "JT Framework/Game Events/2 Parameter/GameObject GameObject")]
    public class GameEventTwoGameObject : GameEvent<GameObject, GameObject> { }
}
