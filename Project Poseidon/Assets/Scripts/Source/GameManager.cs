using System;
using System.Collections.Generic;
using Base.Timers;
using Source.Battle_Field;
using Source.Graphics;
using Source.Graphics.Markers;
using Source.Graphics.UI;
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
        [SerializeField] private TimerLine _timerLine;
        
        private AmmoController _ammoController;
        private Timer _timer;
        private TimeInvoker _timeInvoker;

        private void Start()
        {
            _timeInvoker = TimeInvoker.Instance;
            _grid = new Grid(GridFabric.CreateGrid());
            _grid.ShipExplosion += VisualizeExplosionMarkers;
            _gridVisualizer.Initialize(_grid);
            _gridVisualizer.Visualize();
            _opensTypeIdentifier = new OpensTypeIdentifier(_grid);
            ShipFabric.Initialize(_shipsPack);
            var shipPlacer = new ShipPlacer(_grid, _fleet);

            shipPlacer.TryPlaceShips();
            MarkerFabric.Initialize(_markersPack);
            
            _shipVisualizer.AddShips(shipPlacer.GetAllShips());
            _shipVisualizer.Visualize();

            _actions = new PlayerActions();
            _actions.Enable();
            _shootHandler = new ShootHandler(_actions, _camera);

            _actions.Base.Shoot.performed += _ => Shoot();

            _ammoController = new AmmoController();
            
            _ammoController.AmmunitionIsEmpty += OnEndTurn;

            _timer = TimerFabric.Create(TimerType.UnscaledFrameTimer, 5f);
            _timer.TimerFinished += OnEndTurn;
            _timerLine.Initialize(_timer);
            _timer.Start();
        }

        private void Update()
        {
            _timeInvoker.UpdateTimer();
            _timerLine.UpdateTimeBar();
        }

        private void Shoot()
        {
            var shootCoord = _shootHandler.GetCoord();
            var opener = new Opener(shootCoord);
            
            if (!_grid.TryOpenCells(opener)) return;

            var type = _opensTypeIdentifier.GetType(shootCoord, opener);

            if (type != OpenType.Hit)
            {
                _ammoController.TakeAmmo();
            }
            
            _shipVisualizer.VisualizeHit(shootCoord);
            _markersVisualizer.AddMarker(shootCoord, type);
            _markersVisualizer.Visualize();
        }

        private void VisualizeExplosionMarkers()
        {
            var opener = _grid.GetExplosion();
            var coords = (IReadOnlyList<Vector2Int>)opener.GetOpenInformation();
            if(_grid.TryOpenCells(opener)) _markersVisualizer.AddMarkers(coords, new List<OpenType>(_opensTypeIdentifier.GetTypes(coords, opener)));
            _markersVisualizer.Visualize();
        }
        
        private void OnDisable()
        {
            _shootHandler.Disable();
            _actions.Base.Shoot.performed -= _ => Shoot();
            _ammoController.Dispose();
            _timer.Dispose();
        }

        private void OnEndTurn()
        {
            TurnEnded?.Invoke();
           _timer.Restart();
        }
    }
}
