using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 4 parameters(string, byte, Color, GameObject).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Byte Color GameObject Event",
        menuName = "JT Framework/Game Events/4 Parameter/String Byte Color GameObject")]
    public class GameEventStringByteColorGameObject : GameEvent<string, byte, Color, GameObject> { }
}
