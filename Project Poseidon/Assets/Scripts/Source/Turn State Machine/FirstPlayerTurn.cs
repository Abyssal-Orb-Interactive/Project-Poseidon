using UnityEngine;

namespace Source.Turn_State_Machine
{
    public class FirstPlayerTurn : TurnState
    {
        public FirstPlayerTurn(){}
        public FirstPlayerTurn(TurnState nextState) : base(nextState)
        {
        }

        public override void Enter()
        {
            Debug.Log("FirstPlayerTurn Starts");
        }

        public override void Exit()
        {
            Debug.Log("FirstPlayerTurn Ends");
        }

        public void ChangeNextState(TurnState state)
        {
            NextState = state;
        }

        public override void Dispose()
        {
            NextState?.Dispose();
            base.Dispose();
        }
    }
}
