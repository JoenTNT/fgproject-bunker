using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

namespace JT.FGP.Tests
{
    /// <summary>
    /// Testing job handler on something using sample.
    /// </summary>
    public class Test_JobHandler : MonoBehaviour
    {
        [SerializeField]
        private Test_SampleAnimationCurve[] _samples = new Test_SampleAnimationCurve[0];

        // Runtime variable data.
        private List<Test_AnimationCurveJob> _jobs = null;

        private void Start()
        {
            if (_jobs == null) _jobs = new List<Test_AnimationCurveJob>();

            foreach (var sample in _samples)
            {
                _jobs.Add(sample.CreateJob());
            }
        }

        private void Update()
        {
            
        }
    }
}
