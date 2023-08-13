using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(string, byte).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Byte Event",
        menuName = "JT Framework/Game Events/2 Parameter/String Byte")]
    public class GameEventStringByte : GameEvent<string, byte> { }
}
