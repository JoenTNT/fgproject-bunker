using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, byte, GameObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Byte GameObject Event",
        menuName = "JT Framework/Game Events/3 Parameter/String Byte GameObject")]
    public class GameEventStringByteGameObject : GameEvent<string, byte, GameObject> { }
}
