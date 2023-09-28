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

        private void Start()
        {
            _grid = new Grid(GridGenerator.CreateGrid());
            _grid.ShipExplose += VisualizeExplosionMarkers;
            _gridVisualizer.Initialize(_grid);
            _opensTypeIdentifier = new OpensTypeIdentifier(_grid);
            ShipCreator.Initialize(_shipsPack);
            var shipPlacer = new ShipPlacer(_grid);
           
            shipPlacer.TryPlaceShip(new Vector2Int(5, 5), ShipType.TorpedoBoat);
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
            if(_grid.TryOpenCells(opener)) _markersVisualizer.PlaceMarker(shootCoord, _opensTypeIdentifier.GetType(shootCoord, opener)); 
        }

        private void VisualizeExplosionMarkers()
        {
            var opener = _grid.GetExplosion();
            var coords = (IReadOnlyList<Vector2Int>)opener.GetOpenInformation();
            if(_markersVisualizer != null) _markersVisualizer.PlaceMarkers(coords, (IReadOnlyList<TypeOfOpens>)_opensTypeIdentifier.GetTypes(coords, opener));
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
