using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace JT.FGP.Tests
{
    public struct Test_AnimationCurveJob : IJob
    {
        // Modification variable data.
        public NativeArray<Keyframe> keyframes;
        public float currentIntensityValue;
        public float currentSeconds;
        public float deltaTime;
        public bool isTurningOn; // Turning off if false.

        public void Execute()
        {
            int fromIndex = 0, toIndex = 0;
            bool found = false;
            for (int i = 0; i < keyframes.Length - 1; i++)
            {
                fromIndex = i;
                toIndex = i + 1;
                if (keyframes[i].time >= currentSeconds && keyframes[i + 1].time < currentSeconds)
                {
                    found = true;
                    break;
                }
            }

            // Cancel execution if not found.
            if (!found) return;

            // Get time distance between 2 keyframe
            Keyframe fromKey = keyframes[fromIndex];
            Keyframe toKey = keyframes[toIndex];
            float deltaTimeRange = fromKey.time - toKey.time;
            float outT, inT;

            // Check if the animation keyframe mode is weighten or tangent.
            if (fromKey.weightedMode == WeightedMode.Out)
                outT = fromKey.outWeight * deltaTimeRange;
            else
                outT = fromKey.outTangent * deltaTimeRange;
            if (toKey.weightedMode == WeightedMode.In)
                inT = toKey.inWeight * deltaTimeRange;
            else
                inT = toKey.inTangent * deltaTimeRange;

            // Calculate curve value.
            float t2 = currentSeconds * currentSeconds;
            float t3 = t2 * currentSeconds;
            float a = 2f * t3 - 3f * t2 + 1f;
            float b = t3 - 2 * t2 + currentSeconds;
            float c = t3 - t2;
            float d = -2 * t3 + 3 * t2;
            currentIntensityValue = a * fromKey.value + b * outT + c * inT + d * toKey.value;
        }

        public Test_AnimationCurveJob(NativeArray<Keyframe> keyframes)
        {
            this.keyframes = keyframes;
            currentIntensityValue = 0f;
            currentSeconds = 0f;
            deltaTime = 0f;
            isTurningOn = false;
        }
    }
}

