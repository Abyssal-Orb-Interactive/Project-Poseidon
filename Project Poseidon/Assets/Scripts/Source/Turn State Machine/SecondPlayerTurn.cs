using UnityEngine;

namespace Source.Turn_State_Machine
{
    public class SecondPlayerTurn : TurnState
    {
        public SecondPlayerTurn(TurnState nextState) : base(nextState)
        {
        }
        
        public override void Enter()
        {
            Debug.Log("SecondPlayerTurn Starts");
        }

        public override void Exit()
        {
            Debug.Log("SecondPlayerTurn Ends");
        }
        
        public override void Dispose()
        {
            NextState?.Dispose();
            base.Dispose();
        }
    }
}
