using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, string, byte).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String String Byte Event",
        menuName = "JT Framework/Game Events/3 Parameter/String String Byte")]
    public class GameEventTwoStringByte : GameEvent<string, string, byte> { }
}
