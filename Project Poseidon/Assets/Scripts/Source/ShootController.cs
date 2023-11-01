using System;
using Source.Battle_Field;
using Source.Input;
using UnityEngine;

namespace Source
{
    public class ShootController : IDisposable
    {
        private AmmoController _ammoController;
        private ShootHandler _shootHandler;
        private PlayerActions _actions;
        private PlayersManager _players;
        private Visualizer _visualizer;
        
        private event Action AmmoEnded;
        private event Action Shot;
        private event Action Hit;

        public ShootController(Camera camera, PlayersManager manager, Visualizer visualizer)
        {
                _actions = new PlayerActions();
                _actions.Enable();
                _shootHandler = new ShootHandler(_actions, camera);

                _players = manager;
                
                _ammoController = new AmmoController();
                _ammoController.AmmunitionIsEmpty += OnAmmoEnded;
                
                _actions.Base.Shoot.performed += _ => Shoot(_players.GetPlayerByID(_players.GetNextPlayerID()).GetBattlefield());

                _visualizer = visualizer;
        }
        
        
        private void Shoot(Battlefield battlefield)
        {
            var shootCoord = _shootHandler.GetCoord();
            var opener = new Opener(shootCoord);
            
            if (!battlefield.TryRegisterShoot(opener)) return;

            var type = battlefield.GetTypeOfShoot(shootCoord, opener);

            if (type == OpenType.Hit)
            {
                _visualizer.VisualizeHitOnShip(shootCoord, _players);
                OnHit();
            }
            else
            {
                _ammoController.TakeAmmo();
            }
            
            _visualizer.VisualizeMarker(shootCoord, type, _players);
            OnShot();
        }

        public void OnPause()
        {
            _actions.Disable();
        }

        public void OnResume()
        {
            _actions.Enable();
        }

        private void OnAmmoEnded()
        {
            _actions.Base.Shoot.performed -= _ =>  Shoot(_players.GetPlayerByID(_players.GetNextPlayerID()).GetBattlefield());
            AmmoEnded?.Invoke();
            _actions.Base.Shoot.performed += _ =>  Shoot(_players.GetPlayerByID(_players.GetNextPlayerID()).GetBattlefield());
        }
        
        public void SubscribeOnAmmoEnded(Action action)
        {
            AmmoEnded += action;
        }
        public void UnSubscribeOnAmmoEnded(Action action)
        {
            AmmoEnded -= action;
        }
        
        private void OnHit()
        {
            Hit?.Invoke();
        }
        
        public void SubscribeOnHit(Action action)
        {
            Hit += action;
        }
        public void USubscribeOnHit(Action action)
        {
            Hit -= action;
        }
        private void OnShot()
        {
            Shot?.Invoke();
        }
        
        public void SubscribeOnShot(Action action)
        {
            Shot += action;
        }
        public void UnSubscribeOnShot(Action action)
        {
            Shot -= action;
        }

        public PlayerActions GetActionsMap()
        {
            return _actions;
        }
        
        public void Dispose()
        {
            AmmoEnded = null;
            Shot = null;
            _ammoController.Dispose();
            _shootHandler.Dispose();
            _actions.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
