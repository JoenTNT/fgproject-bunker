using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle shooter function of a weapon gun.
    /// </summary>
    public class Shooter2DFunc : MonoBehaviour, IShootCommand<Bullet2DControl>
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Transform _shootPoint = null;

        [Header("Properties")]
        [SerializeField]
        private float _shootForce = 100f; // Unit per second.

        [Tooltip("The wider the angle, the more inaccurate shots.")]
        [SerializeField, Min(0f)] // Max is 180 degree facing to the front.
        private float _accuracyAngle = 0; // In degree.
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField]
        private float _maxLengthDrawRay = 10f; // In Unit.
#endif
        #endregion

        #region Mono
#if UNITY_EDITOR
        private void OnValidate()
        {
            // Fix max value.
            if (_accuracyAngle > 180f)
                _accuracyAngle = 180f;
        }

        private void OnDrawGizmosSelected()
        {
            // Initialize values.
            Vector2 right = transform.right;
            float max = _accuracyAngle / 2f, min = -max;

            // Draw max angle shot.
            Debug.DrawRay(transform.position, DirectionChangeByDegree(right, max) * _maxLengthDrawRay,
                Color.cyan);

            // Draw min angle shot.
            Debug.DrawRay(transform.position, DirectionChangeByDegree(right, min) * _maxLengthDrawRay,
                Color.cyan);
        }
#endif
        #endregion

        #region IShootCommand

        public void Shoot(Bullet2DControl ammo)
        {
            // Calculate facing forward of the weapon, set initial meta, and then shoot the bullet.
            ammo.transform.position = _shootPoint.position;
            ammo.Reset(); // Always reset the info.

            // Calculate inaccuracy.
            float maxAngle = _accuracyAngle / 2f;
            float angleShot = Random.Range(-maxAngle, maxAngle + 1f);

            // Shoot the ammo.
            ammo.Shoot(DirectionChangeByDegree(transform.right, angleShot), _shootForce);
        }

        #endregion

        #region Main

        private static Vector2 DirectionChangeByDegree(Vector2 dir, float rotateDegree)
        {
            // Convert degrees to radians because Unity uses radians for rotations
            float radians = rotateDegree * Mathf.Deg2Rad;

            // Calculate the cosine and sine of the rotation angle
            float cosAngle = Mathf.Cos(radians);
            float sinAngle = Mathf.Sin(radians);

            // Apply the rotation matrix to the direction vector
            float newX = dir.x * cosAngle - dir.y * sinAngle;
            float newY = dir.x * sinAngle + dir.y * cosAngle;

            // Return the rotated direction
            return new Vector2(newX, newY);
        }

        #endregion
    }
}
