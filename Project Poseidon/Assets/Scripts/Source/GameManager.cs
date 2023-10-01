using System;
using System.Collections.Generic;
using Source.Battle_Field;
using Source.Graphics;
using Source.Graphics.Markers;
using Source.Input;
using Source.Ships;
using UnityEngine;
using Grid = Source.Battle_Field.Grid;

namespace Source
{
    public class GameManager : MonoBehaviour
    {
        private Grid _grid;
        private PlayerActions _actions;
        private ShootHandler _shootHandler;
        private OpensTypeIdentifier _opensTypeIdentifier;
        public event Action TurnEnded;

        [SerializeField] private GridVisualizer _gridVisualizer;
        [SerializeField] private MarkersVisualizer _markersVisualizer;
        [SerializeField] private ShipVisualizer _shipVisualizer;
        [SerializeField] private Camera _camera;
        [SerializeField] private MarkersPack _markersPack;
        [SerializeField] private ShipsPack _shipsPack;
        [SerializeField] private Fleet _fleet;

        private void Start()
        {
            _grid = new Grid(GridFabric.CreateGrid());
            _grid.ShipExplosion += VisualizeExplosionMarkers;
            _gridVisualizer.Initialize(_grid);
            _opensTypeIdentifier = new OpensTypeIdentifier(_grid);
            ShipFabric.Initialize(_shipsPack);
            var shipPlacer = new ShipPlacer(_grid, _fleet);

            shipPlacer.TryPlaceShips();
            MarkerCreator.Initialize(_markersPack);
            
            _shipVisualizer.VisualizeShips(shipPlacer.GetAllShips());

            _actions = new PlayerActions();
            _actions.Enable();
            _shootHandler = new ShootHandler(_actions, _camera);

            _actions.Base.Shoot.performed += _ => Shoot();
            
        }

        private void Shoot()
        {
            var shootCoord = _shootHandler.GetCoord();
            var opener = new Opener(shootCoord);
            
            if (!_grid.TryOpenCells(opener)) return;
            
            _shipVisualizer.VisualizeHit(shootCoord);
            _markersVisualizer.PlaceMarker(shootCoord, _opensTypeIdentifier.GetType(shootCoord, opener));
        }

        private void VisualizeExplosionMarkers()
        {
            var opener = _grid.GetExplosion();
            var coords = (IReadOnlyList<Vector2Int>)opener.GetOpenInformation();
            if(_grid.TryOpenCells(opener)) _markersVisualizer.PlaceMarkers(coords, new List<OpenType>(_opensTypeIdentifier.GetTypes(coords, opener)));
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
