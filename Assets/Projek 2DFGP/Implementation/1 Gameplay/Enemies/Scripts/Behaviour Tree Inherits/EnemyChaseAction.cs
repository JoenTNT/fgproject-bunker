using UnityEngine;
using UnityEngine.AI;

namespace JT.FGP
{
    // Shorten includes.
    using EC = EnemyConstants;

    /// <summary>
    /// Handle chasing target.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Enemy Chase Action",
        menuName = "FGP/AI/Enemy Chase Action")]
    public class EnemyChaseAction : BT_Action
    {
        #region Variables

        // Initial variable references.
        private BakedDashboard _dashboard = null;
        private BakedParamFloat _targetFadingOutSecondsParam = null;
        private BakedParamFloat _chaseSpeedParam = null;
        private BakedParamTransform _targetParam = null;
        private Topview2DLookAtFunc _lookAtFunc = null;
        private BakedParamVector2 _moveTargetPosParam = null;
        private NavMeshAgent _agent = null;
        private InsideAreaObjectCollector2D _targetCollector = null;
        private BakedParamLayerMask _blockerLayer = null;

        // Runtime variable data.
        private GameObject _tempNearest = null;
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
            _targetFadingOutSecondsParam = (BakedParamFloat)@params[EC.TARGET_FADING_OUT_IN_SECONDS_KEY];
            _moveTargetPosParam = (BakedParamVector2)@params[EC.MOVE_TARGET_POSITION_KEY];
            _chaseSpeedParam = (BakedParamFloat)@params[EC.CHASE_SPEED_KEY];
            _lookAtFunc = (Topview2DLookAtFunc)@params[EC.LOOK_AT_FUNCTION_KEY];
            _agent = (NavMeshAgent)@params[EC.NAVMESH_AGENT_KEY];
            _targetCollector = (InsideAreaObjectCollector2D)@params[EC.INSIDE_FOV_AREA_KEY];
            _blockerLayer = (BakedParamLayerMask)@params[EC.BLOCKER_LAYER_KEY];
        }

        public override void OnBeforeAction()
        {
            // Set initial speed.
            _agent.speed = _chaseSpeedParam.Value;
            Debug.Log($"[DEBUG] Chasing Target: {_targetParam.Value}");
        }

        public override void OnTickAction()
        {
            // Chase the target.
            ChaseTarget(_targetParam);

            // Check if enemy reach the target.
            if (IsArrived())
            {
                State = BT_State.Success;
                return;
            }

            // Check new target but nearer than the current target.
            if (!_targetCollector.HasObject) return;

            // Get nearest target.
            Vector2 thisObjPos = ObjectRef.transform.position;
            _tempNearest = _targetCollector.GetNearestObject(thisObjPos);

            // Ignore the same target.
            if (_tempNearest.GetInstanceID() == _targetParam.Value.gameObject.GetInstanceID())
                return;

            // Check blocked new entity target.
            Vector2 targetObjPos = _tempNearest.transform.position;
            Vector2 dir = targetObjPos - thisObjPos;
            float distance = Vector2.Distance(thisObjPos, targetObjPos);
            int blockedFound = Physics2D.RaycastNonAlloc(thisObjPos, dir.normalized, _hits, distance, _blockerLayer.Value);

            // Ignore if the target is blocked by something.
            if (blockedFound > 0) return;

            // Change target.
            _targetParam.Value = _tempNearest.transform;
        }

        #endregion

        #region Main

        private void ChaseTarget(BakedParamTransform target)
        {
            // Ignore empty target.
            if (target.Value == null) return;

            Vector2 targetPos = target.Value.position;
            _agent.SetDestination(targetPos);

            Vector2 thisObjPos = ObjectRef.transform.position;
            _lookAtFunc.LookAtDirection(targetPos - thisObjPos);
        }

        private bool IsArrived()
        {
            // Check if agent has arrived.
            return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance
                && (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f);
        }

        #endregion
    }
}

