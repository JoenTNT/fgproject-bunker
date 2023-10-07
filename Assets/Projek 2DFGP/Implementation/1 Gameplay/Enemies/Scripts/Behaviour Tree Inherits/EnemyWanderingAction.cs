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
        private BakedParamVector2 _moveTargetPosParam = null;
        private InsideAreaObjectCollector2D _targetCollector = null;
        private BakedParamLayerMask _blockerLayer = null;

        // Runtime variable data.
        private GameObject _tempDetectedObj = null;
        private RaycastHit2D[] _hits = new RaycastHit2D[1];
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
            _moveTargetPosParam = (BakedParamVector2)@params[EC.MOVE_TARGET_POSITION_KEY];
            _targetCollector = (InsideAreaObjectCollector2D)@params[EC.INSIDE_FOV_AREA_KEY];
            _blockerLayer = (BakedParamLayerMask)@params[EC.BLOCKER_LAYER_KEY];
        }

        public override BT_State CalcStateCondition() => BT_State.Running;

        public override void OnBeforeAction()
        {
            // Set wandering pivot initial.
            _moveTargetPosParam.Value = _wanderingPivotPositionParam.Value = ObjectRef.transform.position;

            // Always reset idle time on start.
            ResetIdle();
#if UNITY_EDITOR
            //Debug.Log($"[DEBUG] Agent is Idling! Waiting for {_tempIdleSeconds}");
#endif
        }

        public override void OnTickAction()
        {
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

            // Handle idle.
            if (HandleTickIdle()) return;

            // Get target walk position.
            _moveTargetPosParam.Value = GetRandWanderTargetPosition();
#if UNITY_EDITOR
            //Debug.Log($"[DEBUG] Walk to position {_moveTargetPosParam.Value};");
#endif
            // End action process.
            State = BT_State.Success;
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
