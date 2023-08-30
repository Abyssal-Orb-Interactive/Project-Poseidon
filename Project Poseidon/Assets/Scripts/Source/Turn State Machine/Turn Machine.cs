using System;
using UnityEngine;

namespace Source.Turn_State_Machine
{
    /// <summary>
    /// Turn state machine.
    /// </summary>
    [RequireComponent(typeof(GameManager))]
    public class TurnMachine : MonoBehaviour
    {
        /// <summary>
        /// Current state of Machine.
        /// </summary>
        private TurnState _currentState = null;

        [SerializeField] private GameManager _gameManager = null;
        
        private void OnValidate()
        {
            _gameManager ??= GetComponent<GameManager>();
        }

        private void Start()
        {
            _gameManager.TurnEnded += ChangeState;
            var first = new FirstPlayerTurn();
            var second = new SecondPlayerTurn(first);
            first.ChangeNextState(second);
            _currentState = first;
            _currentState.Enter();
        }

        private void Update()
        {
            Debug.Log(_currentState.ToString());
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
        }
    } 
}

