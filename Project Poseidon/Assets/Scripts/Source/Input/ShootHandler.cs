using System;
using Source.Battle_Field;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Input
{
    public class ShootHandler
    {
        private readonly PlayerActions _actions;
        private Opener _opener;
        private Camera _camera;
        
        public ShootHandler(PlayerActions actions, Camera camera)
        {
            _actions = actions;
            _camera = camera;
            
            Enable();
        }

        public Opener GetOpener()
        {
            if (_opener == null) throw new InvalidOperationException("Player hasn't clicked yet.");

            var opener = _opener;
            _opener = null;
            return opener;
        }

        public void Enable()
        {
            _actions.Base.Shoot.performed += _ => PrepareOpener();
        }

        public void Disable()
        {
            _actions.Base.Shoot.performed -= _ => PrepareOpener();
        }
        private void PrepareOpener()
        {
            var mouseScreenPos = Mouse.current.position.ReadValue();
            _opener = new Opener(MousePositionCalculator.CalculateMousePosition(mouseScreenPos, _camera));
        }
    }
}
