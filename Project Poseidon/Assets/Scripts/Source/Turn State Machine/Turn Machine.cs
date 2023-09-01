using System;
using UnityEngine;
using UnityEngine.Serialization;

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

        [FormerlySerializedAs("_gameManager")] [SerializeField] private GameManager gameManager = null;
        
        private void OnValidate()
        {
            gameManager ??= GetComponent<GameManager>();
        }

        private void Start()
        {
            gameManager.TurnEnded += ChangeState;
            InitializeStates();
            _currentState.Enter();
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
        }
    } 
}

