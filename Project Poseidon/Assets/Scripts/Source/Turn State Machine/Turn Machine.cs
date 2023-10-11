using Base;
using UnityEngine;

namespace Source.Turn_State_Machine
{
    [RequireComponent(typeof(GameManager))]
    public class TurnMachine : MonoBehaviour
    {
        private TurnState _currentState;
        [SerializeField] private GameManager _gameManager;
        
        private void OnValidate()
        {
            _gameManager ??= GetComponent<GameManager>();
        }

        private void Start()
        {
            _gameManager.TurnEnded += ChangeState; 
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

        private void OnDisable()
        {
        }
    } 
}

