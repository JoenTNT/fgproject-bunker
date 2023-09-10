using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle FX control for any kinds of splatter.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class Splatter2DFXControl : MonoBehaviour, ISplat<float>
    {
        #region Variables

        [Header("Properties")]
        [SerializeField, Min(1)]
        private int _minParticle = 1;

        [SerializeField, Min(1)]
        private int _maxParticle = 10;

        // Runtime variable data.
        private ParticleSystem _particleSystem = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Get particle system component.
            TryGetComponent(out _particleSystem);

            // Swap if min and max is switched.
            if (_maxParticle < _minParticle)
            {
                var temp = _minParticle;
                _minParticle = _maxParticle;
                _maxParticle = temp;
            }
        }

        //private void Update()
        //{
        //    // TEMP: Left click to splat.
        //    if (Input.GetMouseButtonDown(0))
        //        Splat(Vector2.zero);
        //}

        #endregion

        #region ISplat

        /// <param name="direction">2D degree direction.</param>
        public void Splat(float direction)
        {
            // Set splat direction.
            ParticleSystem.ShapeModule shape = _particleSystem.shape;
            Vector3 rotation = _particleSystem.shape.rotation;
            rotation.x = direction;
            shape.rotation = rotation;

            // Burst emit particles.
            _particleSystem.Emit(Random.Range(_minParticle, _maxParticle));
        }

        #endregion
    }
}
