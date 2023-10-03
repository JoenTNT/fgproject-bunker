using JT.GameEvents;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace JT.FGP
{
    /// <summary>
    /// Handle control lighting in game everytime something happen.
    /// </summary>
    public class Light2DControl : MonoBehaviour, IPhysicalSwitch
    {
        #region Variables

        [Header("Properties")]
        [Tooltip("Between X and Y is Daytime in gameplay. Other than that it's night time.")]
        [SerializeField]
        private Vector2 _dayTimePercent = Vector2.zero;

        [SerializeField]
        private InGameLightingType _onDaySwitchOn = InGameLightingType.Unknown;

        [SerializeField]
        private InGameLightingType _onDaySwitchOff = InGameLightingType.Primary;

        [SerializeField]
        private InGameLightingType _onNightSwitchOn = InGameLightingType.Primary;

        [SerializeField]
        private InGameLightingType _onNightSwitchOff = InGameLightingType.Unknown;

        [Header("Game Events")]
        [SerializeField]
        private GameEventUnityObject _registerLightCallback = null;

        [SerializeField]
        private GameEventFloat _onTickDayPercentage = null;

        // Runtime variable data.
        private Dictionary<InGameLightingType, List<Light2DSwitchFunc>> _lights = new();
        private bool _isPreviouslyDaytime = false;

        #endregion

        #region Mono

        private void Awake()
        {
            // Instantiate container if not yet init.
            if (_lights == null) _lights = new();
            if (_lights.Count == 0)
            {
                System.Array e = System.Enum.GetValues(typeof(InGameLightingType));
                InGameLightingType lightingType;
                for (int i = 0; i < e.Length; i++)
                {
                    lightingType = (InGameLightingType)e.GetValue(i);
                    _lights[lightingType] = new List<Light2DSwitchFunc>();
                }
            }

            // Subscribe events.
            _registerLightCallback.AddListener(ListenRegisterLightCallback);
            _onTickDayPercentage.AddListener(ListenOnTickDataPercentage);
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _registerLightCallback.RemoveListener(ListenRegisterLightCallback);
            _onTickDayPercentage.RemoveListener(ListenOnTickDataPercentage);
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            // TODO ISSUE: Y is Uncontrollable after X exceeding Y.
            if (_dayTimePercent.x > 1f)
                _dayTimePercent.x = 1f;
            if (_dayTimePercent.y < 0f)
                _dayTimePercent.y = 0f;
            if (_dayTimePercent.x > _dayTimePercent.y)
                _dayTimePercent.y = _dayTimePercent.x;
            if (_dayTimePercent.y < _dayTimePercent.x)
                _dayTimePercent.x = _dayTimePercent.y;
        }
#endif
        #endregion

        #region IPhysicalSwitch

        public bool IsTurnedOn => _isPreviouslyDaytime;

        /// <summary>
        /// It is day time and start turning the light.
        /// </summary>
        public void TurnOn()
        {
            foreach (var ltk in _lights.Keys)
            {
                // Check day switch on the light.
                if (ltk == (ltk & _onDaySwitchOn))
                    foreach (var l in _lights[ltk])
                        l.RunLightOn();

                // Check day switch off the light.
                if (ltk == (ltk & _onDaySwitchOff))
                    foreach(var l in _lights[ltk])
                        l.RunLightOff();
            }
        }

        /// <summary>
        /// It is night time and start turning the light.
        /// </summary>
        public void TurnOff()
        {
            foreach (var ltk in _lights.Keys)
            {
                // Check night switch on the light.
                if (ltk == (ltk & _onNightSwitchOn))
                    foreach (var l in _lights[ltk])
                        l.RunLightOn();

                // Check night switch off the light.
                if (ltk == (ltk & _onNightSwitchOff))
                    foreach (var l in _lights[ltk])
                        l.RunLightOff();
            }
        }

        #endregion

        #region Main

        private void ListenRegisterLightCallback(Object lightObj)
        {
            // Check validation.
            if (lightObj is not Light2DSwitchFunc) return;

            // Register light, manually check for better performance with valid registration.
            Light2DSwitchFunc light = (Light2DSwitchFunc)lightObj;
            if (InGameLightingType.Unknown == (light.LightType & InGameLightingType.Unknown))
                _lights[InGameLightingType.Unknown].Add(light);
            if (InGameLightingType.Global == (light.LightType & InGameLightingType.Global))
                _lights[InGameLightingType.Global].Add(light);
            if (InGameLightingType.Primary == (light.LightType & InGameLightingType.Primary))
                _lights[InGameLightingType.Primary].Add(light);
            if (InGameLightingType.Secondary == (light.LightType & InGameLightingType.Secondary))
                _lights[InGameLightingType.Secondary].Add(light);
            if (InGameLightingType.Tertiary == (light.LightType & InGameLightingType.Tertiary))
                _lights[InGameLightingType.Tertiary].Add(light);
            if (InGameLightingType.Quartenary == (light.LightType & InGameLightingType.Quartenary))
                _lights[InGameLightingType.Quartenary].Add(light);
            if (InGameLightingType.Quinary == (light.LightType & InGameLightingType.Quinary))
                _lights[InGameLightingType.Quinary].Add(light);
            if (InGameLightingType.Senary == (light.LightType & InGameLightingType.Senary))
                _lights[InGameLightingType.Senary].Add(light);
            if (InGameLightingType.Septenary == (light.LightType & InGameLightingType.Septenary))
                _lights[InGameLightingType.Septenary].Add(light);
            if (InGameLightingType.Octonary == (light.LightType & InGameLightingType.Octonary))
                _lights[InGameLightingType.Octonary].Add(light);
            if (InGameLightingType.Nonary == (light.LightType & InGameLightingType.Nonary))
                _lights[InGameLightingType.Nonary].Add(light);
            if (InGameLightingType.Denary == (light.LightType & InGameLightingType.Denary))
                _lights[InGameLightingType.Denary].Add(light);
        }

        private void ListenOnTickDataPercentage(float percentInDay)
        {
            // Check light switching.
            bool isDayTime = percentInDay >= _dayTimePercent.x && percentInDay < _dayTimePercent.y;
            if (isDayTime && !_isPreviouslyDaytime)
            {
                TurnOn();
                _isPreviouslyDaytime = true;
            }
            else if (!isDayTime && _isPreviouslyDaytime)
            {
                TurnOff();
                _isPreviouslyDaytime = false;
            }
        }

        #endregion
    }
}
