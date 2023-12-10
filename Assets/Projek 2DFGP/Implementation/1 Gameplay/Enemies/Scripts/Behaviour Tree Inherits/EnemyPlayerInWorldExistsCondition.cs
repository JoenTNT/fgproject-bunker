using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP
{
    // Shorten includes.
    using EC = EnemyConstants;

    /// <summary>
    /// Always receive initial player before doing something.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Enemy Player In World Exists Action",
        menuName = "FGP/AI/Enemy Player In World Exists Action")]
    public class EnemyPlayerInWorldExistsCondition : BT_Conditional
    {
        #region Variables

        [Header("Settings")]
        [SerializeField]
        [Tooltip("If this is set true then the result will be inversed. " +
            "(Failed -> Success; Success -> Failed)")]
        private bool _inverseResult = false;

        // Initial variable references.
        private BakedDashboard _dashboard = null;
        private BakedParamTransform _chaseTargetParam = null;

        // Runtime variable data.
        private PlayerEntity _mainPlayer = null;

        #endregion

        #region BT Conditional

        public override BT_State CalcStateCondition()
        {
            // Get player in world.
            _mainPlayer = PlayerSpawner.MainPlayer;
            if (_mainPlayer != null)
            {
                _chaseTargetParam.Value = _mainPlayer.transform;
                return _inverseResult ? BT_State.Success : BT_State.Failed;
            }

            // By Default will return failed.
            return _inverseResult ? BT_State.Failed : BT_State.Success;
        }

        public override void OnInit()
        {
            // Get target dashboard.
            _dashboard = GlobalDPContainer.GetDashboard(ObjectRef.GetInstanceID());

            // Get all references.
            var @params = _dashboard.Parameters;
            _chaseTargetParam = (BakedParamTransform)@params[EC.CHASE_TARGET_KEY];
        }
#if UNITY_EDITOR
        public override Dictionary<string, string> GetVariableKeys()
            => new Dictionary<string, string>() {
                { EC.CHASE_TARGET_KEY, typeof(ParamTransform).AssemblyQualifiedName },
            };
#endif
        #endregion
    }
}