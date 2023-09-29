using System.Collections.Generic;
using UnityEngine;

namespace JT
{
    /// <summary>
    /// Containing data of blackboards.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Dashboard : MonoBehaviour, IBake<BakedDashboard>
    {
        #region structs

        /// <summary>
        /// Helps editor to choose type and data before serialize values.
        /// </summary>
        [System.Serializable]
        private struct ParamPair
        {
            #region Variables

            [SerializeField]
            private string _paramType;

            [SerializeField]
            private string _paramName;

            [SerializeField]
            private Parameter _parameter;
#if UNITY_EDITOR
            [SerializeField, HideInInspector]
            private bool _foldoutParameter;

            [SerializeField, HideInInspector]
            private bool _isValid;
#endif
            #endregion

            #region Properties

            /// <summary>
            /// Name of parameter.
            /// </summary>
            public string ParameterName => _paramName;

            /// <summary>
            /// Parameter reference.
            /// </summary>
            public Parameter Parameter => _parameter;

            #endregion
        }

        #endregion

        #region Variables

        [SerializeField]
        private ParamPair[] _paramPairs = new ParamPair[0];

        [SerializeField]
        private GameObject _targetObject = null;

        [SerializeField]
        private bool _destroyObjectOnBake = false;

        // Runtime variable data.
        private List<IBakedObject> _bakeList = new();

        #endregion

        #region IBake

        /// <summary>
        /// Bake all data and send it to the container, then delete the component after baking.
        /// WARNING: Do not run this method before all bake informations registered.
        /// </summary>
        public BakedDashboard Bake()
        {
            // Change to parameters dictionary.
            Dictionary<string, BakedParameter> paramDict= new();
            foreach (var pp in _paramPairs)
                paramDict[pp.ParameterName] = pp.Parameter.BakeParam();

            // Assign all information to baking list.
            foreach (var bake in _bakeList)
                bake.Bake(paramDict);

            // Create baked form.
            BakedDashboard baked = new BakedDashboard(_targetObject, paramDict);
            paramDict = null; // Release memory.

            // Always delete component, or object if should.
            if (_destroyObjectOnBake) Destroy(gameObject);
            else Destroy(this);

            // Always send to container, then return baked form.
            GlobalDPContainer.Instance.RegisterDashboard(baked);
            return baked;
        }

        #endregion

        #region Main

        /// <summary>
        /// Run this method to register list of bake informations.
        /// </summary>
        /// <param name="bake"></param>
        public void RegisterBeforeBake(IBakedObject bake) => _bakeList.Add(bake);

        #endregion
    }

}
