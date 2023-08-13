using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Handling game event with 2 parameters(float, Vector3).
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Float Vector3 Event",
        menuName = "JT Framework/Game Events/2 Parameter/Float Vector3")]
    public class GameEventFloatVector3 : GameEvent<float, Vector3> { }
}
