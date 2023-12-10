using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace JT.FGP
{
    // Shorten includes.
    using EC = EnemyConstants;

    /// <summary>
    /// Only looking around like searching surroundings.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Enemy Random Look Around Action",
        menuName = "FGP/AI/Enemy Random Look Around Action")]
    public sealed class EnemyRandomLookAroundAction : BT_Action
    {
        #region Variables

        [SerializeField]
        private BT_State _onFinishLookingAround = BT_State.Success;

        // Initial variable references.
        private BakedDashboard _dashboard = null;
        private BakedParamFloat _staringSecondParam = null;
        private BakedParamInt _randomLookCountParam = null;
        private BakedParamVector2 _moveTargetPosParam = null;
        private NavMeshAgent _agent = null;
        private DampingRotation2DFunc _rotateFunc = null;

        // Runtime variable data.
        private Transform _objTransform = null;
        private int _lookAroundCounter = 0;
        private float _secondBeforeNextLook = 0f;

        #endregion

        #region BT Action

        public override void OnInit()
        {
            // Get target dashboard.
            _dashboard = GlobalDPContainer.GetDashboard(ObjectRef.GetInstanceID());

            // Get all references.
            var @params = _dashboard.Parameters;
            _rotateFunc = (DampingRotation2DFunc)@params[EC.ROTATION_FUNCTION_KEY];
            _moveTargetPosParam = (BakedParamVector2)@params[EC.MOVE_TARGET_POSITION_KEY];
            _staringSecondParam = (BakedParamFloat)@params[EC.STARING_SECOND_KEY];
            _randomLookCountParam = (BakedParamInt)@params[EC.LOOK_AROUND_COUNT_KEY];
            _agent = (NavMeshAgent)@params[EC.NAVMESH_AGENT_KEY];

            // Set initial info.
            _objTransform = ObjectRef.transform;
        }

        public override void OnBeforeAction()
        {
            // Set initial data.
            _lookAroundCounter = _randomLookCountParam.Value;
            _secondBeforeNextLook = _staringSecondParam.Value;
            _agent.destination = _moveTargetPosParam.Value = _objTransform.position;
            _rotateFunc.SetTargetRotationDegree(_objTransform.eulerAngles.z);
        }

        public override void OnTickAction()
        {
            // Run rotation.
            _rotateFunc.OnRotate();

            // Wait tick.
            _secondBeforeNextLook -= Time.deltaTime;

            // Ignore ticking undone.
            if (_secondBeforeNextLook > 0f) return;

            // Next random target.
            RandomTargetRotateDirection(_rotateFunc);
            _lookAroundCounter--;

            // Check process is done.
            if (_lookAroundCounter > 0)
            {
                // Reset timer.
                _secondBeforeNextLook = _staringSecondParam.Value;
                return;
            }

            // End of Process.
            State = _onFinishLookingAround;
        }
#if UNITY_EDITOR
        public override Dictionary<string, string> GetVariableKeys()
            => new Dictionary<string, string>() {
                { EC.ROTATION_FUNCTION_KEY, typeof(ParamComponent).AssemblyQualifiedName },
                { EC.MOVE_TARGET_POSITION_KEY, typeof(ParamVector2).AssemblyQualifiedName },
                { EC.STARING_SECOND_KEY, typeof(ParamFloat).AssemblyQualifiedName },
                { EC.LOOK_AROUND_COUNT_KEY, typeof(ParamInt).AssemblyQualifiedName },
                { EC.NAVMESH_AGENT_KEY, typeof(ParamComponent).AssemblyQualifiedName },
            };
#endif
        #endregion

        #region Main

        private void RandomTargetRotateDirection(DampingRotation2DFunc func)
            => func.SetTargetLookDirection(Random.insideUnitCircle);

        #endregion
    }
}
