using UnityEngine;
using UnityEngine.AI;

namespace JT.FGP
{
    // Shorten includes.
    using EC = EnemyConstants;

    /// <summary>
    /// Handle enemy moving by walk action.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Enemy Walking Action",
        menuName = "FGP/AI/Enemy Walking Action")]
    public class EnemyWalkAction : BT_Action
    {
        #region Variable

        // Initial variable references.
        private BakedDashboard _dashboard = null;
        private BakedParamTransform _targetParam = null;
        private BakedParamFloat _walkSpeedParam = null;
        private BakedParamVector2 _moveTargetPosParam = null;
        //private Topview2DMovementFunc _movementFunc = null;
        private Topview2DLookAtFunc _lookAtFunc = null;
        private NavMeshAgent _agent = null;
        private InsideAreaObjectCollector2D _targetCollector = null;
        private BakedParamLayerMask _blockerLayer = null;

        // Runtime variable data.
        private GameObject _tempDetectedObj = null;
        private RaycastHit2D[] _hits = new RaycastHit2D[1];

        #endregion

        #region BT Action

        public override BT_State CalcStateCondition() => BT_State.Running;

        public override void OnInit()
        {
            // Get target dashboard.
            _dashboard = GlobalDPContainer.GetDashboard(ObjectRef.GetInstanceID());

            // Get all references.
            var @params = _dashboard.Parameters;
            _targetParam = (BakedParamTransform)@params[EC.CHASE_TARGET_KEY];
            _walkSpeedParam = (BakedParamFloat)@params[EC.WALK_SPEED_KEY];
            _moveTargetPosParam = (BakedParamVector2)@params[EC.MOVE_TARGET_POSITION_KEY];
            //_movementFunc = (Topview2DMovementFunc)@params[EC.MOVEMENT_FUNCTION_KEY];
            _lookAtFunc = (Topview2DLookAtFunc)@params[EC.LOOK_AT_FUNCTION_KEY];
            _agent = (NavMeshAgent)@params[EC.NAVMESH_AGENT_KEY];
            _targetCollector = (InsideAreaObjectCollector2D)@params[EC.INSIDE_FOV_AREA_KEY];
            _blockerLayer = (BakedParamLayerMask)@params[EC.BLOCKER_LAYER_KEY];

            // Set initial settings.
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }

        public override void OnBeforeAction()
        {
            // Set initial speed.
            _agent.speed = _walkSpeedParam.Value;
            _agent.SetDestination(_moveTargetPosParam.Value);
        }

        public override void OnTickAction()
        {
            // Handle walking algorithm.
            HandleWalking();

            // Check interuption because enemy found target.
            if (_targetCollector.HasObject)
            {
                Vector2 thisObjPos = ObjectRef.transform.position;
                _tempDetectedObj = _targetCollector.GetNearestObject(thisObjPos, IsTargetNotBlocked);

                if (_tempDetectedObj != null)
                {
                    _targetParam.Value = _tempDetectedObj.transform;
                    State = BT_State.Failed;
                    Debug.Log($"[DEBUG] Target Detected: {_tempDetectedObj}");
                    return;
                }
            }

            // Check agent is not arrived yet.
            if (!IsArrived())
            {
                // Handle look function.
                _lookAtFunc.LookAtDirection(_agent.desiredVelocity);
                return;
            }
#if UNITY_EDITOR
            //Debug.Log("[DEBUG] The Agent has Arrived!");
#endif
            // End of process.
            State = BT_State.Success;
        }

        #endregion

        #region Main

        private void HandleWalking()
        {
            // Do move function.
            // TEMPORARY: using nav mesh agent.
            //Debug.Log($"[DEBUG] Agent Velocity At {(Vector2)_agent.velocity}");
        }

        private bool IsArrived()
        {
            // Check if agent has arrived.
            return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance
                && (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f);
        }

        private bool IsTargetNotBlocked(GameObject detectedObject)
        {
            Vector2 thisObjPos = ObjectRef.transform.position;
            Vector2 targetObjPos = detectedObject.transform.position;
            Vector2 dir = targetObjPos - thisObjPos;
            float distance = Vector2.Distance(thisObjPos, targetObjPos);
            int blockedCount = Physics2D.RaycastNonAlloc(thisObjPos, dir.normalized, _hits, distance,
                _blockerLayer.Value);
#if UNITY_EDITOR
            Debug.DrawLine(thisObjPos, targetObjPos, blockedCount > 0 ? Color.red : Color.green);
#endif
            return blockedCount <= 0;
        }

        #endregion
    }
}

