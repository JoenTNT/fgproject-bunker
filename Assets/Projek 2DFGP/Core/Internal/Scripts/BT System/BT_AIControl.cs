/* 
 * Copyright © 2023 JoenTNT All Rights Reserved
 * 
 * JT's Unity SOA Behaviour Tree
 * 
 * Main control of behaviour tree.
 */

using UnityEngine;

namespace JT
{
    /// <summary>
    /// Behaviour trees system control.
    /// </summary>
    [DisallowMultipleComponent]
    public class BT_AIControl : MonoBehaviour, IBTActivation, IBTInitHandler
    {
        #region Variables

        [Header("Base System")]
        [SerializeField]
        private BT_Execute _rootExecute = null;

        // Runtime variable data.
        private BT_Execute _currentExecution = null;
        private IBTProcessHolder _processHolder = null;
        private IBTRuntimeAction _runtimeAction = null;
        private bool _isInit = false;

        #endregion

        #region Mono

        private void Start() => OnStartBehaviour();

        private void OnEnable()
        {
            // Check if behaviour not yet initialized.
            if (!_isInit)
            {
                // Bake dashboard and copy processor.
                _rootExecute = _rootExecute.GetCopy(gameObject);

                // Check root executor.
                if (_rootExecute is IBTProcessHolder)
                    _processHolder = (IBTProcessHolder)_rootExecute;
                if (_rootExecute is IBTRuntimeAction)
                    _runtimeAction = (IBTRuntimeAction)_rootExecute;

                // Call on init.
                OnInit();

                // Set status to initialized.
                _isInit = true;
            }

            // Run single action.
            if (_processHolder == null && _runtimeAction != null)
                _runtimeAction.OnBeforeAction();

            // Run enable behaviour.
            OnEnableBehaviour();
        }

        private void Update()
        {
            // If process holder not exists.
            if (_processHolder == null) goto RunSingleAction;

            // Check if processor is not holding.
            if (!_processHolder.IsExecutionHold)
            {
                // Execute processor.
                _rootExecute.Execute();

                // Check for any action running.
                if (_rootExecute.IsAction)
                    _currentExecution = _rootExecute;
                else if (_rootExecute is IBTTrunkNode)
                    _currentExecution = ((IBTTrunkNode)_rootExecute).GetCurrentLeafProcess();

                // Convert to action.
                if (_currentExecution is IBTRuntimeAction)
                {
                    // Check previous action exit.
                    IBTRuntimeAction newAction = (IBTRuntimeAction)_currentExecution;
                    if (_runtimeAction != null && _runtimeAction != newAction)
                        _runtimeAction.OnAfterAction();

                    // Change to new action.
                    _runtimeAction = newAction;
                    _runtimeAction.OnBeforeAction();
                }
            }

            // Always check state before running action.
            if (_currentExecution.State == BT_State.Running) goto RunSingleAction;

            // Release holder if not running.
            _processHolder.ReleaseHolder();
            return;

        RunSingleAction:
            // Check if there's runtime action running, if not the abort process.
            if (_runtimeAction == null || _runtimeAction.IsDone) return;

            // Run current action running.
            _runtimeAction.OnTickAction();
        }

        private void OnDisable()
        {
            // Run single action.
            if (_processHolder == null && _runtimeAction != null)
                _runtimeAction.OnAfterAction();

            // Run disable behaviour.
            OnDisableBehaviour();
        }

        private void OnDestroy() => OnEndBehaviour();

        #endregion

        #region IBTActivation

        public virtual void OnStartBehaviour() { }

        public virtual void OnEndBehaviour() { }
        
        public virtual void OnEnableBehaviour() { }

        public virtual void OnDisableBehaviour() { }

        #endregion

        #region IBTInitHandler

        /// <summary>
        /// Called before initializing behaviour tree containing processor and data.
        /// </summary>
        public virtual void OnInit() => _rootExecute.OnInit();

        #endregion
    }
}
