using System;
using Source.Battle_Field;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.Exceptions;

namespace Source.Input
{
    public class ShootHandler
    {
        private readonly PlayerActions _actions;
        private Vector2Int? _coord;
        private Camera _camera;
        
        public ShootHandler(PlayerActions actions, Camera camera)
        {
            _actions = actions;
            _camera = camera;
            
            Enable();
        }

        public Vector2Int GetCoord()
        {
            if(_coord == null) throw new InvalidOperationException("Player hasn't clicked yet.");
            
            var coord = (Vector2Int) _coord;
            _coord = null;
            return coord;
        }

        public void Enable()
        {
            _actions.Base.Shoot.performed += _ => CalculateCoord();
        }

        public void Disable()
        {
            _actions.Base.Shoot.performed -= _ => CalculateCoord();
        }
        private void CalculateCoord()
        {
            var mouseScreenPos = Mouse.current.position.ReadValue();
            _coord = MousePositionCalculator.CalculateMousePosition(mouseScreenPos, _camera);
        }
    }
}
