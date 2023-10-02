using System;
using UnityEngine;

namespace Source.Turn_State_Machine
{
    [RequireComponent(typeof(GameManager))]
    public class TurnMachine : MonoBehaviour
    {
        private TurnState _currentState;
        private TimerUniTaskAdapter _timer;
        [SerializeField] private GameManager _gameManager;
        
        private void OnValidate()
        {
            _gameManager ??= GetComponent<GameManager>();
        }

        private void Start()
        {
            _timer = new TimerUniTaskAdapter(5f);
            _gameManager.TurnEnded += ChangeState;
            _timer.TimeEnded += ChangeState;
            
            InitializeStates();
            
            _currentState.Enter();
            _timer.StartTimerAsync();
        }

        private void InitializeStates()
        {
            var first = new FirstPlayerTurn();
            var second = new SecondPlayerTurn(first);
            first.ChangeNextState(second);
            _currentState = first;
        }

        private void Update()
        {
            _currentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            _currentState.PhysicsUpdate();
        }
        
        private void ChangeState()
        {
            _currentState.Exit();
            _currentState = _currentState.NextState;
            _currentState.Enter();
            _timer.StartTimerAsync();
        }

        private void OnDisable()
        {
            _timer.StopTimer();
            _timer.Destroy();
        }
    } 
}

