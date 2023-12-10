using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP
{
    // Shorten includes.
    using EC = EnemyConstants;

    /// <summary>
    /// Handle attacking target.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Enemy Shoot Action",
        menuName = "FGP/AI/Enemy Shoot Action")]
    public class EnemyShootAction : BT_Action
    {
        #region Variables

        // Initial variable references.
        private BakedDashboard _dashboard = null;
        private BakedParamString _shootAmmoTypeParam = null;
        private BakedParamTransform _chaseTargetParam = null;
        private BakedParamFloat _afterAttackRestSecondParam = null;
        private BakedParamFloat _attackAnticipationSecondParam = null;
        private EntityID _ownerID = null;
        private DampingRotation2DFunc _rotateFunc = null;
        private Shooter2DFunc _shooterFunc = null;
        private Animator _animator = null;
        private BakedParamString _attackTypeKeyParam = null;

        // Runtime variable data.
        private GameObjectPool _ammoPool = null;
        private GameObject _bulletObj = null;
        private PhysicalAmmo2DControl _bullet = null;
        private string _usingBulletType = string.Empty;
        private float _tempSecond = 0f;
        private bool _isShootDone = false;

        #endregion

        #region BT Action

        public override void OnInit()
        {
            // Get target dashboard.
            _dashboard = GlobalDPContainer.GetDashboard(ObjectRef.GetInstanceID());

            // Get all references.
            var @params = _dashboard.Parameters;
            _shootAmmoTypeParam = (BakedParamString)@params[EC.SHOOTER_AMMO_TYPE_KEY];
            _chaseTargetParam = (BakedParamTransform)@params[EC.CHASE_TARGET_KEY];
            _afterAttackRestSecondParam = (BakedParamFloat)@params[EC.AFTER_ATTACK_REST_SECOND_KEY];
            _attackAnticipationSecondParam = (BakedParamFloat)@params[EC.ATTACK_ANTICIPATION_SECOND_KEY];
            _shooterFunc = (Shooter2DFunc)@params[EC.SHOOTER_FUNCTION_KEY];
            _rotateFunc = (DampingRotation2DFunc)@params[EC.ROTATION_FUNCTION_KEY];
            _ownerID = (EntityID)@params[EC.OWNER_ID_KEY];
            _animator = (Animator)@params[EC.ANIMATOR_KEY];
            _attackTypeKeyParam = (BakedParamString)@params[EC.ANIM_PARAM_ATTACK_TYPE_KEY];
        }

        public override void OnBeforeAction()
        {
            // Initialize requirements.
            if (_ammoPool == null || _usingBulletType != _shootAmmoTypeParam.Value)
            {
                _usingBulletType = _shootAmmoTypeParam.Value;
                _ammoPool = GameObjectPoolManager.Instance.GetGameObjPool(_usingBulletType);
            }

            // Instant rotate to target.
            _rotateFunc.SetInstantLookAtPosition(_chaseTargetParam.Value.position);

            // Reset shoot attack declarations.
            _tempSecond = _attackAnticipationSecondParam.Value;
            _isShootDone = false;

            // Begin run attack animation.
            _animator.SetInteger(_attackTypeKeyParam.Value, 1);
        }

        public override void OnTickAction()
        {
            // Tick time.
            _tempSecond -= Time.deltaTime;

            // Check attack action.
            if (_tempSecond > 0f) return;

            // If yet shot ammo.
            if (!_isShootDone)
            {
                _bulletObj = _ammoPool.GetObject();
                if (_bulletObj.TryGetComponent(out _bullet))
                {
                    _bullet.EntityID = _ownerID.ID;
                    _shooterFunc.Shoot(_bullet);
                }

                // Rest after shot, set status to done.
                _tempSecond = _afterAttackRestSecondParam.Value;
                _isShootDone = true;
                return;
            }

            // After done, set status to succeed.
            State = BT_State.Success;
        }

        public override void OnAfterAction()
        {
            // End run attack animation.
            _animator.SetInteger(_attackTypeKeyParam.Value, 0);
        }
#if UNITY_EDITOR
        public override Dictionary<string, string> GetVariableKeys()
            => new Dictionary<string, string> {
                { EC.SHOOTER_AMMO_TYPE_KEY, typeof(ParamString).AssemblyQualifiedName },
                { EC.CHASE_TARGET_KEY, typeof(ParamTransform).AssemblyQualifiedName },
                { EC.AFTER_ATTACK_REST_SECOND_KEY, typeof(ParamFloat).AssemblyQualifiedName },
                { EC.ATTACK_ANTICIPATION_SECOND_KEY, typeof(ParamFloat).AssemblyQualifiedName },
                { EC.SHOOTER_FUNCTION_KEY, typeof(ParamComponent).AssemblyQualifiedName },
                { EC.ROTATION_FUNCTION_KEY, typeof(ParamComponent).AssemblyQualifiedName },
                { EC.OWNER_ID_KEY, typeof(ParamComponent).AssemblyQualifiedName },
                { EC.ANIMATOR_KEY, typeof(ParamComponent).AssemblyQualifiedName },
                { EC.ANIM_PARAM_ATTACK_TYPE_KEY, typeof(ParamString).AssemblyQualifiedName },
            };
#endif
        #endregion
    }
}

