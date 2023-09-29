/* 
 * Copyright © 2023 JoenTNT All Rights Reserved
 * 
 * JT's Unity SOA Behaviour Tree
 * 
 * Sequence nodes execute children in order until one child returns Failure or all children returns Success.
 */

using UnityEngine;

namespace JT
{
    /// <summary>
    /// Handle sequencial AI process.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Behaviour Tree Sequence",
        menuName = "JT Framework/Behaviour Tree/BT Sequence")]
    public class BT_Sequence : BT_Execute, IBTTrunkNode, IBTProcessHolder
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private BT_Execute[] _sequences = null;

        // Runtime variable data.
        private BT_Execute _tempExecute = null;
        private int _currentIndex = 0;
        private bool _loopRun = false;
        private bool _holdRun = false;

        #endregion

        #region BT Execute

        public sealed override bool IsAction => false;

        public override void Execute()
        {
            // Set initial state.
            State = BT_State.Running;
            _loopRun = true;

            while (_currentIndex < _sequences.Length)
            {
                // Execute sequence.
                _tempExecute = _sequences[_currentIndex];
                _tempExecute.Execute();

                switch (_tempExecute.State)
                {
                    // Check if failed, finish the process with failed result.
                    case BT_State.Failed:
                        State = BT_State.Failed;
                        _loopRun = false;
                        break;

                    // Check if running onto action, then pause the process.
                    case BT_State.Running when _tempExecute.IsAction:
                    case BT_State.Running when _tempExecute is IBTProcessHolder && ((IBTProcessHolder)_tempExecute).IsExecutionHold:
                        _loopRun = false;
                        _holdRun = true;
                        break;

                    // All have been successfully run.
                    case BT_State.Success when _currentIndex == _sequences.Length - 1:
                        State = BT_State.Success;
                        _loopRun = false;
                        break;
                }

                // Check loop run.
                if (!_loopRun) break;

                // Next process.
                _currentIndex++;
            }

            // Reset back to zero if only the execution is not being hold.
            if (!_holdRun) _currentIndex = 0;
        }

        public override BT_Execute GetCopy(GameObject objRef)
        {
            // Duplicate all child processes first.
            var dupe = (BT_Sequence)base.GetCopy(objRef);
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
