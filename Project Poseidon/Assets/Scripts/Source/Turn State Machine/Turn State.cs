using System;

namespace Source.Turn_State_Machine
{
    public abstract class TurnState : IDisposable
    {
        public TurnState NextState { get; protected set; }

        protected TurnState(){}
        protected TurnState(TurnState nextState)
        {
            NextState = nextState;
        }
        
        public virtual void Enter(){}
        public virtual void Exit(){}
        public virtual void LogicUpdate(){}
        public virtual void PhysicsUpdate(){}

        public virtual void Dispose()
        {
            NextState?.Dispose();
        }
    }
}
