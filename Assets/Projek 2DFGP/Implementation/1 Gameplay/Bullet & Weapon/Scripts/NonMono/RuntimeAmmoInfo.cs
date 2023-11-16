using System;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle runtime ammunition information object.
    /// </summary>
    [Serializable]
    public sealed class RuntimeAmmoInfo : RuntimeInfo, IAmmo, IRequiredReset, ICloneable,
        ICopyValue<RuntimeAmmoInfo>, IObjectID<string>
    {
        #region Variables

        /// <summary>
        /// A fixed object type as ID.
        /// </summary>
        public const string FID = "AmmoInfo";

        [Header("Properties")]
        [SerializeField, Min(-1)]
        private int _maxBagAmmo = -1; // Minus one means infinite.

        [SerializeField, Min(-1)]
        private int _maxAmmo = -1; // Minus one means infinite.

        // Runtime variable data.
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
            internal set => _ammoInBag = value;
        }

        public int MaxAmmo
        {
            get => _maxAmmo;
            internal set => _maxAmmo = value;
        }

        public int Ammo
        {
            get => _ammoLeft;
            internal set => _ammoLeft = value;
        }

        public bool IsAmmoBagFull => AmmoInBag >= MaxAmmoInBag;

        public void AddAmmoToBag(int amount)
        {
            // Ignore infinite ammo.
            if (_maxBagAmmo == -1) return;

            // Add ammo by amount.
            _ammoInBag += amount;
            if (_ammoInBag > _maxBagAmmo) _ammoInBag = _maxBagAmmo;
        }

        public int UseAmmoInBarrel(int amount)
        {
            _ammoLeft -= amount;
            if (_ammoLeft < 0)
            {
                amount += _ammoLeft;
                _ammoLeft = 0;
            }
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
        }

        public void ResetAmmo() => Ammo = _maxAmmo;

        public void ResetAmmoBag() => AmmoInBag = _maxBagAmmo;

        #endregion

        #region IRequiredReset

        // Short command to reset everything.
        public void Reset()
        {
            ResetAmmoBag();
            ResetAmmo();
        }

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

        #region ICopyValue

        public void CopyValue(RuntimeAmmoInfo target)
        {
            target._maxBagAmmo = _maxBagAmmo;
            target._maxAmmo = _maxAmmo;
            target.AmmoInBag = _ammoInBag;
            target.Ammo = _ammoLeft;
        }

        #endregion

        #region IEntityID

        public string ObjectID => FID;

        #endregion
    }
}
