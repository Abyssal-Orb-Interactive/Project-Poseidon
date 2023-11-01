using System;
using System.Collections.Generic;
using Source.Input;
using Source.Turn_State_Machine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Source
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Elements")] 
        [SerializeField] private Camera _camera;
        [SerializeField] private CameraMover _cameraMover;
        [SerializeField] private Transform[] _cameraTargets;
        [SerializeField] private Visualizer _visualizer;
        [SerializeField] private ShipsManager _shipsManager;
        [SerializeField] private GameObject _sgipsPlacerMenu;
        [SerializeField] private GameObject _battleMenu;
        [SerializeField] private GameObject _menu;
        [SerializeField] private float _turnTime = 10f;
        [SerializeField] private Vector2Int _firstPlayerFieldOffset = new (0,0);
        [SerializeField] private Vector2Int _secondPlayerFieldOffset = new (20, 0);
        
        private PlayersManager _playersManager;
        private TimeToTurnTracker _timeToTurnTracker;
        private ShootController _shootController;
        private PlayerActions _actions;
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
            _shipsManager.UpdateGhostPosition(MousePositionCalculator.CalculateMousePosition(Mouse.current.position.ReadValue(), _camera));
        }
        
        private void InitializeGameComponents()
        {
            _actions = new PlayerActions();
            _actions.Enable();
            _actions.Base.Exit.performed += _ => OnExit();
            _timeToTurnTracker = new TimeToTurnTracker(_turnTime);
            
            var first = new Player(_firstPlayerFieldOffset, _cameraTargets[(int)Players.First]);
            var second = new Player(_secondPlayerFieldOffset, _cameraTargets[(int)Players.Second]);
            _playersManager = new PlayersManager(new List<Player>{ first, second });
            
            _visualizer.InitializeGridVisualizers(_playersManager, Players.First);
            _visualizer.InitializeGridVisualizers(_playersManager, Players.Second);
            
            _shipsManager.Initialize(_playersManager);
            _shipsManager.SubscribeToContinueDesired(PrepareBattle);
        }

        private void OnEndTurn()
        {
            _visualizer.MakeShipsInvisible((int)_playersManager.GetCurrentPlayerID());
            _playersManager.PassToNextPlayer();
            _visualizer.MakeShipsVisible((int)_playersManager.GetCurrentPlayerID());
            _cameraMover.ChangeMovingTarget(_playersManager.GetCurrentPlayer().GetBattlefield().GetCameraTarget());
            TurnEnded?.Invoke();
            _timeToTurnTracker.Restart();
        }

        private void PrepareBattle()
        {
            _cameraMover.ChangeMovingTarget(_playersManager.GetCurrentPlayer().GetBattlefield().GetCameraTarget());
            _shipsManager.SubscribeToContinueDesired(StartBattle);
        }

        private void StartBattle()
        {
            _sgipsPlacerMenu.SetActive(false);
            _battleMenu.SetActive(true);
            _shootController = new ShootController(_camera, _playersManager, _visualizer);
            _shootController.SubscribeOnAmmoEnded(OnEndTurn);
            _shootController.SubscribeOnHit(_timeToTurnTracker.Restart);
            _visualizer.MakeShipsInvisible((int)Players.Second);
            
            InitializeTimer(); 
        }
        
        private void InitializeTimer()
        {
            _timeToTurnTracker.SubscribeToTimeToTurnEnded(OnEndTurn);
            _visualizer.InitializeTimerLine(_timeToTurnTracker);
            _timeToTurnTracker.SubscribeToTimePaused(_shootController.OnPause);
            _timeToTurnTracker.SubscribeToTimeResumed(_shootController.OnResume);
            _timeToTurnTracker.Start();
        }

        public void Pause()
        {
            _timeToTurnTracker.Pause();
        }

        public void Resume()
        {
            _timeToTurnTracker.Resume();
        }

        private void OnExit()
        {
            _menu.SetActive(true);
            _timeToTurnTracker.Pause();
        }
    }
}
