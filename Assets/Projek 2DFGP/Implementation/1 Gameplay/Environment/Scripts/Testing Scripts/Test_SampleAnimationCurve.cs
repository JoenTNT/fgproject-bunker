using Unity.Collections;
using UnityEngine;

namespace JT.FGP.Tests
{
    public class Test_SampleAnimationCurve : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [SerializeField]
        private bool _runUpdate = true;

        // Runtime variable data.
        private float _currentSecond = 0f;

        private void Update()
        {
            if (!_runUpdate) return;

            _currentSecond += Time.deltaTime;
            _curve.Evaluate(_currentSecond);

            if (_currentSecond > _curve.keys[_curve.keys.Length - 1].time)
                _currentSecond = 0f;
        }

        public Test_AnimationCurveJob CreateJob()
        {
            NativeArray<Keyframe> n = new NativeArray<Keyframe>(_curve.keys, Allocator.Persistent);
            var job = new Test_AnimationCurveJob(n);
            return job;
        }
    }
}