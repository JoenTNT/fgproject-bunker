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

        [Header("Settings")]
        [SerializeField]
        private BT_State _onReachTarget = BT_State.Success;

        [SerializeField]
        private BT_State _onCompleteLostTarget = BT_State.Failed;

        // Initial variable references.
        private BakedDashboard _dashboard = null;
        private BakedParamFloat _chaseSpeedParam = null;
        private BakedParamTransform _chaseTargetParam = null;
        private DampingRotation2DFunc _rotateFunc = null;
        private BakedParamVector2 _moveTargetPosParam = null;
        private BakedParamFloat _attackRangeParam = null;
        private NavMeshAgent _agent = null;
        private InsideAreaObjectCollector2D _targetCollector = null;
        private BakedParamLayerMask _blockerLayer = null;

        // Runtime variable data.
        private System.Func<Vector2, GameObject, bool> _isBlockedFunc = null;
        private GameObject _tempNearest = null;
        private RaycastHit2D[] _hits = new RaycastHit2D[1];
        private Vector2 _thisObjPos = Vector2.zero;
        private Vector2 _targetsPos = Vector2.zero;
        private int _blockedCount = 0;
        private bool _isTargetLost = false;

        #endregion

        #region BT Action

        public override void OnInit()
        {
            // Get target dashboard.
            _dashboard = GlobalDPContainer.GetDashboard(ObjectRef.GetInstanceID());

            // Get all references.
            var @params = _dashboard.Parameters;
            _chaseTargetParam = (BakedParamTransform)@params[EC.CHASE_TARGET_KEY];
            _moveTargetPosParam = (BakedParamVector2)@params[EC.MOVE_TARGET_POSITION_KEY];
            _chaseSpeedParam = (BakedParamFloat)@params[EC.CHASE_SPEED_KEY];
            _rotateFunc = (DampingRotation2DFunc)@params[EC.ROTATION_FUNCTION_KEY];
            _agent = (NavMeshAgent)@params[EC.NAVMESH_AGENT_KEY];
            _targetCollector = (InsideAreaObjectCollector2D)@params[EC.INSIDE_FOV_AREA_KEY];
            _blockerLayer = (BakedParamLayerMask)@params[EC.BLOCKER_LAYER_KEY];
            _attackRangeParam = (BakedParamFloat)@params[EC.ATTACK_RANGE_KEY];

            // Set initial data.
            _isBlockedFunc = IsTargetBlocked;
        }

        public override void OnBeforeAction()
        {
            // Set initial values.
            _agent.speed = _chaseSpeedParam.Value;
            _agent.stoppingDistance = _attackRangeParam.Value;
            _isTargetLost = false;
        }

        public override void OnTickAction()
        {
            // Check for lost chase target.
            if (_isTargetLost)
            {
                OnCheckNearestTarget();
                OnTargetLost(_moveTargetPosParam);
                return;
            }

            // Chase the target.
            ChaseTarget(_chaseTargetParam);

            // Check if enemy reach the target.
            if (IsArrived(_agent))
            {
                State = _onReachTarget;
                return;
            }

            // Check new target but nearer than the current target.
            if (!_targetCollector.HasObject)
            {
                SetTargetIsLost();
                return;
            }

            // Always check for nearest target.
            OnCheckNearestTarget();
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
            _rotateFunc.SetTargetLookDirection(targetPos - thisObjPos);
            _rotateFunc.OnRotate();
        }

        private bool IsTargetBlocked(Vector2 fromPos, GameObject target)
        {
            // Check blocked new entity target.
            _targetsPos = target.transform.position;
            Vector2 dir = _targetsPos - fromPos;
            float distance = Vector2.Distance(fromPos, _targetsPos);
            _blockedCount = Physics2D.RaycastNonAlloc(fromPos, dir.normalized, _hits,
                distance, _blockerLayer.Value);

            // Ignore if the target is blocked by something.
            if (_blockedCount > 0) return false;

            // Change target.
            _chaseTargetParam.Value = target.transform;
            return true;
        }

        private void OnCheckNearestTarget()
        {
            // Get nearest target.
            _thisObjPos = ObjectRef.transform.position;
            _tempNearest = _targetCollector.GetNearestObject(_thisObjPos, _isBlockedFunc);

            // Ignore the same object.
            if (_tempNearest == _chaseTargetParam.Value) return;

            // Case: Lost the target.
            if (_tempNearest == null)
            {
                if (!_isTargetLost)
                    SetTargetIsLost();
                return;
            }

            // Case: Found new nearest target.
            SetTargetHasBeenFound(_tempNearest.transform);
        }

        private void SetTargetIsLost()
        {
            _agent.stoppingDistance = 0f;
            _isTargetLost = true;
            _moveTargetPosParam.Value = _chaseTargetParam.Value.position;
            _chaseTargetParam.Value = null;
        }

        private void SetTargetHasBeenFound(Transform newTarget)
        {
            _chaseTargetParam.Value = newTarget;
            _agent.stoppingDistance = _attackRangeParam.Value;
            _isTargetLost = false;
        }

        private void OnTargetLost(BakedParamVector2 targetLastPosition)
        {
            // Chase to last position.
            Vector2 targetPos = targetLastPosition.Value;
            _agent.SetDestination(targetPos);
            _rotateFunc.SetTargetLookDirection(_agent.desiredVelocity);
            _rotateFunc.OnRotate();

            // Check arrival.
            if (IsArrived(_agent))
                State = _onCompleteLostTarget;
        }

        #endregion

        #region Statics

        private static bool IsArrived(NavMeshAgent agent)
        {
            // Check if agent has arrived.
            return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance
                && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
        }

        #endregion
    }
}

