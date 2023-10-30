using System;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle runtime ammunition information object.
    /// </summary>
    [Serializable]
    public sealed class RuntimeAmmoInfo : IAmmo, ICloneable, IInjectDependency<UI_AmmoInfo>
    {
        #region Variables

        [Header("Properties")]
        [SerializeField, Min(-1)]
        private int _maxBagAmmo = -1; // Minus one means infinite.

        [SerializeField, Min(-1)]
        private int _maxAmmo = -1; // Minus one means infinite.

        // Runtime variable data.
        private UI_AmmoInfo _ammoInfo = null;
        private int _ammoInBag = 0;
        private int _ammoLeft = 0;

        #endregion

        #region IAmmo

        public int MaxAmmoInBag
        {
            get => _maxBagAmmo;
            internal set => _maxBagAmmo = value;
        }

        public int AmmoInBag
        {
            get => _ammoInBag;
            internal set
            {
                _ammoInBag = value;
                if (_ammoInfo != null)
                    _ammoInfo.SetInfo(_ammoLeft, _ammoInBag);
            }
        }

        public int MaxAmmo
        {
            get => _maxAmmo;
            internal set => _maxAmmo = value;
        }

        public int Ammo
        {
            get => _ammoLeft;
            internal set
            {
                _ammoLeft = value;
                if (_ammoInfo != null)
                    _ammoInfo.SetInfo(_ammoLeft, _ammoInBag);
            }
        }

        public void AddAmmoToBag(int amount)
        {
            // Ignore infinite ammo.
            if (_maxBagAmmo == -1) return;

            // Add ammo by amount.
            _ammoInBag += amount;
            if (_ammoInBag > _maxBagAmmo) _ammoInBag = _maxBagAmmo;
            if (_ammoInfo != null) _ammoInfo.SetInfo(_ammoLeft, _ammoInBag);
        }

        public int UseAmmoInBarrel(int amount)
        {
            _ammoLeft -= amount;
            if (_ammoLeft < 0)
            {
                amount += _ammoLeft;
                _ammoLeft = 0;
            }
            if (_ammoInfo != null) _ammoInfo.SetInfo(_ammoLeft, _ammoInBag);
            return amount;
        }

        public void TransferAmmo()
        {
            // Ignore infinite ammo.
            if (_maxAmmo == -1) return;

            // Fill by infinite max ammo in bag.
            if (_maxBagAmmo == -1)
            {
                Ammo = _maxAmmo;
                return;
            }

            // Fill by leftover ammo in bag.
            int tempExpectedFill = _maxAmmo - _ammoLeft;
            _ammoInBag -= tempExpectedFill;
            if (_ammoInBag < 0)
            {
                tempExpectedFill += _ammoInBag;
                _ammoInBag = 0;
            }
            _ammoLeft += tempExpectedFill;
            if (_ammoInfo != null) _ammoInfo.SetInfo(_ammoLeft, _ammoInBag);
        }

        public void ResetAmmo() => Ammo = _maxAmmo;

        public void ResetAmmoBag() => AmmoInBag = _maxBagAmmo;

        #endregion

        #region ICloneable

        public object Clone()
        {
            var cloned = new RuntimeAmmoInfo();
            cloned._maxBagAmmo = _maxBagAmmo;
            cloned._maxAmmo = _maxAmmo;
            return cloned;
        }

        #endregion

        #region IDependencyInject

        public void Inject(UI_AmmoInfo instance = null)
        {
            _ammoInfo = instance;
            if (_ammoInfo != null)
                _ammoInfo.SetInfo(_ammoLeft, _ammoInBag);
            _ammoInfo?.gameObject.SetActive(_ammoInfo != null);
        }

        #endregion
    }
}
