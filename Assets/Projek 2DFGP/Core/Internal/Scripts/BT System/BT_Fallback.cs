/* 
 * Copyright © 2023 JoenTNT All Rights Reserved
 * 
 * JT's Unity SOA Behaviour Tree
 * 
 * Fallback nodes execute children in order until one of them returns Success or all children return Failure.
 * These nodes are key in designing recovery behaviors for your autonomous agents.
 */

using System.Collections.Generic;
using UnityEngine;

namespace JT
{
    /// <summary>
    /// Handle behaviour tree condition checker fallback.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Behaviour Tree Fallback",
        menuName = "JT Framework/Behaviour Tree/BT Fallback")]
    public class BT_Fallback : BT_Execute, IBTTrunkNode, IBTProcessHolder
    {
        #region Variables

        [SerializeField]
        private BT_State _onEmptySequenceState = BT_State.None;

        [SerializeField]
        private BT_Execute[] _sequences = new BT_Execute[0];

        // Runtime variable data.
        private BT_Execute _tempExecute = null;
        private int _currentIndex = -1;
        private bool _holdRun = false;

        #endregion

        #region BT Execute

        public sealed override bool IsAction => false;

        public override void Execute()
        {
            // Check empty sequence, then ignore fallback.
            if (_sequences == null || _sequences.Length == 0)
            {
                State = _onEmptySequenceState;
                return;
            }

            // Set initial state.
            State = BT_State.Running;

            // Always add one when begin, check exceeding index, reset back to start index.
            if (_currentIndex < 0) _currentIndex++;
            else if (_sequences[_currentIndex].State == BT_State.Failed) _currentIndex++;
            else if (_sequences[_currentIndex].State == BT_State.Success)
            {
                State = BT_State.Failed;
                _currentIndex = -1;
                return;
            }

            // Loop through all executions.
            int sequenceLen = _sequences.Length;
            while (_currentIndex < sequenceLen)
            {
                // Execute sequence.
                _tempExecute = _sequences[_currentIndex];
                _tempExecute.Execute();

                switch (_tempExecute.State)
                {
                    // At least one successful process state fulfill the fallback.
                    case BT_State.Success:
                        State = BT_State.Success;
                        _holdRun = false;
                        goto CheckBroker;

                    // Check if running onto action, then pause the process.
                    case BT_State.Running when _tempExecute.IsAction:
                    case BT_State.Running when _tempExecute is IBTProcessHolder && ((IBTProcessHolder)_tempExecute).IsExecutionHold:
                        _holdRun = true;
                        goto CheckBroker;
                }

                // Next process.
                _currentIndex++;
            }

            // Check index is final, then reset sequence as failed.
            if (_currentIndex >= sequenceLen)
            {
                State = BT_State.Failed;
                _holdRun = false;
            }

            // Reset back to zero if only the execution is not being hold.
        CheckBroker:
            if (!_holdRun) _currentIndex = -1;
#if UNITY_EDITOR
            if (DebugMode)
                Debug.Log($"[DEBUG] Fallback Result, State: {State}; Latest Execution " +
                    $"{_tempExecute} ({this})", ObjectRef);
#endif
        }

        public override BT_Execute GetCopy(GameObject objRef)
        {
            // Duplicate all child processes first.
            var dupe = (BT_Fallback)base.GetCopy(objRef);
            dupe._sequences = new BT_Execute[_sequences.Length];
            for (int i = 0; i < _sequences.Length; i++)
                dupe._sequences[i] = _sequences[i].GetCopy(objRef);
            return dupe;
        }

        public override void OnInit()
        {
            // On initialize all children sequences.
            foreach (var seq in _sequences)
                seq.OnInit();
        }
#if UNITY_EDITOR
        public override Dictionary<string, string> GetVariableKeys()
        {
            Dictionary<string, string> k = new();
            int len = _sequences.Length;
            for (int i = 0; i < len; i++)
            {
                var childKeys = _sequences[i].GetVariableKeys();
                if (childKeys == null) continue;
                foreach (var ck in childKeys)
                    k[ck.Key] = ck.Value;
            }
            return k;
        }
#endif
        #endregion

        #region IBTTrunkNode

        public int NodeIndex => _currentIndex;

        public BT_Execute GetCurrentProcess() => _sequences[_currentIndex];

        public BT_Execute GetCurrentLeafProcess()
        {
            BT_Execute leafProcess = GetCurrentProcess();
            while (leafProcess is IBTTrunkNode)
                leafProcess = ((IBTTrunkNode)leafProcess).GetCurrentProcess();
            return leafProcess;
        }

        #endregion

        #region IBTProcessHolder

        public bool IsExecutionHold => _holdRun;

        public void ReleaseHolder()
        {
            _holdRun = false;
            _tempExecute = GetCurrentProcess();
            if (_tempExecute is IBTProcessHolder)
                ((IBTProcessHolder)_tempExecute).ReleaseHolder();
        }

        #endregion
    }
}
