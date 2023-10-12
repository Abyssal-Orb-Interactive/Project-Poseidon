using System;
using System.Collections.Generic;
using Base.Timers;
using Source.Battle_Field;
using Source.Graphics;
using Source.Graphics.Markers;
using Source.Graphics.UI;
using Source.Input;
using Source.Ships;
using Source.Turn_State_Machine;
using UnityEngine;
using Grid = Source.Battle_Field.Grid;

namespace Source
{
    public class GameManager : MonoBehaviour
    {
        private Grid _firstPlayerGrid;
        private Grid _secondPlayerGrid;
        private PlayerActions _actions;
        private ShootHandler _shootHandler;
        private OpensTypeIdentifier _firstPlayerOpensTypeIdentifier;
        private OpensTypeIdentifier _secondPlayerOpensTypeIdentifier;
        public event Action TurnEnded;

        [SerializeField] private GridVisualizer _firstPlayerGridVisualizer;
        [SerializeField] private GridVisualizer _secondPlayerGridVisualizer;
        [SerializeField] private MarkersVisualizer _markersVisualizer;
        [SerializeField] private ShipVisualizer _firstPlayrtShipVisualizer;
        [SerializeField] private ShipVisualizer _secondPlayrtShipVisualizer;
        [SerializeField] private Camera _camera;
        [SerializeField] private MarkersPack _markersPack;
        [SerializeField] private ShipsPack _shipsPack;
        [SerializeField] private Fleet _fleet;
        [SerializeField] private TimerLine _timerLine;
        [SerializeField] private CameraMover _cameraMover;
        
        private AmmoController _ammoController;
        private Timer _timer;
        private TimeInvoker _timeInvoker;
        private Players _player = Players.First;

        private void Start()
        {
            _timeInvoker = TimeInvoker.Instance;
            TimerFabric.Initialize(_timeInvoker);
            
            _firstPlayerGrid = new Grid(GridFabric.CreateGrid(0,0));
            _secondPlayerGrid = new Grid(GridFabric.CreateGrid(20, 0));
            
            _firstPlayerOpensTypeIdentifier = new OpensTypeIdentifier(_firstPlayerGrid);
            _secondPlayerOpensTypeIdentifier = new OpensTypeIdentifier(_secondPlayerGrid);
            
            _firstPlayerGrid.ShipExplosion += () =>VisualizeExplosionMarkers(_firstPlayerGrid, _firstPlayerOpensTypeIdentifier);
            _secondPlayerGrid.ShipExplosion += () => VisualizeExplosionMarkers(_secondPlayerGrid, _secondPlayerOpensTypeIdentifier);
            
            _firstPlayerGridVisualizer.Initialize(_firstPlayerGrid);
            _firstPlayerGridVisualizer.Visualize();
            _secondPlayerGridVisualizer.Initialize(_secondPlayerGrid);
            _secondPlayerGridVisualizer.Visualize();
            
            ShipFabric.Initialize(_shipsPack);
            
            var firstPlayerShipPlacer = new ShipPlacer(_firstPlayerGrid, _fleet);
            var secondPlayerShipPlacer = new ShipPlacer(_secondPlayerGrid, _fleet);
            firstPlayerShipPlacer.TryPlaceShips();
            secondPlayerShipPlacer.TryPlaceShips();
            
            
            
            _firstPlayrtShipVisualizer.AddShips(firstPlayerShipPlacer.GetAllShips());
            _secondPlayrtShipVisualizer.AddShips(secondPlayerShipPlacer.GetAllShips());
            _firstPlayrtShipVisualizer.Visualize();
            _secondPlayrtShipVisualizer.Visualize();
            
            _cameraMover.ChangeMovingTarget(_secondPlayrtShipVisualizer.transform);

            _actions = new PlayerActions();
            _actions.Enable();
            _shootHandler = new ShootHandler(_actions, _camera);

            _actions.Base.Shoot.performed += _ => Shoot(GetCurrentPlayerGrid(), GetCurrentPlayerOpensTypeIdentifier(), GetCurrentPlayerShipVisualizer());

            _ammoController = new AmmoController();
            
            _ammoController.AmmunitionIsEmpty += OnEndTurn;

            _timer = TimerFabric.Create(TimerType.UnscaledFrameTimer, 5f);
            _timer.TimerFinished += OnEndTurn;
            MarkerFabric.Initialize(_markersPack);
            _timerLine.Initialize(_timer);
            _timer.Start();
        }
        
        private void Update()
        {
            _timeInvoker.UpdateTimer();
            _timerLine.UpdateTimeBar();
            _cameraMover.Move();
        }

        private void Shoot(Grid grid , OpensTypeIdentifier opensTypeIdentifier, ShipVisualizer shipVisualizer)
        {
            var shootCoord = _shootHandler.GetCoord();
            var opener = new Opener(shootCoord);
            
            if (!grid.TryOpenCells(opener)) return;

            var type = opensTypeIdentifier.GetType(shootCoord, opener);

            if (type == OpenType.Hit)
            {
                _timer.Restart();
                shipVisualizer.VisualizeHit(shootCoord);
            }
            else
            {
                _ammoController.TakeAmmo();
            }
            
            _markersVisualizer.AddMarker(shootCoord, type);
            _markersVisualizer.Visualize();
        }

        private void VisualizeExplosionMarkers(Grid grid, OpensTypeIdentifier opensTypeIdentifier)
        {
            var opener = grid.GetExplosion();
            var coords = (IReadOnlyList<Vector2Int>)opener.GetOpenInformation();
            if(grid.TryOpenCells((IOpener)opener)) _markersVisualizer.AddMarkers(coords, new List<OpenType>(opensTypeIdentifier.GetTypes(coords, (IOpener)opener)));
            _markersVisualizer.Visualize();
        }
        
        private void OnDisable()
        {
            _shootHandler.Disable();
            _actions.Base.Shoot.performed -= _ => Shoot(GetCurrentPlayerGrid(), GetCurrentPlayerOpensTypeIdentifier(), GetCurrentPlayerShipVisualizer());
        }

        private Grid GetCurrentPlayerGrid()
        {
            return _player == Players.First ? _secondPlayerGrid : _firstPlayerGrid;
        }
        
        private OpensTypeIdentifier GetCurrentPlayerOpensTypeIdentifier()
        {
            return _player == Players.First ? _secondPlayerOpensTypeIdentifier : _firstPlayerOpensTypeIdentifier;
        }
        
        private ShipVisualizer GetCurrentPlayerShipVisualizer()
        {
            return _player == Players.First ? _secondPlayrtShipVisualizer : _firstPlayrtShipVisualizer;
        }
        
        private void NextPlayer()
        {
            var playerCount = Enum.GetNames(typeof(Players)).Length;
            var nextIndex = (int)_player + 1;
            _player = (nextIndex >= playerCount) ? Players.First : (Players)nextIndex;
        }
        
        private void OnEndTurn()
        {
            _actions.Base.Shoot.performed -= _ => Shoot(GetCurrentPlayerGrid(), GetCurrentPlayerOpensTypeIdentifier(), GetCurrentPlayerShipVisualizer());
            NextPlayer();
            _cameraMover.ChangeMovingTarget(GetCurrentPlayerShipVisualizer().transform);
            _actions.Base.Shoot.performed += _ => Shoot(GetCurrentPlayerGrid(), GetCurrentPlayerOpensTypeIdentifier(), GetCurrentPlayerShipVisualizer());
            TurnEnded?.Invoke();
           _timer.Restart();
        }
    }
}
