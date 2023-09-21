using System;
using Source.Battle_Field;
using Source.Graphics;
using Source.Graphics.Markers;
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
        [SerializeField] private MarkersVisualizer _markersVisualizer;
        [SerializeField] private Camera _camera;
        [SerializeField] private MarkersPack _pack;

        private void Start()
        {
            _grid = new Grid(GridGenerator.CreateGrid());
            _gridVisualizer.Initialize(_grid);
            MarkerCreator.Initialize(_pack);

            _actions = new PlayerActions();
            _actions.Enable();
            _shootHandler = new ShootHandler(_actions, _camera);

            _actions.Base.Shoot.performed += _ => Shoot();
            
        }

        private void Shoot()
        {
            var shootCoord = _shootHandler.GetCoord();
            var opener = new Opener(shootCoord);
            if(_grid.TryOpenCells(opener)) _markersVisualizer.PlaceMarker(shootCoord, TypeOfOpens.Miss); 
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
