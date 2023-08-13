using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 3 parameters(string, byte, UnityEngine.Object).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Byte UnityObject Event",
        menuName = "JT Framework/Game Events/3 Parameter/String Byte UnityObject")]
    public class GameEventStringByteUnityObject : GameEvent<string, byte, Object> { }
}
