using System;
using Source.Battle_Field;
using Source.Graphics;
using Source.Input;
using UnityEngine;
using Grid = Source.Battle_Field.Grid;

namespace Source
{
    public class GameManager : MonoBehaviour
    {
        private Grid _grid;
        private PlayerActions _actions;
        private ShootHandler _shootHandler;
        public event Action TurnEnded;

        [SerializeField] private GridVisualizer _gridVisualizer;
        [SerializeField] private Camera _camera;

        private void Start()
        {
            _grid = new Grid(GridGenerator.CreateGrid());
            _gridVisualizer.Initialize(_grid);

            _actions = new PlayerActions();
            _actions.Enable();
            _shootHandler = new ShootHandler(_actions, _camera);

            _actions.Base.Shoot.performed += _ => Shoot();
            
        }

        private void Shoot()
        {
            var opener = new Opener(_shootHandler.GetCoord());
            _grid.OpenCells(opener);
        }

        private void OnDisable()
        {
            _shootHandler.Disable();
            _actions.Base.Shoot.performed -= _ => Shoot();
            
        }

        private void EndTurn()
        {
            TurnEnded?.Invoke();
        }
    }
}
