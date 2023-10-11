using System;
using Base;

namespace Source
{
    public class AmmoController : IDisposable
    {
        private const int AMMO_AMOUNT = 1;
        
        private CounterInt _counter;
        public int CurrentAmmoAmount => _counter.CurrentValue;

        public event Action AmmunitionIsEmpty;

        public AmmoController()
        {
            _counter = new CounterInt(AMMO_AMOUNT, 0, () => -1);
            _counter.TargetReached += OnAmmunitionEmpty;
        }
        
        public void TakeAmmo()
        {
            _counter.CalculateNextValue();
        }

        public void Dispose()
        {
            _counter.Dispose();
            GC.SuppressFinalize(this);
        }

        private void OnAmmunitionEmpty()
        {
            AmmunitionIsEmpty?.Invoke();
            _counter.Reset();
        }
    }
}