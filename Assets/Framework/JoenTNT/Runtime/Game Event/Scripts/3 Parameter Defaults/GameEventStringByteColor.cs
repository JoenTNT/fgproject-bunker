using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, byte, Color).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Byte Color Event",
        menuName = "JT Framework/Game Events/3 Parameter/String Byte Color")]
    public class GameEventStringByteColor : GameEvent<string, byte, Color> { }
}
