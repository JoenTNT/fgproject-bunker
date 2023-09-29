using UnityEngine;

namespace JT.FGP
{
    // Shorten includes.
    using EC = EnemyConstants;

    /// <summary>
    /// Wandering around system process for enemy.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Enemy Wandering Action",
        menuName = "FGP/AI/Enemy Wandering Action")]
    public class EnemyWanderingAction : BT_Action
    {
        #region Variables

        // Initial variable references.
        private BakedDashboard _dashboard = null;
        private BakedParamTransform _targetParam = null;
        private BakedParamVector2 _idleSecondsRangeParam = null;
        private BakedParamVector2 _wanderingPivotPositionParam = null;
        private BakedParamFloat _maxWanderingRadiusParam = null;
        private Topview2DMovementFunc _movementFunc = null;
        private Topview2DLookAtFunc _lookAtFunc = null;

        // Runtime variable data.
        private Vector2 _targetMovePosition = Vector2.zero;
        private float _tempIdleSeconds = 0f;

        #endregion

        #region BT Action

        public override void OnInit()
        {
            // Get target dashboard.
            _dashboard = GlobalDPContainer.GetDashboard(ObjectRef.GetInstanceID());

            // Get all references.
            var @params = _dashboard.Parameters;
            _targetParam = (BakedParamTransform)@params[EC.CHASE_TARGET_KEY];
            _idleSecondsRangeParam = (BakedParamVector2)@params[EC.IDLE_SECONDS_RANGE_KEY];
            _wanderingPivotPositionParam = (BakedParamVector2)@params[EC.WANDERING_PIVOT_POSITION_KEY];
            _maxWanderingRadiusParam = (BakedParamFloat)@params[EC.MAX_WANDERING_RADIUS_KEY];
            _movementFunc = (Topview2DMovementFunc)@params[EC.MOVEMENT_FUNCTION_KEY];
            _lookAtFunc = (Topview2DLookAtFunc)@params[EC.LOOK_AT_FUNCTION_KEY];
        }

        public override BT_State CalcStateCondition()
        {
            // Check if current target chase.
            return _targetParam.Value == null ? BT_State.Running : BT_State.Failed;
        }

        public override void OnBeforeAction()
        {
            // Set wandering pivot initial.
            _targetMovePosition = _wanderingPivotPositionParam.Value = ObjectRef.transform.position;
            _movementFunc.Direction = Vector2.zero;

            // Always reset idle time on start.
            ResetIdle();
        }

        public override void OnTickAction()
        {
            // Handle walking to target.
            HandleWalking();

            // Handle idle.
            if (HandleTickIdle()) return;

            // Get target walk position.
            _targetMovePosition = GetRandWanderTargetPosition();

            // Reset Idle time.
            ResetIdle();
#if UNITY_EDITOR
            Debug.Log($"[DEBUG] Walk to position {_targetMovePosition}, now wait for more {_tempIdleSeconds}");
#endif
        }

        #endregion

        #region Main

        /// <summary>
        /// Handle idle action.
        /// </summary>
        /// <returns>Enemy status is still idling</returns>
        private bool HandleTickIdle()
        {
            _tempIdleSeconds -= Time.deltaTime;
            return _tempIdleSeconds > 0f;
        }

        private void HandleWalking()
        {
            // Do move function.
            _movementFunc.Move();
        }

        private void ResetIdle()
        {
            // Get starting how long will the enemy idle.
            _tempIdleSeconds = GetRandIdleSeconds();
        }

        private Vector2 GetRandWanderTargetPosition()
        {
            Vector2 pivotPos = _wanderingPivotPositionParam.Value;
            Vector2 addByRadius = Random.insideUnitCircle * _maxWanderingRadiusParam.Value;
            return pivotPos + addByRadius;
        }

        private float GetRandIdleSeconds()
        {
            return Random.Range(_idleSecondsRangeParam.Value.x, _idleSecondsRangeParam.Value.y);
        }

        #endregion
    }
}
