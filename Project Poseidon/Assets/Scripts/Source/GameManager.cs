using System;
using System.Collections.Generic;
using Source.Ships;
using Source.Turn_State_Machine;
using UnityEngine;

namespace Source
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Elements")] 
        [SerializeField] private Camera _camera;
        [SerializeField] private ShipsPack _shipsPack;
        [SerializeField] private Fleet _fleet;
        [SerializeField] private CameraMover _cameraMover;
        [SerializeField] private Transform[] _cameraTargets;
        [SerializeField] private Visualizer _visualizer;
        
        private PlayersManager _playersManager;
        private TimeToTurnTracker _timeToTurnTracker;
        private ShootController _shootController;
        public event Action TurnEnded;

        private void Start()
        {
            InitializeGameComponents();
        }
        
        private void Update()
        {
            UpdateGameElements();
        }
        
        private void UpdateGameElements()
        {
            _timeToTurnTracker.UpdateTime();
            _cameraMover.Move();
        }
        
        private void InitializeGameComponents()
        {
            _timeToTurnTracker = new TimeToTurnTracker(5f);
            var first = new Player(new Vector2Int(0,0), _cameraTargets[(int)Players.First]);
            var second = new Player(new Vector2Int(20, 0), _cameraTargets[(int)Players.Second]);
            _playersManager = new PlayersManager(new List<Player>{ first, second });
            _shootController = new ShootController(_camera, _playersManager, _visualizer);
            _shootController.SubscribeOnAmmoEnded(OnEndTurn);
            _shootController.SubscribeOnHit(_timeToTurnTracker.Restart);
            _cameraMover.ChangeMovingTarget(_playersManager.GetCurrentPlayer().GetBattlefield().GetCameraTarget());
            _visualizer.InitializeGridVisualizers(_playersManager, Players.First);
            _visualizer.InitializeGridVisualizers(_playersManager, Players.Second);
            PlaceShips();
            InitializeTimer();
        }
        
        private void PlaceShips()
        {
            ShipFabric.Initialize(_shipsPack);
            var firstPlayerShipPlacer = new ShipPlacer(_playersManager.GetPlayerByID(Players.First).GetBattlefield().GetGrid(), _fleet);
            var secondPlayerShipPlacer = new ShipPlacer(_playersManager.GetPlayerByID(Players.Second).GetBattlefield().GetGrid(), _fleet);
            firstPlayerShipPlacer.TryPlaceShips();
            secondPlayerShipPlacer.TryPlaceShips();
            
            _visualizer.VisualizeShips(Players.First, firstPlayerShipPlacer);
            _visualizer.VisualizeShips(Players.Second, secondPlayerShipPlacer);
        }
        
        private void InitializeTimer()
        {
            _timeToTurnTracker.SubscribeToTimeToTurnEnded(OnEndTurn);
            _visualizer.InitializeTimerLine(_timeToTurnTracker);
            _timeToTurnTracker.Start();
        }

        private void OnEndTurn()
        {
            _playersManager.PassToNextPlayer();
            _cameraMover.ChangeMovingTarget(_playersManager.GetCurrentPlayer().GetBattlefield().GetCameraTarget());
            TurnEnded?.Invoke();
            _timeToTurnTracker.Restart();
        }
    }
}
